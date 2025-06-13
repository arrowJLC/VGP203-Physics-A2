using UnityEngine;
using System;
using System.Collections.Generic;

public class fineIlldoItmySelf : MonoBehaviour
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

    public float carMass = 1200f; // in kg
    public float engineForce = 8000f; // in Newtons
    public float maxReverse = 4000f;
    public float brakeForce = 10000f;
    public float drag = 0.1f;
    public float accelerationMultiplier = 1f;
    public float accelerationSmoothness = 5f;
    public float steeringSpeed = 30f;
    public float maxSteerAngle = 30f;

    private Rigidbody rb;
    private float moveInput;
    private float steerInput;
    private float steeringInput;
    private Vector3 velocity = Vector3.zero;
    public float currentSpeed;
    public bool inCar = false;

    public float speed;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        //rb.isKinematic = true; // Required for kinematic control
    }
    void Update()
    {
        //if (Input.GetKey(KeyCode.J))
        //{
        //    ApplyBrakes(brakeForce);
        //}
        //else
        //{
        //    ApplyBrakes(0f);
        //}
        moveInput = Input.GetAxis("Vertical");
        steerInput = Input.GetAxis("Horizontal");

        if (inCar)
        {
            Steer();
            Brake();
           //ApplySteering();
            ApplyKinematicMovement();
            UpdateWheelVisuals();
        }
    }


    public void MoveInput(float input)
    {
        moveInput = input;
    }

    public void SteerInput(float input)
    {
        steerInput = input;
    }

    void ApplyKinematicMovement()
    {

        //  //old movmnt
        float appliedForce = moveInput * engineForce;

        // Calculate acceleration using F = ma
        Vector3 acceleration = transform.forward * (appliedForce / carMass) * accelerationMultiplier;

        Vector3 targetAcceleration = transform.forward * (appliedForce / carMass) * accelerationMultiplier;
        velocity = Vector3.Lerp(velocity, velocity + targetAcceleration * Time.deltaTime, Time.deltaTime * accelerationSmoothness);


        // Apply drag
        velocity *= (1 - drag * Time.deltaTime);

        // Apply acceleration
        velocity += acceleration * Time.deltaTime;

        // Clamp forward and reverse speed
        float forwardSpeed = Vector3.Dot(velocity, transform.forward);

        float maxSpeed = 50f; // in m/s (180 km/h)
        if (forwardSpeed > maxSpeed)
            velocity = transform.forward * maxSpeed;
        //    if (forwardSpeed > engineForce /*maxSpeed*/)
        //{
        //    velocity = transform.forward * engineForce;
        //}
        else if (forwardSpeed < -maxReverse)
        {
            velocity = transform.forward * -maxReverse;
        }

        // Move the car
        transform.position += velocity * Time.deltaTime;

        currentSpeed = velocity.magnitude * 3.6f;
    }




    //void ApplyKinematicMovement()
    //{
    //    float appliedForce = moveInput * engineForce;

    //    // Calculate acceleration
    //    Vector3 acceleration = transform.forward * (appliedForce / carMass) * accelerationMultiplier;

    //    // Apply drag
    //    velocity *= (1f - drag * Time.deltaTime);

    //    // Apply acceleration
    //    velocity += acceleration * Time.deltaTime;

    //    // Clamp forward and reverse speed
    //    float forwardSpeed = Vector3.Dot(velocity, transform.forward);
    //    if (forwardSpeed > engineForce)
    //        velocity = transform.forward * engineForce;
    //    else if (forwardSpeed < -maxReverse)
    //        velocity = transform.forward * -maxReverse;

    //    // Smooth rotation alignment with movement direction
    //    //if (velocity.magnitude > 1f)
    //    //{
    //    //    Quaternion targetRot = Quaternion.LookRotation(velocity.normalized, Vector3.up);
    //    //    transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, steeringSpeed * 0.5f * Time.deltaTime);
    //    //}

    //    // Move the car
    //    transform.position += velocity * Time.deltaTime;

    //    currentSpeed = velocity.magnitude * 3.6f;

    //    // Optional: apply wheel collider torque/steering just for visuals
    //    float motorTorque = moveInput * engineForce;
    //    float steerAngle = steerInput * maxSteerAngle;

    //    rearLeftCollider.motorTorque = motorTorque;
    //    rearRightCollider.motorTorque = motorTorque;

    //    frontLeftCollider.steerAngle = steerAngle;
    //    frontRightCollider.steerAngle = steerAngle;
    //}

    void Brake()
    {
        if (Input.GetKey(KeyCode.Space) || Mathf.Approximately(moveInput, 0))
        {
            float brakingDecel = (brakeForce / carMass) * Time.deltaTime;
            velocity = Vector3.MoveTowards(velocity, Vector3.zero, brakingDecel);
        }
    }
    void ApplyBrakes(float force)
    {
        frontLeftCollider.brakeTorque = force;
        frontRightCollider.brakeTorque = force;
        rearLeftCollider.brakeTorque = force;
        rearRightCollider.brakeTorque = force;
    }

    //void ApplySteering()
    //{

    //    float steeringAngle = steeringInput * steeringCurve.Evaluate(speed);
    //    if (slipAngle < 120f)
    //    {
    //        steeringAngle += Vector3.SignedAngle(transform.forward, rb.linearVelocity + transform.forward, Vector3.up);
    //    }
    //    steeringAngle = Mathf.Clamp(steeringAngle, -90f, 90f);
    //    frontLeftCollider.steerAngle = steerAngle;
    //    frontRightCollider.steerAngle = steerAngle;
    //}

    //void ApplySteering()
    //{
    //    float steeringAngle = steerInput * maxSteerAngle; // <-- Use steerInput directly
    //    frontLeftCollider.steerAngle = steeringAngle;
    //    frontRightCollider.steerAngle = steeringAngle;
    //}

    void Steer()
    {
        if (velocity.magnitude > 0.1f)
        {
            float steerAmount = steerInput * steeringSpeed * Time.deltaTime;
            Quaternion turnRotation = Quaternion.Euler(0f, steerAmount, 0f);

            // Rotate the car
            transform.rotation *= turnRotation;

            // Rotate the velocity vector to match car heading
            velocity = turnRotation * velocity;
        }

        float steeringAngle = steerInput * maxSteerAngle;
        frontLeftCollider.steerAngle = steeringAngle;
        frontRightCollider.steerAngle = steeringAngle;
    }

        void UpdateWheelVisuals()
    {
        UpdateWheelPose(frontLeftCollider, frontLeftTransform);
        UpdateWheelPose(frontRightCollider, frontRightTransform);
        UpdateWheelPose(rearLeftCollider, rearLeftTransform);
        UpdateWheelPose(rearRightCollider, rearRightTransform);
    }

    void UpdateWheelPose(WheelCollider collider, Transform transform)
    {
        if (collider != null && transform != null)
        {
            Vector3 pos;
            Quaternion rot;
            collider.GetWorldPose(out pos, out rot);
            transform.position = pos;
            transform.rotation = rot;
        }
    }
}













