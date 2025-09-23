using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.InputSystem.OnScreen;
using Unity.VisualScripting;
/*
#if !UNITY_EDITOR && UNITY_ANDROID
using GooglePlayGames;
#endif
using UnityEngine.SocialPlatforms;
*/
[System.Serializable]
public class InputDriver : MonoBehaviour
{
    [Header("Global settings")]
    [SerializeField] public bool inputEnabled;
    [Header("Mobile input variables")]
    //most used stuff is serialized.
    [SerializeField]public Quaternion deviceRotation;
    [SerializeField] bool touched;
    [SerializeField]public float roll;
    public static bool HasGyroscope {get {return SystemInfo.supportsGyroscope;}}
    private static bool gyroInitialized = false;
    public bool hasGyro;
    Quaternion referenceRotation = Quaternion.identity;
    [SerializeField] private bool touchThrust;

    [Header("Gamepad/Keyboard input variables")]
    //Keyboard variables
    [SerializeField] private bool KBCWPressed;
    [SerializeField]private bool KBCCWPressed;
    [SerializeField]private bool KBThrustPressed;
    public InputAction KBthrustAction;
    public InputAction KBCWAction;
    public InputAction KBCCWAction;
    [Header("On Screen Control stuff")]
    [SerializeField]private bool OSCWPressed;
    [SerializeField]private bool OSCCWPressed;
    [SerializeField]private bool OSThrustPressed;
    public InputAction OSthrustAction;
    public InputAction OSCWAction;
    public InputAction OSCCWAction;
    [Header("Amalgam variables")]
    public bool GoThrust;
    public bool GoCw;
    public bool GoCcw;
    protected void OnEnable(){
        
    }
    // Start is called before the first frame update
    void Start(){
        touched = false;
        gyroInitialized = false;
        OSthrustAction.Enable();
        OSCWAction.Enable();
        OSCCWAction.Enable();
        KBthrustAction.Enable();
        KBCWAction.Enable();
        KBCCWAction.Enable();
    }

    void FixedUpdate()
    {
        if(!inputEnabled){return;} //Only accept input when input is enabled.
        TrackRollData(); //always be checking the roll data.
        touchThrust = FilterTouchInput(); //Setting the touch thrust, filtering out other control methods.
        SetOSControlValues();
        SetKBControlValues();
        //keyboard values are set by events.
        GoCw = OSCWPressed | KBCWPressed ? true : false;
        GoCcw = OSCCWPressed | KBCCWPressed ? true : false;
        GoThrust = OSThrustPressed | KBThrustPressed | touchThrust ? true : false;

    }

    public void EnableInput(){
        inputEnabled = true;
    }
    public void DisableInput(){
        inputEnabled = false;
    }
    private bool FilterTouchInput(){
        if(Input.touchCount == 0){return false;} //Don't run thrust if untouched
        if(Time.timeScale == 0.0f){return false;} //Don't run thrust if paused
        //if (OSthrustAction.ReadValue<float>() == 1.0f){ return false;} //Don't run Thrust if on screen thrust is touched
        if (OSCWAction.ReadValue<float>() == 1.0f & Input.touchCount == 1){return false;} //Don't run thrust if only on screen CW is touched
        if (OSCCWAction.ReadValue<float>() == 1.0f & Input.touchCount == 1){return false;} //Don't run thrust if if only on screen CCW is touched
        //If any of the above are true, we're not considering ourselves touched.
        //If we get here, then we're good to use thrust.
        return true;
    }
    private void SetOSControlValues(){
        OSThrustPressed = OSthrustAction.ReadValue<float>() == 1.0f ? true : false;
        OSCWPressed = OSCWAction.ReadValue<float>() == 1.0f ? true : false;
        OSCCWPressed = OSCCWAction.ReadValue<float>() == 1.0f ? true : false;
    }

#region keyboardControlValues
    //The below methods are for keyboard input

    private void SetKBControlValues(){
        KBThrustPressed = KBthrustAction.ReadValue<float>() == 1.0f ? true : false;
        KBCWPressed = KBCWAction.ReadValue<float>() == 1.0f ? true : false;
        KBCCWPressed = KBCCWAction.ReadValue<float>() == 1.0f ? true : false;
    }
#endregion

    private void TrackRollData(){
        if (!HasGyroscope){
            roll = 0.0f;
            hasGyro = false;
        }else{
            hasGyro = true;
            if (!gyroInitialized){
                Input.gyro.enabled = true;                // enable the gyroscope
                Input.gyro.updateInterval = 0.0167f;    // set the update interval to it's highest value (60 Hz)
                gyroInitialized = true;
                roll = 0.0f;
            }else{
                if (Input.gyro.gravity == Vector3.zero){
                    roll = GetRollDataFallback();
                }else{
                    roll = GetRollDataFromGravity(Input.gyro.gravity);
                    if(roll > 20.0f & roll < 340.0f){
                    }
                }
            }
        }
    }


    private float GetRollDataFallback(){
        Quaternion eliminationOfXY = Quaternion.Inverse(Quaternion.FromToRotation(referenceRotation * Vector3.forward, deviceRotation * Vector3.forward));
        Quaternion rotationZ = eliminationOfXY * deviceRotation;
        return rotationZ.eulerAngles.z;
    }
    private float GetRollDataFromGravity(Vector3 gravData){
        return gravData.x * -45.0f;

    }

}
