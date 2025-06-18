using UnityEngine;

public class RopeVisual : MonoBehaviour
{
    public Transform pulleyTopPoint;     
    public Transform hangingMass; 

    private LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
    }

    void Update()
    {
        if (pulleyTopPoint != null && hangingMass != null)
        {
            lineRenderer.SetPosition(0, pulleyTopPoint.position);
            lineRenderer.SetPosition(1, hangingMass.position);
        }
    }
}
