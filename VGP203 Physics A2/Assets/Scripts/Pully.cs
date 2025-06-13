using UnityEngine;
public class PulleySystem : MonoBehaviour
{
    //[Header("Pulley Settings")]
    //public Rigidbody pulleyRb;
    //public float pulleyRadius = 0.5f; // meters
    //public float pulleyMass = 1f;     // kg

    //[Header("Mass Settings")]
    //public Rigidbody hangingMassRb;
    //public float ropeLength = 5f;

    //private float g = 9.81f;

    //void FixedUpdate()
    //{
    //    float m = hangingMassRb.mass;
    //    float r = pulleyRadius;

    //    // Moment of inertia for a solid disk: I = 0.5 * m * r^2
    //    float I = 0.5f * pulleyMass * r * r;

    //    // Assume mass is hanging and pulling the rope
    //    float a = (m * g) / (m + I / (r * r));      // Linear acceleration
    //    float T = m * (g - a);                      // Tension in rope
    //    float torque = T * r;                       // Torque on pulley

    //    // Apply torque to pulley
    //    pulleyRb.AddTorque(Vector3.back * torque); // Adjust axis as needed

    //    // Apply pulling force to hanging mass (simulate tension)
    //    Vector3 tensionForce = Vector3.up * T;
    //    hangingMassRb.AddForce(tensionForce);
    //}

    public Rigidbody pulleyRb;
    public Transform ropeStartPoint; // Point on the pulley where rope is attached
    public Rigidbody hangingMassRb;

    public float ropeLength = 2f;
    public float pulleyRadius = 0.5f;
    public float pulleyMass = 1f;
    public float speed = 600f;

    [HideInInspector] public bool beingPulled = false;
    private float g = 9.81f;

    void FixedUpdate()
    {
        Vector3 massPos = hangingMassRb.worldCenterOfMass;
        Vector3 ropeVec = massPos - ropeStartPoint.position;
        float currentLength = ropeVec.magnitude;

        if (currentLength > ropeLength)
        {
            // Normalize rope vector
            Vector3 ropeDir = ropeVec.normalized;

            // Enforce rope constraint: project velocity to get radial component
            Vector3 relativeVelocity = hangingMassRb.linearVelocity;
            float radialVel = Vector3.Dot(relativeVelocity, ropeDir);

            // Calculate tension (simplified spring-like)
            float stretch = currentLength - ropeLength;
            float stiffness = 1000f;
            float damping = 20f;

            Vector3 tensionForce = -ropeDir * (stiffness * stretch + damping * radialVel);

            // Apply tension to mass
            hangingMassRb.AddForce(tensionForce);

            // Apply torque to pulley
            float torqueMagnitude = Vector3.Cross(ropeStartPoint.position - pulleyRb.worldCenterOfMass, tensionForce).magnitude;
            Vector3 torqueDirection = Vector3.Cross(ropeStartPoint.position - pulleyRb.worldCenterOfMass, tensionForce).normalized;
            pulleyRb.AddTorque(torqueDirection * torqueMagnitude);
        }
    }

    public void playerPull()
    {
        if (beingPulled)
        {
            ropeStartPoint.Translate(Vector3.up * speed * Time.deltaTime);
        }

        else return;
    }
}

