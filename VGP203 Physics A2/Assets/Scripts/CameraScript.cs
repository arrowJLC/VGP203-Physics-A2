using System.Collections;
using UnityEngine;
using Unity.Cinemachine;

public class CinemachineCameraSwitcher : MonoBehaviour
{
    [Header("Assign your Cinemachine Cameras")]
    public CinemachineVirtualCameraBase[] cameras;
    CarControllers carControl;
    PlayerControllers playerControl;

    //[Header("Key to switch cameras")]
    //public KeyCode switchKey = KeyCode.C;

    private int currentCameraIndex = 0;
    public bool isInsideCar = true;

    void Start()
    {
        SetActiveCamera(currentCameraIndex);
        carControl = FindAnyObjectByType<CarControllers>();
        playerControl = FindAnyObjectByType<PlayerControllers>();
    }

    private bool wasInCarLastFrame = false;

    //void FixedUpdate()
    //{
    //    //bool isInCarNow = carControl.inCar && playerControl.isInside;
    //    bool isInCarNow = carControl.inCar && playerControl.isInside && !playerControl.leftCar;

    //    if (isInCarNow && !wasInCarLastFrame)
    //    {
    //        // Switch to the next camera only once when entering the car
    //        currentCameraIndex = (currentCameraIndex + 1) % cameras.Length;
    //        SetActiveCamera(currentCameraIndex);
    //    }

    //    // Update the state for the next frame
    //    wasInCarLastFrame = isInCarNow;
    //}

    void FixedUpdate()
    {
        bool isInCarNow = carControl.inCar && playerControl.isInside && !playerControl.leftCar;

        if (isInCarNow && !wasInCarLastFrame)
        {
            currentCameraIndex = (currentCameraIndex + 1) % cameras.Length;
            SetActiveCamera(currentCameraIndex);
        }
        else if (!isInCarNow && wasInCarLastFrame)
        {
            currentCameraIndex = 0; // or any default exit camera
            SetActiveCamera(currentCameraIndex);
        }

        wasInCarLastFrame = isInCarNow;
    }


    private void SetActiveCamera(int index)
    {
        for (int i = 0; i < cameras.Length; i++)
        {
            cameras[i].Priority = (i == index) ? 10 : 0;
        }

        Debug.Log("Switched to camera: " + cameras[index].name);
    }
}
  

