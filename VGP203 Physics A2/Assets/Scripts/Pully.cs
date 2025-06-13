using UnityEngine;
public class PulleySystem : MonoBehaviour
{
    public Rigidbody pulleyRb;
    public Transform ropeStartPoint; // Point on the pulley where rope is attached
    public Rigidbody hangingMassRb;

    public float ropeLength = 2f;
    public float pulleyRadius = 0.5f;
    public float pulleyMass = 1f;
    public float speed = 20f;

    [HideInInspector] public bool beingPulled = false;
    private float g = 9.81f;

    //void FixedUpdate()
    //{
    //    float m = hangingMassRb.mass;
    //    float r = pulleyRadius;

    //    // Moment of inertia of a solid disk
    //    float I = 0.5f * pulleyMass * r * r;

    //    // Linear acceleration of the hanging mass
    //    float a = (m * g) / (m + I / (r * r));

    //    // Tension in the rope
    //    float T = m * (g - a);

    //    // Torque applied to pulley
    //    float torque = T * r;

    //    // Apply torque to the pulley
    //    pulleyRb.AddTorque(Vector3.back * torque);  // Make sure this axis matches your setup

    //    // Apply upward tension force to the hanging mass
    //    Vector3 tensionForce = Vector3.up * T;
    //    hangingMassRb.AddForce(tensionForce);
    //}

    void FixedUpdate()
    {
        float m = hangingMassRb.mass;
        float r = pulleyRadius;
        float I = 0.5f * pulleyMass * r * r;
        Vector3 ropeTop = ropeStartPoint.position;
        Vector3 ropeVec = hangingMassRb.position - ropeTop;
        float currentLength = ropeVec.magnitude;
        Vector3 direction = ropeVec.normalized;

        // Ideal linear acceleration
        float a = (m * g) / (m + I / (r * r));
        float T = m * (g - a);
        float torque = T * r;

        // Apply torque to the pulley
        pulleyRb.AddTorque(Vector3.back * torque); // Adjust axis if needed

        // Apply tension force upward along rope direction
        Vector3 tensionForce = -direction * T;
        hangingMassRb.AddForce(tensionForce);

        // Apply gravity
        hangingMassRb.AddForce(Vector3.down * m * g);

        // ?? Enforce fixed rope length
        float stretch = currentLength - ropeLength;
        if (Mathf.Abs(stretch) > 0.001f)
        {
            // Move mass back to the correct position (non-physical correction)
            hangingMassRb.position = ropeTop + direction * ropeLength;

            // Remove velocity along rope direction (zero out radial velocity)
            Vector3 velocity = hangingMassRb.linearVelocity;
            float radialSpeed = Vector3.Dot(velocity, direction);
            hangingMassRb.linearVelocity = velocity - direction * radialSpeed;
        }
    }


    void LateUpdate()
    {
        float ropeLength = 2.0f; // Example value
        Vector3 dir = (hangingMassRb.position - pulleyRb.position).normalized;
        //hangingMassRb.position = pulleyRb.position + dir * ropeLength;
    }

    public void playerPull()
    {
        if (beingPulled)
        {
            ropeStartPoint.Translate(Vector3.back * speed * Time.deltaTime);
        }
    }
}

















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


//void FixedUpdate()
//{
//    //Vector3 massPos = hangingMassRb.worldCenterOfMass;
//    //Vector3 ropeVec = massPos - ropeStartPoint.position;
//    //float currentLength = ropeVec.magnitude;

//    ////if (currentLength > ropeLength)
//    ////{
//    ////    // Normalize rope vector
//    ////    Vector3 ropeDir = ropeVec.normalized;

//    ////    // Enforce rope constraint: project velocity to get radial component
//    ////    Vector3 relativeVelocity = hangingMassRb.linearVelocity;
//    ////    float radialVel = Vector3.Dot(relativeVelocity, ropeDir);

//    ////    // Calculate tension (simplified spring-like)
//    ////    float stretch = currentLength - ropeLength;
//    ////    float stiffness = 1000f;
//    ////    float damping = 20f;

//    ////    Vector3 tensionForce = -ropeDir * (stiffness * stretch + damping * radialVel);

//    ////    // Apply tension to mass
//    ////    hangingMassRb.AddForce(tensionForce);

//    ////    // Apply torque to pulley
//    ////    float torqueMagnitude = Vector3.Cross(ropeStartPoint.position - pulleyRb.worldCenterOfMass, tensionForce).magnitude;
//    ////    Vector3 torqueDirection = Vector3.Cross(ropeStartPoint.position - pulleyRb.worldCenterOfMass, tensionForce).normalized;
//    ////    pulleyRb.AddTorque(torqueDirection * torqueMagnitude);
//    ////}
//    ///
//    //float m = hangingMassRb.mass;
//    //float r = pulleyRadius;

//    //// Moment of inertia for a solid disk: I = 0.5 * m * r^2
//    //float I = 0.5f * pulleyMass * r * r;

//    //// Assume mass is hanging and pulling the rope
//    //float a = (m * g) / (m + I / (r * r));      // Linear acceleration
//    //float T = m * (g - a);                      // Tension in rope
//    //float torque = T * r;                       // Torque on pulley

//    //// Apply torque to pulley
//    //pulleyRb.AddTorque(Vector3.back * torque); // Adjust axis as needed

//    //// Apply pulling force to hanging mass (simulate tension)
//    //Vector3 tensionForce = Vector3.up * T;
//    //hangingMassRb.AddForce(tensionForce);


//}
