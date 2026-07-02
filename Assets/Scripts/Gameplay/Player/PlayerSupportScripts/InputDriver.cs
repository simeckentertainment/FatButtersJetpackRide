using UnityEngine;
using UnityEngine.InputSystem;
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
    public static bool HasGyroscope { get { return SystemInfo.supportsGyroscope; } }
    public static bool HasAccelerometerFallback { get {return SystemInfo.supportsAccelerometer;}}
    public static bool MotionControlsAvailable;
    [System.NonSerialized] private static bool gyroInitialized = false;
    [System.NonSerialized] public bool hasMotionControls;
    [System.NonSerialized] private Quaternion deviceRotation;

    [System.NonSerialized] private Quaternion referenceRotation = Quaternion.identity;
    [System.NonSerialized] private bool touchThrust;
    [System.NonSerialized] private int touchCount;
    [System.NonSerialized] private bool touchBoostTriggered;
    [SerializeField] CameraRotationManager cameraRot;

    [Header("Keyboard input variables")]
    //Keyboard variables
    [SerializeField] private bool KBCWPressed;
    [SerializeField] private bool KBCCWPressed;
    [SerializeField] private bool KBThrustPressed;
    [SerializeField] private bool KBBoostPressed;
    [SerializeField] private float KBMinAngle;
    [SerializeField] private float KBMaxAngle;
    [System.NonSerialized] private float KBCurrentAngle;
    [SerializeField] private float KBAccelerationTimer;
    [SerializeField] private float KBMaxSpeedFrame;
    [SerializeField] private InputAction KBthrustAction;
    [SerializeField] private InputAction KBCWAction;
    [SerializeField] private InputAction KBCCWAction;
    [SerializeField] private InputAction KBBoostAction;

    [Header("Gamepad input vars")]
    [SerializeField] float TriggerActivationMinimum;
    [SerializeField] float JoystickActivationMinimum;
    [SerializeField] private float GPAimVal;
    [SerializeField] private bool GPThrustPressed;
    [SerializeField] private bool GPBoostPressed;
    [SerializeField] private InputAction GPThrustAction;
    [SerializeField] private InputAction GPAimAction;
    [SerializeField] private InputAction GPBoostAction;

    [Header("OnScreen Control Vars")]
    [SerializeField] private bool OnScreenControlsEnabled;
    [SerializeField] private float OSCAimAngle;
    [SerializeField] private float OSAccelSensitivity;
    [SerializeField] private bool OSCWPressed;
    [SerializeField] private bool OSCCWPressed;
    [SerializeField] private bool OSThrustPressed;
    [System.NonSerialized] private float OSBoostDelayCounter;
    [SerializeField] private float OSBoostDelayThreshold;
    [SerializeField] private bool OSBoostPressed;

    [SerializeField] private InputAction OSthrustAction;
    [SerializeField] private InputAction OSCWAction;
    [SerializeField] private InputAction OSCCWAction;
    [SerializeField] private InputAction OSBoostAction;

    [Header("Amalgam variables")]
    public bool GoThrust;
    //GoCw and GoCcw are strictly to be used for the plus/minus particle rotations.
    public bool GoCw;
    public bool GoCcw;
    public bool GoBoost;  // Boost : Multi-touch (mobile) or Thrust + M key (Pc/Gamepad)
    public float aimAngle;

    protected void OnEnable()
    {

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
        GPAimAction.Enable();
        GPBoostAction.Enable();
        GPThrustAction.Enable();
        MotionControlsAvailable = HasGyroscope || HasAccelerometerFallback;
        cameraRot.SetStationary();
    }

    void FixedUpdate()
    {

        if (!inputEnabled) { return; } //Only accept input when input is enabled.
        SetAllControlValues();
        switch (QueryCurrentInputMethod())
        {
            case CurrentInputMethod.OnScreenControls:
                cameraRot.SetStationary();
                ClearStaleGPInputs();
                ClearStaleKBInputs();
                ClearStaleMotionControlInputs();
                break;
            case CurrentInputMethod.Gamepad:
                cameraRot.SetStationary();
                ClearStaleKBInputs();
                ClearStaleMotionControlInputs();
                ClearStaleOSCInputs();
                break;
            case CurrentInputMethod.Keyboard:
                cameraRot.SetStationary();
                ClearStaleGPInputs();
                ClearStaleMotionControlInputs();
                ClearStaleOSCInputs();
                break;
            case CurrentInputMethod.MotionControls:
                cameraRot.SetMotionControlled();
                ClearStaleGPInputs();
                ClearStaleKBInputs();
                ClearStaleOSCInputs();
                touchThrust = FilterTouchInput();
                break;
        }
        //Amalgam variable checkers.
        GoThrust = OSThrustPressed || KBThrustPressed || touchThrust || GPThrustPressed;

        //Final Aim Angle
        aimAngle = deviceRoll + OSCAimAngle + KBCurrentAngle + (GPAimVal * -45);
        // Boost detection: Multi-touch (mobile) or Thrust + L Shift key (Pc/Gamepad)

        GoBoost = touchBoostTriggered || OSBoostPressed || KBBoostPressed || GPBoostPressed;
    }

    public void EnableInput()
    {
        inputEnabled = true;
    }
    public void DisableInput()
    {
        inputEnabled = false;
    }

    public CurrentInputMethod QueryCurrentInputMethod()
    {
        // Go in order from least likely to most likely. On screen controls, then keyboard, then controller, then motion.
        //We're only doing one control method at a time.
        if(OnScreenControlsEnabled) //&& (OSCWPressed || OSCCWPressed || OSThrustPressed || OSBoostPressed))
        {
            return CurrentInputMethod.OnScreenControls;
        }
        if(KBCWPressed || KBCCWPressed || KBThrustPressed || KBBoostPressed)
        {
            return CurrentInputMethod.Keyboard;
        }
        if(GPBoostPressed || GPThrustPressed || Mathf.Abs(GPAimVal) > JoystickActivationMinimum)
        {
            return CurrentInputMethod.Gamepad;
        }
        return CurrentInputMethod.MotionControls;


    }
    private void SetAllControlValues()
    {
        SetOSControlValues();
        SetGPControlValues();
        SetKBControlValues();
        TrackMotionControlRollData();
    }
    public bool CheckForMotionControls()
    {
        if(!SystemInfo.supportsGyroscope && !SystemInfo.supportsAccelerometer) //no motion controls.
        {
            deviceRoll = 0.0f;
            return false;
        }
        if(!SystemInfo.supportsGyroscope && SystemInfo.supportsAccelerometer) { //no gyro, yes accelerometer.
            deviceRoll = Input.acceleration.x * -45f;
            return true;
        }
        if (SystemInfo.supportsGyroscope)
        {
            if(!gyroInitialized) InitializeGyro();
            return true;
        }
        return false;
    }
    private bool FilterTouchInput()
    {
        if(OSCCWPressed || OSCWPressed || OSThrustPressed){return false;} //don't get tricked by the on screen controls.
        if (PauseUtility.IsPaused) { return false; } //Don't run thrust if paused
        touchCount = Input.touchCount;
        if (touchCount == 0) { return false; } //Don't run thrust if untouched

        if (OSCWAction.ReadValue<float>() == 1.0f & Input.touchCount == 1) { return false; } //Don't run thrust if only on screen CW is touched
        if (OSCCWAction.ReadValue<float>() == 1.0f & Input.touchCount == 1) { return false; } //Don't run thrust if if only on screen CCW is touched
        //If any of the above are true, we're not considering ourselves touched.
        //My head will forever be in the gutter. ~Randy
        //If we get here, then we're good to use thrust. :F

        //checking for multitouch AKA boost
        touchBoostTriggered = touchCount > 1;
        return true;
    }
    public void ToggleOnScreenControls(bool enabled)
    {
        OnScreenControlsEnabled = enabled;
    }
    private void SetOSControlValues()
    {
        OSThrustPressed = OSthrustAction.ReadValue<float>() == 1.0f;
        OSCWPressed = OSCWAction.ReadValue<float>() == 1.0f;
        OSCCWPressed = OSCCWAction.ReadValue<float>() == 1.0f;

        //Boost detection logic. On Screen Controls works a little differently than the rest in terms of boost.
        //To keep the controls as simple as possible, we're putting a timer on the OSCs. After a few seconds of
        //holding thrust, the boost kicks in automatically.

        if (OSThrustPressed && !OSBoostPressed)
        {
            OSBoostDelayCounter++;

            if(OSBoostDelayCounter > OSBoostDelayThreshold) OSBoostPressed = true;

        } else
        {
            OSBoostDelayCounter = 0f;
        }

        if(OSBoostPressed && !OSThrustPressed) //turns off boost if thrust is released.
        {
            OSBoostPressed = false;
        }

        if((!OSCWPressed && !OSCCWPressed) || (OSCWPressed && OSCCWPressed)) //Only accept a single directional input at a time, or wait patiently for input.
        {
            OSCAimAngle = 0.0f;
            GoCw = GoCcw = false;
            return;
        }
        
        if (OSCWPressed && aimAngle > -45.0f)
        {
            //GoCw = true;
            OSCAimAngle -= 0.25f * OSAccelSensitivity;
        } else if (OSCCWPressed && aimAngle < 45.0f)
        {
            //GoCcw = true;
            OSCAimAngle += 0.25f * OSAccelSensitivity;
        }
    }

    #region keyboardControlValues
    //The below methods are for keyboard input

    private void SetKBControlValues()
    {
        KBThrustPressed = KBthrustAction.ReadValue<float>() == 1.0f;
        KBCWPressed = KBCWAction.ReadValue<float>() == 1.0f;
        KBCCWPressed = KBCCWAction.ReadValue<float>() == 1.0f;
        KBBoostPressed = KBBoostAction.ReadValue<float>() == 1.0f;
        if((!KBCWPressed && !KBCCWPressed) || (KBCWPressed && KBCCWPressed)) //Only accept a single directional input at a time, or wait patiently for input.
        {
            KBCurrentAngle = 0.0f;
            KBAccelerationTimer = 0;
            return;
        }
        if (KBCWPressed)
        {
            KBAccelerationTimer = Mathf.Clamp(++KBAccelerationTimer, 0.0f, KBMaxSpeedFrame); //Found a cool new way to do this! ~Randy
            KBCurrentAngle = Mathf.Lerp(KBMinAngle * -1.0f, KBMaxAngle * -1.0f, KBAccelerationTimer / KBMaxSpeedFrame);
        }
        if (KBCCWPressed)
        {
            KBAccelerationTimer = Mathf.Clamp(++KBAccelerationTimer, 0.0f, KBMaxSpeedFrame);
            KBCurrentAngle = Mathf.Lerp(KBMinAngle, KBMaxAngle, KBAccelerationTimer / KBMaxSpeedFrame);
        }
    }
    #endregion

    #region GamePadValues
    void SetGPControlValues()
    {
        GPAimVal = GPAimAction.ReadValue<float>();
        GPThrustPressed = GPThrustAction.ReadValue<float>() > TriggerActivationMinimum;
        GPBoostPressed = GPBoostAction.ReadValue<float>() > TriggerActivationMinimum;
    }

    #endregion

    private void TrackMotionControlRollData()
    {
        if(!HasGyroscope && !HasAccelerometerFallback) //no motion controls. SHOULD be dead code but safeguard here just in case.
        {
            hasMotionControls = false;
            deviceRoll = 0.0f;
            return;
        }
        if(!HasGyroscope && HasAccelerometerFallback) { //no gyro, yes accelerometer.
            hasMotionControls = true;
            deviceRoll = Input.acceleration.x * -45f;
            return;
        }
        if (HasGyroscope) //If we have the gyro, prefer that.
        {
            hasMotionControls = true;
            if(!gyroInitialized) InitializeGyro();
            deviceRoll = Input.gyro.gravity == Vector3.zero ? GetRollDataFallback() : GetRollDataFromGravity(Input.gyro.gravity);
        }
    }

    private void InitializeGyro()
    {
        Input.gyro.enabled = true;                // enable the gyroscope
        Input.gyro.updateInterval = 0.0167f;    // set the update interval to it's highest value (60 Hz)
        gyroInitialized = true;
        deviceRoll = 0.0f;
    }
    private float GetRollDataFallback()
    {
        Quaternion eliminationOfXY = Quaternion.Inverse(Quaternion.FromToRotation(referenceRotation * Vector3.forward, deviceRotation * Vector3.forward));
        Quaternion rotationZ = eliminationOfXY * deviceRotation;
        return rotationZ.eulerAngles.z;
    }
    private float GetRollDataFromGravity(Vector3 gravData)
    {
        return gravData.x * -45.0f;
    }

    private void ClearStaleMotionControlInputs()
    {
        deviceRoll = 0;
        touchThrust = touchBoostTriggered = false;
    }
    private void ClearStaleOSCInputs()
    {
        OSCWPressed = OSCCWPressed = OSThrustPressed = OSBoostPressed  = false;
    }
    private void ClearStaleGPInputs()
    {
        GPThrustPressed = GPBoostPressed = false;
        GPAimVal = 0f;
    }
    private void ClearStaleKBInputs()
    {
        KBCurrentAngle = KBAccelerationTimer = 0f;
        KBCWPressed = KBCCWPressed = KBThrustPressed =KBBoostPressed = false;
    }
    public enum CurrentInputMethod {MotionControls, Gamepad, OnScreenControls, Keyboard}

}