//// New smoother acceleration section
//float appliedForce = moveInput * engineForce;
//Vector3 targetAccel = transform.forward * (appliedForce / carMass) * accelerationMultiplier;
//velocity = Vector3.Lerp(velocity, velocity + targetAccel * Time.deltaTime, Time.deltaTime * accelerationSmoothness);

//// Apply drag
//velocity *= (1 - drag * Time.deltaTime);

//// Clamp speed
//float forwardSpeed = Vector3.Dot(velocity, transform.forward);
//float maxForwardSpeed = 50f;
//if (forwardSpeed > maxForwardSpeed)
//    velocity = transform.forward * maxForwardSpeed;
//else if (forwardSpeed < -maxReverse)
//    velocity = transform.forward * -maxReverse;

//// Move
//transform.position += velocity * Time.deltaTime;
//currentSpeed = velocity.magnitude * 3.6f; // km/h


//float motorTorque = moveInput * engineForce;
//float steerAngle = steerInput * steeringSpeed;

//// Apply rear-wheel torque
//rearLeftCollider.motorTorque = motorTorque;
//rearRightCollider.motorTorque = motorTorque;

//// Steering to front wheels
////frontLeftCollider.steerAngle = steerAngle;
////frontRightCollider.steerAngle = steerAngle;


////steeringAngle = Mathf.Clamp(steeringAngle, -90f, 90f);
//frontLeftCollider.steerAngle = steerAngle;
//frontRightCollider.steerAngle = steerAngle;


//// Braking
//if (Input.GetKey(KeyCode.Space))
//{
//    ApplyBrakes(brakeForce);
//}
//else
//{
//    ApplyBrakes(0f);
//}


















//    /*
//    if (velocity.magnitude > 0.1f)
//    {
//        float speedFactor = Mathf.Clamp01(velocity.magnitude / 50f); // reduce at high speeds
//        float steerAmount = steerInput * maxSteerAngle * speedFactor;

