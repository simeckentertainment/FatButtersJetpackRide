using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;
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
    [System.NonSerialized] private static bool gyroInitialized = false;
    [System.NonSerialized] public bool hasGyro;
    [System.NonSerialized] private Quaternion deviceRotation;

    [System.NonSerialized] private Quaternion referenceRotation = Quaternion.identity;
    [System.NonSerialized] private bool touchThrust;
    [System.NonSerialized] private int touchCount;
    [System.NonSerialized] private bool touchBoostTriggered;

    [Header("Keyboard input variables")]
    //Keyboard variables
    [SerializeField] private bool KBCWPressed;
    [SerializeField] private bool KBCCWPressed;
    [System.NonSerialized] private bool KBThrustPressed;
    [System.NonSerialized] private bool KBBoostPressed;
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
    }

    void FixedUpdate()
    {
        if (!inputEnabled) { return; } //Only accept input when input is enabled.

        if(!OnScreenControlsEnabled){
            //Motion control checkers
            TrackRollData(); //always be checking the roll data.
            touchThrust = FilterTouchInput(); //Setting the touch thrust, filtering out other control methods.
        }
        //OSC control checkers
        if(OnScreenControlsEnabled){SetOSControlValues();}

        //Keyboard control checkers
        SetKBControlValues();
        //Gamepad Control checkers
        SetGPControlValues();

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
    private bool FilterTouchInput()
    {
        if(OSCCWPressed || OSCWPressed || OSThrustPressed){return false;} //don't get tricked by the on screen controls.
        touchCount = Input.touchCount;
        if (touchCount == 0) { return false; } //Don't run thrust if untouched
        if (PauseUtility.IsPaused) { return false; } //Don't run thrust if paused
        //if (OSthrustAction.ReadValue<float>() == 1.0f){ return false;} //Don't run Thrust if on screen thrust is touched
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
            return;
        }
        
        if (OSCWPressed && aimAngle > -45.0f)
        {
            OSCAimAngle -= 0.25f * OSAccelSensitivity;
        }
        if (OSCCWPressed && aimAngle < 45.0f)
        {
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

    private void TrackRollData()
    {
        if (!HasGyroscope)
        {
            deviceRoll = 0.0f;
            hasGyro = false;
        }
        else
        {
            hasGyro = true;
            if (!gyroInitialized)
            {
                Input.gyro.enabled = true;                // enable the gyroscope
                Input.gyro.updateInterval = 0.0167f;    // set the update interval to it's highest value (60 Hz)
                gyroInitialized = true;
                deviceRoll = 0.0f;
            }
            else
            {
                if (Input.gyro.gravity == Vector3.zero)
                {
                    deviceRoll = GetRollDataFallback();
                }
                else
                {
                    deviceRoll = GetRollDataFromGravity(Input.gyro.gravity);
                    if (deviceRoll > 20.0f & deviceRoll < 340.0f)
                    {
                    }
                }
            }
        }
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



    




}
