using Unity.Cinemachine;
using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public CinemachineVirtualCameraBase carCamera;
    public CinemachineVirtualCameraBase playerCamera;

    CarControllers carControl;
    PlayerControllers playerControl;


    void Start()
    {
        carControl = FindAnyObjectByType<CarControllers>();
        playerControl = FindAnyObjectByType<PlayerControllers>();
    }

    private bool wasInCarLastFrame = false;

    private void Update()
    {

        if (carControl.inCar)
        {
            carCamera.Priority = 10;
            playerCamera.Priority = 0;
        }
        if (!carControl.inCar)
        {
            carCamera.Priority = 0;
            playerCamera.Priority = 10;
        }
    }
}

































//using System.Collections;
//using UnityEngine;
//using Unity.Cinemachine;

//public class CinemachineCameraSwitcher : MonoBehaviour
//{
//    [Header("Assign your Cinemachine Cameras")]
//    public CinemachineVirtualCameraBase[] cameras;
//    CarControllers carControl;
//    PlayerControllers playerControl;

//    //[Header("Key to switch cameras")]
//    //public KeyCode switchKey = KeyCode.C;

//    private int currentCameraIndex = 0;
//    public bool isInsideCar = true;

//    void Start()
//    {
//        //SetActiveCamera(currentCameraIndex);
//        carControl = FindAnyObjectByType<CarControllers>();
//        playerControl = FindAnyObjectByType<PlayerControllers>();
//    }

//    private bool wasInCarLastFrame = false;

//    //void FixedUpdate()
//    //{
//    //    //bool isInCarNow = carControl.inCar && playerControl.isInside;
//    //    bool isInCarNow = carControl.inCar && playerControl.isInside && !playerControl.leftCar;

//    //    if (isInCarNow && !wasInCarLastFrame)
//    //    {
//    //        // Switch to the next camera only once when entering the car
//    //        currentCameraIndex = (currentCameraIndex + 1) % cameras.Length;
//    //        SetActiveCamera(currentCameraIndex);
//    //    }

//    //    else if (!isInCarNow && wasInCarLastFrame)
//    //    {
//    //        Debug.Log("camera changed");
//    //        currentCameraIndex = 0; // or any default exit camera
//    //        SetActiveCamera(currentCameraIndex);
//    //    }

//    //    // Update the state for the next frame
//    //    wasInCarLastFrame = isInCarNow;
//    //}

//    void Update()
//    {
//        //bool isInCarNow = carControl.inCar && playerControl.isInside && !playerControl.leftCar;

//        ////if (isInCarNow && !wasInCarLastFrame)
//        //if (carControl.inCar && playerControl.isInside && !playerControl.leftCar && !wasInCarLastFrame)
//        //{
//        //    currentCameraIndex = (currentCameraIndex + 1) % cameras.Length;
//        //    SetActiveCamera(currentCameraIndex);
//        //}
//        ////else if (!isInCarNow && wasInCarLastFrame)
//        //else if (!carControl.inCar && !playerControl.isInside && playerControl.leftCar && wasInCarLastFrame)
//        //{
//        //    Debug.Log("camera changed");
//        //    currentCameraIndex = 0; // or any default exit camera
//        //    SetActiveCamera(currentCameraIndex);
//        //}

//        //wasInCarLastFrame = carControl.inCar && playerControl.isInside && !playerControl.leftCar;
//        //// wasInCarLastFrame = isInCarNow;



//        //if (carControl.inCar && playerControl.isInside && !playerControl.leftCar && !wasInCarLastFrame)
//        //{
//        //    //carCamera.Priority = 10;
//        //    //playerCamera.Priority = 0;
//        //}
//        //else if (!carControl.inCar && !playerControl.isInside && playerControl.leftCar && wasInCarLastFrame)
//        //{
//        //    //Debug.Log("camera changed");
//        //    //carCamera.Priority = 0;
//        //    //playerCamera.Priority = 10;
//        //}

//        //wasInCarLastFrame = carControl.inCar && playerControl.isInside && !playerControl.leftCar;

//        if (carControl.inCar && wasInCarLastFrame)
//        {
//            currentCameraIndex = (currentCameraIndex + 1) % cameras.Length;
//            SetActiveCamera(currentCameraIndex);
//        }

//        if (!carControl.inCar && !wasInCarLastFrame)
//        {
//            currentCameraIndex = 0; // or any default exit camera
//            SetActiveCamera(currentCameraIndex);
//        }

//        wasInCarLastFrame = carControl.inCar;

//    }


//    private void SetActiveCamera(int index)
//    {
//        for (int i = 0; i < cameras.Length; i++)
//        {
//            cameras[i].Priority = (i == index) ? 10 : 0;
//        }

//        Debug.Log("Switched to camera: " + cameras[index].name);
//    }
//}
