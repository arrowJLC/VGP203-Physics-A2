using UnityEngine;
public class PulleySystem : MonoBehaviour
{
    public Rigidbody pulleyRb;
    public Transform ropeStartPoint;
    public Rigidbody hangingMassRb;

    public float ropeLength = 2f;
    public float pulleyRadius = 0.5f;
    public float pulleyMass = 1f;
    public float speed = 20f;

    [HideInInspector] public bool beingPulled = false;
    private float g = 9.81f;

    void FixedUpdate()
    {
        float mass = hangingMassRb.mass;
        float rad = pulleyRadius;
        float Inertia = 0.5f * pulleyMass * rad * rad;

        Vector3 ropeTop = ropeStartPoint.position;
        Vector3 ropeVec = hangingMassRb.position - ropeTop;
        float currentLength = ropeVec.magnitude;
        Vector3 direction = ropeVec.normalized;

        float a = (mass * g) / (mass + Inertia / (rad * rad));
        float Ten = mass * (g - a);
        float torque = Ten * rad;

        pulleyRb.AddTorque(Vector3.back * torque);

        Vector3 tensionForce = -direction * Ten;
        hangingMassRb.AddForce(tensionForce);

        hangingMassRb.AddForce(Vector3.down * mass * g);

        float stretch = currentLength - ropeLength;
        if (Mathf.Abs(stretch) > 0.001f)
        {
            hangingMassRb.position = ropeTop + direction * ropeLength;
            Vector3 velocity = hangingMassRb.linearVelocity;
            float radialSpeed = Vector3.Dot(velocity, direction);
            hangingMassRb.linearVelocity = velocity - direction * radialSpeed;
        }
    }

    //void LateUpdate()
    //{
    //    float ropeLength = 2.0f;
    //    Vector3 dir = (hangingMassRb.position - pulleyRb.position).normalized;
    //    //hangingMassRb.position = pulleyRb.position + dir * ropeLength;
    //}

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
