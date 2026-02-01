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

/// <summary>
/// The input driver has a particular flow to its logic.
/// All potential inputs must have input-dependent variants.
/// Amalgam variables only check for whether one of the possiblities is being used.
/// </summary>






[System.Serializable]
public class InputDriver : MonoBehaviour
{
    [Header("Global settings")]
    [SerializeField] public bool inputEnabled;

    [Header("Mobile input variables")]
    [SerializeField] private float deviceRoll;
    public static bool HasGyroscope {get {return SystemInfo.supportsGyroscope;}}
    [System.NonSerialized] private static bool gyroInitialized = false;
    [System.NonSerialized] public bool hasGyro;
    [System.NonSerialized] private Quaternion deviceRotation;

    [System.NonSerialized] private Quaternion referenceRotation = Quaternion.identity;
    [System.NonSerialized] private bool touchThrust;
    [System.NonSerialized] private int touchCount;
    [System.NonSerialized] private bool touchBoostTriggered;

    [Header("Gamepad/Keyboard input variables")]
    //Keyboard variables
    [SerializeField] private float KeyboardRollOffset;
    [SerializeField] private float KeyboardSensitivity;

    [System.NonSerialized] private bool KBCWPressed;
    [System.NonSerialized] private bool KBCCWPressed;
    [System.NonSerialized] private bool KBThrustPressed;
    [System.NonSerialized] private bool KBBoostPressed;
    [SerializeField] private InputAction KBthrustAction;
    [SerializeField] private InputAction KBCWAction;
    [SerializeField] private InputAction KBCCWAction;
    [SerializeField] private InputAction KBBoostAction;
    [Header("On Screen Control stuff")]
    [SerializeField] private float OSRollOffset;
    [SerializeField] private float OSRollSensitivity;

    [System.NonSerialized] private bool OSCWPressed;
    [System.NonSerialized] private bool OSCCWPressed;
    [System.NonSerialized] private bool OSThrustPressed;
    [System.NonSerialized] private bool OSBoostPressed;
    [SerializeField] private InputAction OSthrustAction;
    [SerializeField] private InputAction OSCWAction;
    [SerializeField] private InputAction OSCCWAction;
    [SerializeField] private InputAction OSBoostAction;
    [Header("Amalgam variables")]
    public bool GoThrust;
    public bool GoCw;
    public bool GoCcw;
    public bool GoBoost;  // Boost : Multi-touch (mobile) or Thrust + M key (Pc/Gamepad)
    public float aimAngle;

    protected void OnEnable(){
        
    }
    // Start is called before the first frame update
    void Start()
    {
        InitializeInputs();
    }

    private void InitializeInputs()
    {
        gyroInitialized = false;
        touchCount = 0;
        OSthrustAction.Enable();
        OSCWAction.Enable();
        OSCCWAction.Enable();
        OSBoostAction.Enable();
        KBthrustAction.Enable();
        KBCWAction.Enable();
        KBCCWAction.Enable();
        KBBoostAction.Enable();
    }

    void FixedUpdate()
    {
        if (!inputEnabled) { return; } //Only accept input when input is enabled.
        

        //Motion control checkers
        TrackRollData(); //always be checking the roll data.
        touchThrust = FilterTouchInput(); //Setting the touch thrust, filtering out other control methods.
        //OSC control checkers
        SetOSControlValues();

        //Keyboard/Gamepad control checkers
        SetKBControlValues();
        //keyboard values are set by events.


        //Amalgam variable checkers.
        GoCw = OSCWPressed | KBCWPressed ? true : false;
        GoCcw = OSCCWPressed | KBCCWPressed ? true : false;
        GoThrust = OSThrustPressed | KBThrustPressed | touchThrust ? true : false;

        //Final Aim Angle
        aimAngle = deviceRoll + OSRollOffset + KeyboardRollOffset;
        // Boost detection: Multi-touch (mobile) or Thrust + L Shift key (Pc/Gamepad)

        GoBoost = touchBoostTriggered | OSBoostPressed | KBBoostPressed;
    }

    public void EnableInput(){
        inputEnabled = true;
    }
    public void DisableInput(){
        inputEnabled = false;
    }
    private bool FilterTouchInput(){
        touchCount = Input.touchCount;
        if(touchCount == 0){return false;} //Don't run thrust if untouched
        if(PauseUtility.IsPaused){return false;} //Don't run thrust if paused
        //if (OSthrustAction.ReadValue<float>() == 1.0f){ return false;} //Don't run Thrust if on screen thrust is touched
        if (OSCWAction.ReadValue<float>() == 1.0f & Input.touchCount == 1){return false;} //Don't run thrust if only on screen CW is touched
        if (OSCCWAction.ReadValue<float>() == 1.0f & Input.touchCount == 1) { return false; } //Don't run thrust if if only on screen CCW is touched
        //If any of the above are true, we're not considering ourselves touched.
        //If we get here, then we're good to use thrust.

        //checking for multitouch AKA boost
        touchBoostTriggered = touchCount > 1;
        return true;
    }
    private void SetOSControlValues(){
        OSThrustPressed = OSthrustAction.ReadValue<float>() == 1.0f ? true : false;
        OSCWPressed = OSCWAction.ReadValue<float>() == 1.0f ? true : false;
        OSCCWPressed = OSCCWAction.ReadValue<float>() == 1.0f ? true : false;
        OSBoostPressed = OSBoostAction.ReadValue<float>() == 1.0f ? true : false;
        if (OSCWPressed & OSCCWPressed) { return; }
        if (OSCWPressed)
        {
            OSRollOffset -= 0.25f * OSRollSensitivity;
        }
        if(OSCCWPressed)
        {
            OSRollOffset += 0.25f * OSRollSensitivity;
        }
    }

#region keyboardControlValues
    //The below methods are for keyboard input

    private void SetKBControlValues(){
        
        KBThrustPressed = KBthrustAction.ReadValue<float>() == 1.0f ? true : false;
        KBCWPressed = KBCWAction.ReadValue<float>() == 1.0f ? true : false;
        KBCCWPressed = KBCCWAction.ReadValue<float>() == 1.0f ? true : false;
        KBBoostPressed = KBBoostAction.ReadValue<float>() == 1.0f ? true : false;
        if (KBCWPressed & KBCCWPressed) { return; }
        if (KBCWPressed)
        {
            KeyboardRollOffset -= 0.25f * KeyboardSensitivity;
        }
        if (KBCCWPressed)
        {
            KeyboardRollOffset += 0.25f * KeyboardSensitivity;
        }
    }
#endregion

    private void TrackRollData(){
        if (!HasGyroscope){
            deviceRoll = 0.0f;
            hasGyro = false;
        }else{
            hasGyro = true;
            if (!gyroInitialized){
                Input.gyro.enabled = true;                // enable the gyroscope
                Input.gyro.updateInterval = 0.0167f;    // set the update interval to it's highest value (60 Hz)
                gyroInitialized = true;
                deviceRoll = 0.0f;
            }else{
                if (Input.gyro.gravity == Vector3.zero){
                    deviceRoll = GetRollDataFallback();
                }else{
                    deviceRoll = GetRollDataFromGravity(Input.gyro.gravity);
                    if(deviceRoll > 20.0f & deviceRoll < 340.0f){
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
