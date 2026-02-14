using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GyroSandboxTestingScript : MonoBehaviour
{
    [Header("Gyro stuff")]
    [SerializeField]  TMP_Text xDynamic;
    [SerializeField]  TMP_Text yDynamic;
    [SerializeField]  TMP_Text zDynamic;
    [SerializeField]  TMP_Text QuatDynamic;
    [SerializeField]  TMP_Text GravDynamic;
    public TMP_Text GyroDisplay;
    private static bool gyroInitialized = false;
    public static bool HasGyroscope {get {return SystemInfo.supportsGyroscope;}}
    Quaternion referenceRotation = Quaternion.identity;
    Quaternion deviceRotation;
    Vector3 limitedRotation;
    // Start is called before the first frame update
    void Start()
    {
        gyroInitialized = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!gyroInitialized) {
             InitGyro();
         }
        if (HasGyroscope){
            GyroDisplay.text = "Has gyro";
            QuatDynamic.text = Input.gyro.attitude.ToString();
            xDynamic.text = Input.gyro.attitude.eulerAngles.x.ToString();
            yDynamic.text = Input.gyro.attitude.eulerAngles.y.ToString();
            zDynamic.text = Input.gyro.attitude.eulerAngles.z.ToString();
            GravDynamic.text = Input.gyro.gravity.ToString();
            limitedRotation = new Vector3(0.0f,0.0f,Input.gyro.gravity.x * 90.0f);
            transform.rotation = Quaternion.Euler(limitedRotation);
        } else {
            GyroDisplay.text = "no gyro";
        }
    }



    private static void InitGyro() {
        if (HasGyroscope) {
            Input.gyro.enabled = true;                // enable the gyroscope
            Input.gyro.updateInterval = 0.0167f;    // set the update interval to it's highest value (60 Hz)
            gyroInitialized = true;
        }
     }

    public static Quaternion GetGyro() {

         return HasGyroscope ? ReadGyroscopeRotation() : Quaternion.identity;
     }
    private static Quaternion ReadGyroscopeRotation() {
         return new Quaternion(0.5f, 0.5f, -0.5f, 0.5f) * Input.gyro.attitude * new Quaternion(0, 0, 1, 0);
     }
}
