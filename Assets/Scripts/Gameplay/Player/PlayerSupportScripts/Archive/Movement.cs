using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
#if UNITY_ANDROID
using GooglePlayGames;
using UnityEngine.SocialPlatforms;
#endif
*/
public class Movement : MonoBehaviour
{ 
    [Header("Base Data")]
    [SerializeField] CorgiEffectHolder FX;
    [SerializeField] CollectibleData collectibleData;
    public Rigidbody ButtersRigidbody;
    [SerializeField] public bool disableRotation;
    [SerializeField] public bool disableThrust;
    public float thrust;
    public float baseThrust;
    public float rotThrust = 2.0f;
    int index;
    bool lowGravMode;

//get all objects for audio

    // Start is called before the first frame update
    void Start()
    {

        index = collectibleData.CurrentSkin;

        //old skin-setting code.
        //for(int i=0;i<FX.Skins.Length;i++){
            //if(i == index){
                //FX.Skins[i].SetActive(true);
            //} else {
                //FX.Skins[i].SetActive(false);
            //}
        //}

        thrust = baseThrust + (collectibleData.thrustUpgradeLevel*2);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        thrust = baseThrust + collectibleData.thrustUpgradeLevel;
        if(lowGravMode){
            ButtersRigidbody.AddForce(Physics.gravity*-0.5f);
        }
    }

    public void ApplyThrust(){
        if(!disableThrust){
            ButtersRigidbody.AddRelativeForce(0,thrust,0);
            /*
            #if UNITY_ANDROID
            if(PlayGamesPlatform.Instance.IsAuthenticated()){
                Social.ReportProgress("CgkI293vto8EEAIQAQ", 100.0f, (bool success) => {
                    if(success){
                        Debug.Log("Achievement success!");
                    }else {
                        Debug.LogWarning("Achievement failure!");
                    }
                    // handle success or failure
                    });
            } else {
                Debug.Log("Not authenticated");
            }
            #endif
                    */
        }
    }
    public void applyRot(float rotatThisFrame){ //Turning and the turn particle effect is handled here.

        if (rotatThisFrame == 0){
            FX.StopAllRotParticles();
        }else{
            ActivateRotParticles(rotatThisFrame);
            transform.Rotate(Vector3.forward * rotatThisFrame * Time.deltaTime * 60);
            //ButtersRigidbody.freezeRotation = false; //unfreezing those same rotations so the physics system can take over.
        }
    }
    public void applyRot(float rotateTo,bool isGyro){
        transform.rotation = Quaternion.Euler(new Vector3(0,0,rotateTo));
    }
    public void ActivateRotParticles(float rotatThisFrame){
        if (rotatThisFrame > 0){
            //FX.StartMinusRotParticles(index);
        }
        else if (rotatThisFrame < 0){
            //FX.StartPlusRotParticles(index);
        }
    }


    //Audio processor for the rocket.
    //Audio actually turns on and off when at 0, depending on
    //whether the rocket is thrusting or not.
    //Otherwise it increases and decreases up until it hits a certain
    //threshold. We use both left and right rockets for effect. It's pretty sweet.
    //Values set are based on the left rocket. It's the driver.
    public void rocketEffectChange(bool toPlay){
        if (toPlay == true){
            FX.StartPrimaryThrusters();
            //FX.StartRocketSounds(index);
            //if(FX.GetThrusterVolume(index) == 0.0f){
                //FX.SetThrusterVolume(index,FX.startVolume);
            //}
            //if (FX.GetThrusterAudioStatus(index) && FX.GetThrusterVolume(index) < FX.maxVolume){
                //FX.SetThrusterVolume(index, FX.GetThrusterVolume(index)+FX.volumeSpeed * Time.deltaTime);
                //FX.SetThrusterLightIntensity((FX.GetThrusterVolume(index) * FX.RocketLightMult) / 6.0f);
            //}
        } else {
            FX.StopPrimaryThrusters();
            FX.TurnOffThrusterLights();
            //FX.SetThrusterLightIntensity((FX.GetThrusterVolume(index) * FX.RocketLightMult) / 6.0f);
            //if(FX.GetThrusterAudioStatus(index) && FX.GetThrusterVolume(index) > FX.startVolume){
                //FX.SetThrusterVolume(index,FX.GetThrusterVolume(index)-FX.volumeSpeed * Time.deltaTime);
            //} else if (FX.GetThrusterAudioStatus(index) && FX.GetThrusterVolume(index) <= FX.startVolume){
                FX.StopRocketSounds();
            //}
        }
    }

    public void EnableLowGravMode(){
        lowGravMode = true;
    }
    public void DisableLowGravMode(){
        lowGravMode = false;
    }
}