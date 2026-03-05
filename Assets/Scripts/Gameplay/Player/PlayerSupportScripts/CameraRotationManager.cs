using UnityEngine;

public class CameraRotationManager : MonoBehaviour
{
    [SerializeField] InputDriver id;
    [SerializeField] Transform virtualCam;
    [SerializeField] DeviceType deviceType;
    enum DeviceType {HandheldGyro, Stationary};

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(deviceType == DeviceType.Stationary){ return;} //Don't need to handle rotation for stationary devices!
        
        //We're manually taking over camera rotations because letting Cinemachine do it was leading to unpredictable results.
        virtualCam.localRotation = Quaternion.Euler(0.0f,0.0f,id.aimAngle*2); 
    }




}
