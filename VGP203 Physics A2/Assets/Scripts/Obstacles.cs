using UnityEngine;

public class Obstacles : MonoBehaviour
{
    [SerializeField] private float forceMag = 1F;
    //[SerializeField] private float desiredDeltaV = 5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody rigidbody = hit.collider.attachedRigidbody;

        if (rigidbody != null)
        {
            Vector3 forceDirection = hit.gameObject.transform.position - transform.position;
            forceDirection.y = 0f;
            forceDirection.Normalize();

            //Vector3 impulse = forceDirection * rigidbody.mass * desiredDeltaV;

            //rigidbody.AddForceAtPosition(impulse, transform.position, ForceMode.Impulse);
            rigidbody.AddForceAtPosition(forceDirection * forceMag, transform.position, ForceMode.Impulse); ;
        }
    }
}
