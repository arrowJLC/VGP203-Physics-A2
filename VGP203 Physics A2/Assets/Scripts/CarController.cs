using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class CarControllers : MonoBehaviour
{
    [Header("Wheel Colliders")]
    public WheelCollider frontLeftCollider;
    public WheelCollider frontRightCollider;
    public WheelCollider rearLeftCollider;
    public WheelCollider rearRightCollider;

    [Header("Wheel Transforms (Optional Visuals)")]
    public Transform frontLeftTransform;
    public Transform frontRightTransform;
    public Transform rearLeftTransform;
    public Transform rearRightTransform;

    [Header("Car Physics Settings")]
    public float engineForce = 1500f;
    public float carMass = 1500f;
    public float drag = 0.1f;
    public float maxSpeed = 50f;
    public float maxReverse = 10f;
    public float steeringSpeed = 0.5f;
    public float maxSteerAngle = 35f;
    public float brakeForce = 5000f;

    public float currentSpeed;

    [Header("Jump Settings")]
    public float jumpForce = 5f;
    public LayerMask groundLayer;
    public float groundCheckDistance = 0.2f;

    private bool isGrounded;

    [Header("Drift Settings")]
    public float driftFactor = 0.95f;
    public float driftSteeringBoost = 2.5f;

    private bool isDrifting = false;
    private bool isHBrake = false;


    [Header("Acceleration Settings")]
    public float accelerationMultiplier = 1f;
    public float minAccelerationMultiplier = 1f;
    public float maxAccelerationMultiplier = 10f;
    public float accelerationRampUpTime = 5f;

    private float accelerationTimer = 0f;

    [Header("Misc Settings")]
    private Rigidbody rb;
    private Vector3 velocity;
    private float moveInput;
    private float steerInput;

    public bool inCar = false;
   
    protected int health;
    public Slider healthSlider;
    public TMP_Text speedText;
    WinCondition winCon;

    public GameObject winScreen;
    public GameObject loseScreen;
    public GameObject finishLine;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        winCon = FindFirstObjectByType<WinCondition>();
        //rb.isKinematic = true;
        health = 5;

        loseScreen.gameObject.SetActive(false);
        winScreen.gameObject.SetActive(false);
        finishLine.gameObject.SetActive(false);

        if (healthSlider != null )
        {
            //Debug.Log("health bar is active");
            healthSlider.value = health;
        }
    }
    private void Update()
    {
        moveInput = Input.GetAxis("Vertical");

        float targetSteer = Input.GetAxis("Horizontal");
        steerInput = Mathf.Lerp(steerInput, targetSteer, 100f * Time.deltaTime); // tweak 5f for responsiveness

        if (inCar)
        {
            UpdateAccelerationMultiplier();
            Steer();
            Brake();
            ApplyKinematicMovement();
            ApplyWheelForces();
            UpdateWheelVisuals();

            //if (Input.GetKeyDown(KeyCode.J) && IsGrounded())
            if (Input.GetKeyDown(KeyCode.J))
            {
                Jump();
                Debug.Log("Jump triggered");
            }

            isDrifting = Input.GetKey(KeyCode.P);
            isHBrake = Input.GetKey(KeyCode.LeftShift);

            if (Input.GetKeyDown(KeyCode.C))
            {
                PlayerControllers[] players = FindObjectsByType<PlayerControllers>(FindObjectsInactive.Include, FindObjectsSortMode.None);

                foreach (PlayerControllers player in players)
                {
                    if (player.isInside)
                    {
                        if (currentSpeed <= 0)
                        {
                            player.gameObject.SetActive(true);
                            player.LeaveCar(this);
                            player.leftCar = false;
                        }
                    }
                }
            }
        }

        healthSlider.value = health;
        checkForDamage();
        speedText.text = $"{(int)currentSpeed} km/h";
    }

    void ApplyKinematicMovement()
    {
        float appliedForce = moveInput * engineForce;

        // Acceleration using F = ma
        Vector3 acceleration = transform.forward * (appliedForce / carMass) * accelerationMultiplier;

        // Drag
        velocity *= (1 - drag * Time.deltaTime);

        // Apply acceleration
        velocity += acceleration * Time.deltaTime;

        Vector3 localVels = transform.InverseTransformDirection(velocity);
        localVels.x *= 0.9f; // Stronger lateral damping
        velocity = transform.TransformDirection(localVels);

        if (isHBrake && velocity.magnitude > 1f)
        {
            Vector3 velocityDir = velocity.normalized;
            Vector3 forwardDir = transform.forward;

            float angleDiff = Vector3.SignedAngle(forwardDir, velocityDir, Vector3.up);
            float driftRotationSpeed = angleDiff * 2f * Time.deltaTime;

            transform.Rotate(0f, driftRotationSpeed, 0f);
        }

        if (isDrifting)
        {
            Vector3 localVel = transform.InverseTransformDirection(velocity);

            // Introduce intentional sideways movement
            float driftSlip = steerInput * 5f; // tweak this value
            localVel.x += driftSlip * Time.deltaTime;

            // Optional: reduce forward grip slightly
            localVel.z *= 0.98f;

            velocity = transform.TransformDirection(localVel);
        }

        // Clamp speed
        float forwardSpeed = Vector3.Dot(velocity, transform.forward);
        if (forwardSpeed > maxSpeed)
        {
            velocity = transform.forward * maxSpeed;
        }
        else if (forwardSpeed < -maxReverse)
        {
            velocity = transform.forward * -maxReverse;
        }

        // Move
        transform.position += velocity * Time.deltaTime;

        currentSpeed = velocity.magnitude * 3.6f; // km/h

        float alignment = Vector3.Dot(transform.forward.normalized, velocity.normalized);

        if (alignment < 0.3f && moveInput > 0f)
        {
            // Too sideways — stop forward driving force
            velocity *= 0.95f; // slow it down, or set to Vector3.zero for hard stop
        }
    }

    public void SteerInput(float input)
    {
        steerInput = input;
    }

    void ApplyWheelForces()
    {
        float motorTorque = moveInput * engineForce;
        //float steerAngle = steerInput * steeringSpeed;

        // Apply rear-wheel torque
        rearLeftCollider.motorTorque = motorTorque;
        rearRightCollider.motorTorque = motorTorque;

        // Steering to front wheels
        //frontLeftCollider.steerAngle = steerAngle;
        //frontRightCollider.steerAngle = steerAngle;
        float steerAngle = steerInput * maxSteerAngle;
        frontLeftCollider.steerAngle = steerAngle;
        frontRightCollider.steerAngle = steerAngle;
    }
    void Brake()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            float speed = velocity.magnitude;
            if (speed > 0.1f)
            {
                // Use braking force to calculate a deceleration value
                float brakingPower = (brakeForce / carMass) * Time.deltaTime;

                // Apply deceleration in the opposite direction of current velocity
                Vector3 braking = velocity.normalized * brakingPower;

                // Only subtract braking if it's less than the current speed
                if (braking.magnitude < velocity.magnitude)
                    velocity -= braking;
                else
                    velocity = Vector3.zero;
            }
        }
        else if (Mathf.Approximately(moveInput, 0))
        {
            // Gentle rolling resistance when no throttle is applied
            velocity = Vector3.MoveTowards(velocity, Vector3.zero, (drag * 2f) * Time.deltaTime);
        }
    }

    void ApplyBrakes(float force)
    {
        frontLeftCollider.brakeTorque = force;
        frontRightCollider.brakeTorque = force;
        rearLeftCollider.brakeTorque = force;
        rearRightCollider.brakeTorque = force;
    }

    void UpdateWheelVisuals()
    {
        UpdateSingleWheel(frontLeftCollider, frontLeftTransform);
        UpdateSingleWheel(frontRightCollider, frontRightTransform);

        if (currentSpeed >= 1)
        {
            if (velocity.magnitude > 0.1f || !Input.GetKey(KeyCode.Space))
            {
                UpdateSingleWheel(rearLeftCollider, rearLeftTransform);
                UpdateSingleWheel(rearRightCollider, rearRightTransform);
            }
        }
    }

    void Steer()
    {
        if (velocity.magnitude > 0.1f)
        {
            // Determine forward or reverse
            float forwardSpeed = Vector3.Dot(velocity, transform.forward);
            float direction = Mathf.Sign(forwardSpeed); // 1 = forward, -1 = reverse

            float speedFactor = Mathf.Clamp01(velocity.magnitude / 50f);
            float driftBoost = isDrifting ? driftSteeringBoost : 1f;

            // Use direction multiplier to steer properly in reverse
            float steerAmount = steerInput * maxSteerAngle * speedFactor * driftBoost * direction;

            Quaternion targetRotation = Quaternion.Euler(0f, steerAmount, 0f) * transform.rotation;
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, steeringSpeed * Time.deltaTime);
        }
    }
    void UpdateSingleWheel(WheelCollider collider, Transform transform)
    {
        if (transform == null) return;

        collider.GetWorldPose(out Vector3 pos, out Quaternion rot);
        transform.position = pos;
        transform.rotation = rot;
    }

    void UpdateAccelerationMultiplier()
    {
        if (moveInput > 0.1f)
        {
            accelerationTimer += Time.deltaTime;
            float t = Mathf.Clamp01(accelerationTimer / accelerationRampUpTime);
            accelerationMultiplier = Mathf.Lerp(minAccelerationMultiplier, maxAccelerationMultiplier, t);
        }
        else
        {
            accelerationTimer = 0f;
            accelerationMultiplier = minAccelerationMultiplier;
        }
    }

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -transform.up, groundCheckDistance, groundLayer);
    }

    void Jump()
    {
        velocity += Vector3.up * jumpForce;

        StartCoroutine(endJump());
    }

    IEnumerator endJump()
    {
        rb.isKinematic = false;

        yield return new WaitForSeconds(4f);

        rb.isKinematic = true;
    }

    void checkForDamage()
    {
        if (health == 0)
        {
            Debug.Log("Car health has reached 0");

            inCar = false;
        }
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Start"))
        {
            Debug.Log("Timer will start");
            winCon.startTimer();
   
        }

       if (collision.CompareTag("Finish"))
        {
            Debug.Log("Timer will Stop");
            winScreen.gameObject.SetActive(true);
            winCon.stopTimer();
            inCar = false;
            //hasHitFinish = true;
        }  

       if (collision.CompareTag("Obsticle"))
        {
            Debug.Log("Car took damage");
            TakeDamage(1);
        }

        if (collision.CompareTag("deathWall"))
        {
            Debug.Log("Insta Death");
            TakeDamage(health);
            loseScreen.gameObject.SetActive(true);
        }

        if (collision.CompareTag("checkPoint"))
        {
            Debug.Log("Finihs line is now active");
            finishLine.gameObject.SetActive(true);
        }
    }

    public virtual void TakeDamage(int damageValue)
    {
        health -= damageValue;
        Debug.Log("Player takes " + damageValue + " damage. Remaining Health: " + health);

        if (health <= 0)
        {
            Debug.Log("Car Health is zero");

            inCar = false;
        }
    }
}