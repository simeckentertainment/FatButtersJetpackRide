using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class LoadingScreengyro : MonoBehaviour
{
    [SerializeField]public bool touched;
    int touchCount;

    [Header("Gyro stuff")]
    private static bool gyroInitialized = false;
    public static bool HasGyroscope {get {return SystemInfo.supportsGyroscope;}}
    Quaternion referenceRotation = Quaternion.identity;
    Quaternion deviceRotation;
    Vector3 limitedRotation;
    [SerializeField] ParticleSystem[] rockets;
    [SerializeField] Transform rooJoint;
    // Start is called before the first frame update
    void Start()
    {
        gyroInitialized = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gyroInitialized) {
             InitGyro();
         }
        if (HasGyroscope){
            limitedRotation = new Vector3(0.0f,0.0f,Input.gyro.gravity.x * 45.0f);
            transform.rotation = Quaternion.Euler(limitedRotation);
        }
        touchCount = Input.touchCount;
        if (touchCount > 0){
            touched = true;
        } else {
            touched = false;
        }
        if(touched){
            foreach (ParticleSystem rocket in rockets){
                rocket.Play();
            }
        } else {
            foreach (ParticleSystem rocket in rockets){
                rocket.Stop();
            }
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
