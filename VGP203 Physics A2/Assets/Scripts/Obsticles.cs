using UnityEngine;

public class Obsticles : MonoBehaviour
{
    [SerializeField] private float forceMag;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody rigidbody = hit.collider.attachedRigidbody;

        if (rigidbody != null)
        {
            Vector3 forceDirection = hit.gameObject.transform.position - transform.position;
            forceDirection.y = 0f;
            forceDirection.Normalize();

            rigidbody.AddForceAtPosition(forceDirection * forceMag, transform.position, ForceMode.Impulse); ;
        }
    }
}
