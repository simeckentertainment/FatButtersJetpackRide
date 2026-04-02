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
    [SerializeField] public bool inputEnabled {get; private set;}
    [Header("Mobile input variables")]
    public float deviceRoll {get; private set;}
    public static bool HasGyroscope { get { return SystemInfo.supportsGyroscope; } }
    [System.NonSerialized] private static bool gyroInitialized = false;
    public bool hasGyro {get; private set;}
    [System.NonSerialized] private Quaternion deviceRotation;

    [System.NonSerialized] private Quaternion referenceRotation = Quaternion.identity;
    [field:SerializeField] public bool touchThrust {get; private set;}
    [field:SerializeField] public int touchCount {get; private set;}
    [field:SerializeField] public bool touchBoostTriggered {get; private set;}

    [Header("Keyboard input variables")]
    //Keyboard variables
    [field:SerializeField] public bool KBCWPressed {get; private set;}
    [field:SerializeField] public bool KBCCWPressed {get; private set;}
    [field:SerializeField] public bool KBThrustPressed {get; private set;}
    [field:SerializeField] public bool KBBoostPressed {get; private set;}
    [field:SerializeField] public float KBMinAngle {get; private set;}
    [field:SerializeField] public float KBMaxAngle {get; private set;}
    public float KBCurrentAngle {get; private set;}
    [field:SerializeField] private float KBAccelerationTimer;
    [field:SerializeField] private float KBMaxSpeedFrame;
    [SerializeField] private InputAction KBthrustAction;
    [SerializeField] private InputAction KBCWAction;
    [SerializeField] private InputAction KBCCWAction;
    [SerializeField] private InputAction KBBoostAction;

    [Header("Gamepad input vars")]
    [SerializeField] float TriggerActivationMinimum;
    [SerializeField] float JoystickActivationMinimum;
    [field:SerializeField] public float GPAimVal {get; private set;}
    [field:SerializeField] public bool GPThrustPressed {get; private set;}
    [field:SerializeField] public bool GPBoostPressed  {get; private set;}
    [field:SerializeField] public bool GPJumpPressed {get; private set;}
    [SerializeField] private InputAction GPThrustAction;
    [SerializeField] private InputAction GPAimAction;
    [SerializeField] private InputAction GPBoostAction;
    [SerializeField] private InputAction GPJumpAction;

    [Header("OnScreen Control Vars")]
    public float OSRollOffset {get; private set;}
    public float OSRollSensitivity {get; private set;}
    [field:SerializeField] public bool OSCWPressed {get; private set;}
    [field:SerializeField] public bool OSCCWPressed {get; private set;}
    [field:SerializeField] public bool OSThrustPressed {get; private set;}
    [field:SerializeField] public bool OSBoostPressed {get; private set;}

    [SerializeField] private InputAction OSthrustAction;
    [SerializeField] private InputAction OSCWAction;
    [SerializeField] private InputAction OSCCWAction;
    [SerializeField] private InputAction OSBoostAction;

    [Header("Amalgam variables")]
    [field:SerializeField] public bool GoThrust {get; private set;}
    //GoCw and GoCcw are strictly to be used for the plus/minus particle rotations.
    [field:SerializeField] public bool GoCw {get; private set;}
    [field:SerializeField] public bool GoCcw {get; private set;}
    [field:SerializeField] public bool GoBoost {get; private set;}  // Boost : Multi-touch (mobile) or Thrust + M key (Pc/Gamepad)
    public bool aimAngleOverride {get; private set;}
    public float aimAngleOverrideVal {get; private set;}
    [field:SerializeField] public float aimAngle {get; private set;}

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
        aimAngleOverride = false;
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
        GPJumpAction.Enable();
    }

    void FixedUpdate()
    {
        if (!inputEnabled) { return; } //Only accept input when input is enabled.

        //set the aim angle override to false every frame. The override itself handles undoing this.
        aimAngleOverride = false;

        //Motion control checkers
        TrackRollData(); //always be checking the roll data.
        touchThrust = FilterTouchInput(); //Setting the touch thrust, filtering out other control methods.
        //OSC control checkers
        SetOSControlValues();


        //Keyboard control checkers
        SetKBControlValues();
        //Gamepad Control checkers
        SetGPControlValues();



        //Amalgam variable checkers.
        GoThrust = OSThrustPressed || KBThrustPressed || touchThrust || GPThrustPressed;

        //Final Aim Angle
        if (aimAngleOverride)
        {
            aimAngle = aimAngleOverrideVal;
        } 
        else 
        {
            SetAimAngleVal( deviceRoll + OSRollOffset + KBCurrentAngle + (GPAimVal * -45));
        }
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
    private void SetOSControlValues()
    {
        OSThrustPressed = OSthrustAction.ReadValue<float>() == 1.0f;
        OSCWPressed = OSCWAction.ReadValue<float>() == 1.0f;
        OSCCWPressed = OSCCWAction.ReadValue<float>() == 1.0f;
        OSBoostPressed = OSBoostAction.ReadValue<float>() == 1.0f;
        if (OSCWPressed & OSCCWPressed) { return; }
        if (OSCWPressed && aimAngle > -45.0f)
        {
            OSRollOffset -= 0.25f * OSRollSensitivity;
        }
        if (OSCCWPressed && aimAngle < 45.0f)
        {
            OSRollOffset += 0.25f * OSRollSensitivity;
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
        GPJumpPressed = GPJumpAction.ReadValue<bool>();
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

    private void SetAimAngleVal(float aimVal)
    {
        aimAngle = aimVal;
    }

    public void SetAimAngleOverrideVal(float overrideVal)
    {
        aimAngleOverride = true;
        aimAngleOverrideVal = overrideVal;
    }

}
