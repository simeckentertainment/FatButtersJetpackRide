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

    [System.NonSerialized] bool DoubleTapDetected;
    [System.NonSerialized] int DoubleTapFrameThreshold = 12;
    [System.NonSerialized] int DoubleTapFrameCounter = 0;
    [System.NonSerialized] bool DTLInitialTap;
    [System.NonSerialized] bool DTLInitialRelease;
    [System.NonSerialized] bool DTLSecondTap;

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
    [SerializeField] private float KeyboardRollOffset;
    [System.NonSerialized] private bool KBCWPressed;
    [System.NonSerialized] private bool KBCWDoublePressed;
    [System.NonSerialized] private bool KBCCWPressed;
    [System.NonSerialized] private bool KBCCWDoublePressed;
    [System.NonSerialized] private bool KBThrustPressed;
    [System.NonSerialized] private bool KBBoostPressed;
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
    [SerializeField] private float OSRollOffset;
    [SerializeField] private float OSRollSensitivity;
    [SerializeField] private bool OSCWPressed;
    [SerializeField] private bool OSCCWPressed;
    [SerializeField] private bool OSThrustPressed;
    [SerializeField] private bool OSBoostPressed;

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


        //Motion control checkers
        TrackRollData(); //always be checking the roll data.
        touchThrust = FilterTouchInput(); //Setting the touch thrust, filtering out other control methods.
        //OSC control checkers
        SetOSControlValues();

        //The listener for direction double taps on keyboard and gamepad.
        DoubleTapDetected = DoubleTapListener();

        //Keyboard control checkers
        SetKBControlValues();
        //Gamepad Control checkers
        SetGPControlValues();



        //Amalgam variable checkers.
        GoCw = OSCWPressed;
        GoCcw = OSCCWPressed;
        GoThrust = OSThrustPressed || KBThrustPressed || touchThrust || GPThrustPressed;

        //Final Aim Angle
        aimAngle = deviceRoll + OSRollOffset + KeyboardRollOffset + (GPAimVal * -45);
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
        //If we get here, then we're good to use thrust.

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
        if (OSCWPressed)
        {
            OSRollOffset -= 0.25f * OSRollSensitivity;
        }
        if (OSCCWPressed)
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
        if (KBCWPressed & KBCCWPressed) {
            KeyboardRollOffset = 0.0f;
            return; }


        if(!KBCWPressed || !KBCCWPressed)
        {
            KeyboardRollOffset = 0.0f;
        }
        if (KBCWPressed)
        {
            KeyboardRollOffset = -22.5f;
        }
        if (KBCCWPressed)
        {
            KeyboardRollOffset = 22.5f;
        }
        if (DoubleTapDetected)
        {
            KeyboardRollOffset *= 2.0f;
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



    bool DoubleTapListener()
    {
        //I was trying to avoid building this into the input driver but Unity doens't provide
        //a native way to do Doubletap+press. So, here we go! ~Randy
        if(KBCWPressed && KBCCWPressed) { return false; } //security measure against false presses.
        if (!DTLInitialTap && !DTLInitialRelease && !DTLSecondTap) {
            //Listen for initial press
            DTLInitialTap = KBCWPressed || KBCCWPressed;
            return false;
         }
        if(DTLInitialTap && !DTLInitialRelease && !DTLSecondTap)
        {
            //Listen for initial release.
            DTLInitialRelease = !KBCWPressed && !KBCCWPressed;
            return false;
        }
        if (DTLInitialTap && DTLInitialRelease && !DTLSecondTap)
        {
            //Start listening now for that crucial second tap.
            DoubleTapFrameCounter++;
            if ((DoubleTapFrameCounter < DoubleTapFrameThreshold) && (KBCWPressed || KBCCWPressed))
            { //Nesting if statements... Not crazy about it but at least it's only 2 deep.
                //If we get that second tap, we can initiate a run.
                //To be clear, with this configuration, pressing CCW and then CW consitutes
                //a run to the right, and I'm OK with that. ~Randy.
                DTLSecondTap = true;
                DoubleTapFrameCounter = 0;
                return true;
            }
            if (DoubleTapFrameCounter >= DoubleTapFrameThreshold)
            {
                //If we don't get the second tap we need, we reset.
                DTLInitialRelease = DTLInitialTap = false;
                return false;
            }
        }
        if(DTLInitialTap && DTLInitialRelease && DTLSecondTap)
        {
            //If we get that second tap, we're good to run.
            DoubleTapDetected = true;

            //As long as that button is held, we stay running. Otherwise, reset.
            if(!KBCWPressed || !KBCCWPressed)
            {
                DTLInitialRelease = DTLInitialTap = DTLSecondTap = false;
                return false;
            }
        }
        return false; //I can't think of an edge case, but I'll keep this here just in case.
    }




}
