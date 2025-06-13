using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    PulleySystem pulleySystem;

    private void Start()
    {
        pulleySystem = FindFirstObjectByType<PulleySystem>();
    }
    private void OnTriggerStay(Collider other)
    {
        Debug.Log("Pressure plate stepped on");
        PulleySystem pull = other.gameObject.GetComponent<PulleySystem>();

        //pull.beingPulled = true;
        pulleySystem.playerPull();

    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Pressure plate stepped off");
        PulleySystem pull = other.gameObject.GetComponent<PulleySystem>();

        //pull.beingPulled = false;
    }
}
