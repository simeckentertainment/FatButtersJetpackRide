using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.InputSystem.OnScreen;
using System.Drawing;

public class LoadingScreenCorgiManager : MonoBehaviour
{
    [Header("Sprite stuff")]
    [SerializeField] Sprite[] dogsprites;
    [SerializeField] public UnityEngine.UI.Image image;
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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(FilterTouchInput()){
            image.sprite = dogsprites[1];
        } else {
            image.sprite = dogsprites[0];
        }
        TrackRollData(); //always be checking the roll data.
        transform.rotation = Quaternion.Euler(0.0f,0.0f,roll*-1);
        touchThrust = FilterTouchInput(); //Setting the touch thrust, filtering out other control methods.

    }

    private bool FilterTouchInput(){
        if(Input.touchCount == 0){return false;} //if untouched
        if(Time.timeScale == 0.0f){return false;} //if paused
        return true;
    }
    private void TrackRollData(){
        if (!HasGyroscope){
            //roll = 0.0f;
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
