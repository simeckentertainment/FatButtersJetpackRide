using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DisplayMobileData : MonoBehaviour
{
    [Header("Touch stuff")]
    public TMP_Text touchCounter;
    public TMP_Text touchPhaseDisplay;
    TouchPhase touchPhase;
    [Header("Gyro stuff")]
    public TMP_Text GyroDisplay;
    private static bool gyroInitialized = false;
    public static bool HasGyroscope {get {return SystemInfo.supportsGyroscope;}}
    Quaternion referenceRotation = Quaternion.identity;
    Quaternion deviceRotation;
    [SerializeField] Transform Butters;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    void OnEnable() {
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        touchCounter.text = Input.touchCount.ToString();
        string touchPhaseText = "";
        touchPhaseDisplay.text = touchPhaseText;
        if(Input.touchCount > 0){
            Touch touch = Input.GetTouch(0);
            switch(touch.phase){
                case TouchPhase.Began:
                    touchPhaseText = "Began";
                    break;
                case TouchPhase.Moved:
                    touchPhaseText = "Moved";
                    break;
                case TouchPhase.Ended:
                    touchPhaseText = "Ended";
                    break;
                case TouchPhase.Stationary:
                    touchPhaseText = "Stationary";
                    break;
                default: //this isn't called.
                    touchPhaseText = "Nothing";
                    break;
            }
        }
        touchPhaseDisplay.text = touchPhaseText;
        if(HasGyroscope){
            deviceRotation = GetGyro();
            Quaternion eliminationOfXY = Quaternion.Inverse(Quaternion.FromToRotation(referenceRotation * Vector3.forward, deviceRotation * Vector3.forward));
            Quaternion rotationZ = eliminationOfXY * deviceRotation;
            float roll = rotationZ.eulerAngles.z;
            Butters.rotation = Quaternion.Inverse(rotationZ);
            GyroDisplay.text = rotationZ.ToString();

        } else {
            GyroDisplay.text = "no gyro";
        }
    }
    private static void InitGyro() {
         if (HasGyroscope) {
             Input.gyro.enabled = true;                // enable the gyroscope
             Input.gyro.updateInterval = 0.0167f;    // set the update interval to it's highest value (60 Hz)
         }
         gyroInitialized = true;
     }

    public static Quaternion GetGyro() {
         if (!gyroInitialized) {
             InitGyro();
         }
         return HasGyroscope
             ? ReadGyroscopeRotation()
             : Quaternion.identity;
     }
    private static Quaternion ReadGyroscopeRotation() {
         return new Quaternion(0.5f, 0.5f, -0.5f, 0.5f) * Input.gyro.attitude * new Quaternion(0, 0, 1, 0);
     }
     
}