//        // Smooth turning using Quaternion.Lerp
//        Quaternion targetRotation = Quaternion.Euler(0f, steerAmount, 0f) * transform.rotation;
//        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, steeringSpeed * Time.deltaTime);
//    }

//    // Lean the car slightly while turning
//    float leanAmount = -steerInput * Mathf.Clamp(velocity.magnitude / 20f, 0f, 1f);
//    Quaternion leanRotation = Quaternion.Euler(0f, 0f, leanAmount * 5f); // 5 degrees max lean
//    transform.rotation *= leanRotation;
//    */

//    //if (velocity.magnitude > 0.1f)
//    //{
//    //    // Reduce steering at high speeds
//    //    float speedFactor = Mathf.Clamp01(velocity.magnitude / 50f);
//    //    float steerAngle = steerInput * maxSteerAngle * speedFactor;

//    //    // Rotate car left/right (Y-axis) based on input
//    //    transform.Rotate(0f, steerAngle * Time.deltaTime, 0f);
//    //}
//    //else
//    //{
//    //    // Low speed: allow turning based on input only
//    //    float steerAngle = steerInput * maxSteerAngle;
//    //    transform.Rotate(0f, steerAngle * Time.deltaTime, 0f);
//    //}

//    if (velocity.magnitude > 0.1f)
//    {
//        velocity = Quaternion.Euler(0f, steerInput * steeringSpeed * Time.deltaTime, 0f) * velocity;
//    }
//}











//using UnityEngine;
//using System;
//using System.Collections.Generic;

//public class fineIlldoItmySelf : MonoBehaviour
//{
//    [Header("Wheel Colliders")]
//    public WheelCollider frontLeftCollider;
//    public WheelCollider frontRightCollider;
//    public WheelCollider rearLeftCollider;
//    public WheelCollider rearRightCollider;

//    [Header("Wheel Transforms (Optional Visuals)")]
//    public Transform frontLeftTransform;
//    public Transform frontRightTransform;
//    public Transform rearLeftTransform;
//    public Transform rearRightTransform;

//    public float carMass = 1200f; // in kg
//    public float engineForce = 8000f; // in Newtons
//    public float maxReverse = 4000f;
//    public float brakeForce = 10000f;
//    public float drag = 0.1f;
//    public float accelerationMultiplier = 1f;
//    public float accelerationSmoothness = 5f;
//    public float steeringSpeed = 30f;
//    public float maxSteerAngle = 30f;

//    private Rigidbody rb;
//    private float moveInput;
//    private float steerInput;
//    private float steeringInput;
//    private Vector3 velocity = Vector3.zero;
//    public float currentSpeed;
//    public bool inCar = false;

//    public float speed;
//    private void Start()
//    {
//        rb = GetComponent<Rigidbody>();
//        //rb.isKinematic = true; // Required for kinematic control
//    }
//    void Update()
//    {
//        //if (Input.GetKey(KeyCode.J))
//        //{
//        //    ApplyBrakes(brakeForce);
//        //}
//        //else
//        //{
//        //    ApplyBrakes(0f);
//        //}
//        moveInput = Input.GetAxis("Vertical");
//        steerInput = Input.GetAxis("Horizontal");

//        if (inCar)
//        {
//            //GetInputs();
//            Steer();
//            Brake();
//            //ApplySteering();
//            ApplyKinematicMovement();
//            UpdateWheelVisuals();
//        }
//    }

//    //void GetInputs()
//    //{
//    //    moveInput = Input.GetAxis("Vertical");
//    //    steerInput = Input.GetAxis("Horizontal");
//    //}

//    public void MoveInput(float input)
//    {
//        moveInput = input;
//    }

//    public void SteerInput(float input)
//    {
//        steerInput = input;
//    }

//    void ApplyKinematicMovement()
//    {

//        //  //old movmnt
//        float appliedForce = moveInput * engineForce;

//        // Calculate acceleration using F = ma
//        Vector3 acceleration = transform.forward * (appliedForce / carMass) * accelerationMultiplier;

//        Vector3 targetAcceleration = transform.forward * (appliedForce / carMass) * accelerationMultiplier;
//        velocity = Vector3.Lerp(velocity, velocity + targetAcceleration * Time.deltaTime, Time.deltaTime * accelerationSmoothness);


//        // Apply drag
//        velocity *= (1 - drag * Time.deltaTime);

//        // Apply acceleration
//        velocity += acceleration * Time.deltaTime;

