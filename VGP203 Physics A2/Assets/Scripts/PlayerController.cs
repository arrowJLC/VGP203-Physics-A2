using UnityEngine;
using UnityEngine.InputSystem.XR;

[RequireComponent(typeof(CharacterController))]
public class PlayerControllers : MonoBehaviour
{
    CharacterController cc;
    public Animator anim;

    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float gravity = -9.81f;
    public Transform cameraTransform;
    private float rotationSpeed = 10f;

    [Header("Jump Settings")]
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float jumpTime = 0.7f;
    private float timeToApex;
    private float initialJumpVelocity;

    private Vector2 direction;
    private Vector3 velocity;
    private float curSpeed;

    private bool isJumpPressed;
    public bool isInside = false;
    public bool leftCar = true;

    public LayerMask raycastCollisionLayer;


    void Start()
    {
        cc = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;

        InitJump();
    }

    void InitJump()
    {
        timeToApex = jumpTime / 2f;
        gravity = (-2 * jumpHeight) / Mathf.Pow(timeToApex, 2);
        initialJumpVelocity = Mathf.Abs(gravity * timeToApex);
    }

    void Update()
    {
        if (isInside) return;

        float x = 0f, y = 0f;
        if (Input.GetKey(KeyCode.W)) y = 1f;
        if (Input.GetKey(KeyCode.S)) y = -1f;
        if (Input.GetKey(KeyCode.A)) x = -1f;
        if (Input.GetKey(KeyCode.D)) x = 1f;

        direction = new Vector2(x, y).normalized;

        Vector2 moveVel = new Vector2(velocity.x, velocity.z);
        anim.SetFloat("Blend", moveVel.magnitude);

        isJumpPressed = Input.GetKeyDown(KeyCode.Space);

        Vector3 move = ProjectedMoveDirection();
        UpdateCharacterVelocity(move);

        if (cc.isGrounded && velocity.y < 0)
            velocity.y = -2f;

        if (!cc.isGrounded)
            velocity.y += gravity * Time.deltaTime;
        else if (isJumpPressed)
            velocity.y = initialJumpVelocity;

        cc.Move(velocity * Time.deltaTime);

        if (move.magnitude > 0.1f)
        {
            Quaternion targetRot = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
        }

        Debug.DrawRay(transform.position, transform.forward * 10f, Color.red);
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 10f, raycastCollisionLayer))
        {
            Debug.Log("Raycast Hit: " + hit.collider.name);
        }
    }

    private Vector3 ProjectedMoveDirection()
    {
        Vector3 cameraFwd = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;
        cameraFwd.y = 0;
        cameraRight.y = 0;
        cameraFwd.Normalize();
        cameraRight.Normalize();
        return (cameraRight * direction.x + cameraFwd * direction.y).normalized;
    }

    private void UpdateCharacterVelocity(Vector3 dir)
    {
        curSpeed = moveSpeed;
        velocity.x = dir.x * curSpeed;
        velocity.z = dir.z * curSpeed;
    }


    public void LeaveCar(CarControllers car)
    {
        transform.parent = null;
        transform.rotation = Quaternion.Euler(3, 0, 0);
        transform.position += Vector3.right * 3;
        gameObject.SetActive(true);

        leftCar = true;
        isInside = false;

        if (car != null)
        {
            car.inCar = false;
        }

        Debug.Log("Player left the car.");
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Player hit car");

        CarControllers car = other.GetComponentInParent<CarControllers>();
        if (car == null) return;

        if (!isInside)
        {
            transform.SetParent(other.transform);
            isInside = true;
            car.inCar = true;
            leftCar = false;
            Debug.Log("Entered Car!");
            gameObject.SetActive(false);
        }
        else
        {
            LeaveCar(car);
        }
    }
}