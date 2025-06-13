using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [SerializeField] private PulleySystem targetPulley;

    private void OnTriggerStay(Collider other)
    {
        if (targetPulley != null)
        {
            targetPulley.beingPulled = true;
            targetPulley.playerPull();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Pressure plate stepped off");

        if (targetPulley != null)
        {
            targetPulley.beingPulled = false;
        }
    }

}