//        // Clamp forward and reverse speed
//        float forwardSpeed = Vector3.Dot(velocity, transform.forward);

//        float maxSpeed = 50f; // in m/s (180 km/h)
//        if (forwardSpeed > maxSpeed)
//            velocity = transform.forward * maxSpeed;
//        //    if (forwardSpeed > engineForce /*maxSpeed*/)
//        //{
//        //    velocity = transform.forward * engineForce;
//        //}
//        else if (forwardSpeed < -maxReverse)
//        {
//            velocity = transform.forward * -maxReverse;
//        }

//        // Move the car
//        transform.position += velocity * Time.deltaTime;

//        currentSpeed = velocity.magnitude * 3.6f;
//    }




//    //void ApplyKinematicMovement()
//    //{
//    //    float appliedForce = moveInput * engineForce;

//    //    // Calculate acceleration
//    //    Vector3 acceleration = transform.forward * (appliedForce / carMass) * accelerationMultiplier;

//    //    // Apply drag
//    //    velocity *= (1f - drag * Time.deltaTime);

//    //    // Apply acceleration
//    //    velocity += acceleration * Time.deltaTime;

//    //    // Clamp forward and reverse speed
//    //    float forwardSpeed = Vector3.Dot(velocity, transform.forward);
//    //    if (forwardSpeed > engineForce)
//    //        velocity = transform.forward * engineForce;
//    //    else if (forwardSpeed < -maxReverse)
//    //        velocity = transform.forward * -maxReverse;

//    //    // Smooth rotation alignment with movement direction
//    //    //if (velocity.magnitude > 1f)
//    //    //{
//    //    //    Quaternion targetRot = Quaternion.LookRotation(velocity.normalized, Vector3.up);
//    //    //    transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, steeringSpeed * 0.5f * Time.deltaTime);
//    //    //}

//    //    // Move the car
//    //    transform.position += velocity * Time.deltaTime;

//    //    currentSpeed = velocity.magnitude * 3.6f;

//    //    // Optional: apply wheel collider torque/steering just for visuals
//    //    float motorTorque = moveInput * engineForce;
//    //    float steerAngle = steerInput * maxSteerAngle;

//    //    rearLeftCollider.motorTorque = motorTorque;
//    //    rearRightCollider.motorTorque = motorTorque;

//    //    frontLeftCollider.steerAngle = steerAngle;
//    //    frontRightCollider.steerAngle = steerAngle;
//    //}

//    void Brake()
//    {
//        if (Input.GetKey(KeyCode.Space) || Mathf.Approximately(moveInput, 0))
//        {
//            float brakingDecel = (brakeForce / carMass) * Time.deltaTime;
//            velocity = Vector3.MoveTowards(velocity, Vector3.zero, brakingDecel);
//        }
//    }
//    void ApplyBrakes(float force)
//    {
//        frontLeftCollider.brakeTorque = force;
//        frontRightCollider.brakeTorque = force;
//        rearLeftCollider.brakeTorque = force;
//        rearRightCollider.brakeTorque = force;
//    }

//    //void ApplySteering()
//    //{

//    //    float steeringAngle = steeringInput * steeringCurve.Evaluate(speed);
//    //    if (slipAngle < 120f)
//    //    {
//    //        steeringAngle += Vector3.SignedAngle(transform.forward, rb.linearVelocity + transform.forward, Vector3.up);
//    //    }
//    //    steeringAngle = Mathf.Clamp(steeringAngle, -90f, 90f);
//    //    frontLeftCollider.steerAngle = steerAngle;
//    //    frontRightCollider.steerAngle = steerAngle;
//    //}

//    //void ApplySteering()
//    //{
//    //    float steeringAngle = steerInput * maxSteerAngle; // <-- Use steerInput directly
//    //    frontLeftCollider.steerAngle = steeringAngle;
//    //    frontRightCollider.steerAngle = steeringAngle;
//    //}

//    void Steer()
//    {
//        if (velocity.magnitude > 0.1f)
//        {
//            float steerAmount = steerInput * steeringSpeed * Time.deltaTime;
//            Quaternion turnRotation = Quaternion.Euler(0f, steerAmount, 0f);

//            // Rotate the car
//            transform.rotation *= turnRotation;

//            // Rotate the velocity vector to match car heading
//            velocity = turnRotation * velocity;
//        }

//        float steeringAngle = steerInput * maxSteerAngle;
//        frontLeftCollider.steerAngle = steeringAngle;
//        frontRightCollider.steerAngle = steeringAngle;
//    }

//    //void Steer()
//    //{
//    //    //    //old steer
//    //    if (velocity.magnitude > 0.1f)
//    //    {
//    //        float steerAmount = steerInput * steeringSpeed * maxSteerAngle;
//    //        transform.Rotate(Vector3.up, steerAmount * Time.deltaTime);
//    //        //UpdateWheelVisuals();
//    //    }
//    //}




//    void UpdateWheelVisuals()
//    {
//        UpdateWheelPose(frontLeftCollider, frontLeftTransform);
//        UpdateWheelPose(frontRightCollider, frontRightTransform);
//        UpdateWheelPose(rearLeftCollider, rearLeftTransform);
//        UpdateWheelPose(rearRightCollider, rearRightTransform);
//    }

//    void UpdateWheelPose(WheelCollider collider, Transform transform)
//    {
//        if (collider != null && transform != null)
//        {
//            Vector3 pos;
//            Quaternion rot;
//            collider.GetWorldPose(out pos, out rot);
//            transform.position = pos;
//            transform.rotation = rot;
//        }
//    }
//}













////// New smoother acceleration section
////float appliedForce = moveInput * engineForce;
////Vector3 targetAccel = transform.forward * (appliedForce / carMass) * accelerationMultiplier;
////velocity = Vector3.Lerp(velocity, velocity + targetAccel * Time.deltaTime, Time.deltaTime * accelerationSmoothness);

////// Apply drag
////velocity *= (1 - drag * Time.deltaTime);

////// Clamp speed
////float forwardSpeed = Vector3.Dot(velocity, transform.forward);
////float maxForwardSpeed = 50f;
////if (forwardSpeed > maxForwardSpeed)
////    velocity = transform.forward * maxForwardSpeed;
////else if (forwardSpeed < -maxReverse)
////    velocity = transform.forward * -maxReverse;

////// Move
////transform.position += velocity * Time.deltaTime;
////currentSpeed = velocity.magnitude * 3.6f; // km/h


////float motorTorque = moveInput * engineForce;
////float steerAngle = steerInput * steeringSpeed;

////// Apply rear-wheel torque
////rearLeftCollider.motorTorque = motorTorque;
////rearRightCollider.motorTorque = motorTorque;

////// Steering to front wheels
//////frontLeftCollider.steerAngle = steerAngle;
//////frontRightCollider.steerAngle = steerAngle;


//////steeringAngle = Mathf.Clamp(steeringAngle, -90f, 90f);
////frontLeftCollider.steerAngle = steerAngle;
////frontRightCollider.steerAngle = steerAngle;


////// Braking
////if (Input.GetKey(KeyCode.Space))
////{
////    ApplyBrakes(brakeForce);
////}
////else
////{
////    ApplyBrakes(0f);
////}


















////    /*
////    if (velocity.magnitude > 0.1f)
////    {
////        float speedFactor = Mathf.Clamp01(velocity.magnitude / 50f); // reduce at high speeds
////        float steerAmount = steerInput * maxSteerAngle * speedFactor;

////        // Smooth turning using Quaternion.Lerp
////        Quaternion targetRotation = Quaternion.Euler(0f, steerAmount, 0f) * transform.rotation;
////        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, steeringSpeed * Time.deltaTime);
////    }

////    // Lean the car slightly while turning
////    float leanAmount = -steerInput * Mathf.Clamp(velocity.magnitude / 20f, 0f, 1f);
////    Quaternion leanRotation = Quaternion.Euler(0f, 0f, leanAmount * 5f); // 5 degrees max lean
////    transform.rotation *= leanRotation;
////    */

////    //if (velocity.magnitude > 0.1f)
////    //{
////    //    // Reduce steering at high speeds
////    //    float speedFactor = Mathf.Clamp01(velocity.magnitude / 50f);
////    //    float steerAngle = steerInput * maxSteerAngle * speedFactor;

////    //    // Rotate car left/right (Y-axis) based on input
////    //    transform.Rotate(0f, steerAngle * Time.deltaTime, 0f);
////    //}
////    //else
////    //{
////    //    // Low speed: allow turning based on input only
////    //    float steerAngle = steerInput * maxSteerAngle;
////    //    transform.Rotate(0f, steerAngle * Time.deltaTime, 0f);
////    //}

////    if (velocity.magnitude > 0.1f)
////    {
////        velocity = Quaternion.Euler(0f, steerInput * steeringSpeed * Time.deltaTime, 0f) * velocity;
////    }
////}