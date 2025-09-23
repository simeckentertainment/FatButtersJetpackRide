using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;


public class CollisionHandler : MonoBehaviour
{
    [SerializeField] CorgiEffectHolder FX;
    [SerializeField] Rigidbody rb;
    [SerializeField] CollectibleData collectibleData;
    [SerializeField] SceneLoadData sceneLoadData;
    [SerializeField] DataHandler dataHandler;
    bool isTransitioning = false;
    bool collisionDisabler = false;
    bool crashLightHandler = false;
    [SerializeField] Movement movement;
    public bool FriendlyCollision;
    public bool HurtingCollision;



    void FixedUpdate(){
        //if (crashLightHandler){
            //if (FX.explosionLight.intensity > 0){
                //FX.explosionLight.intensity -= Time.deltaTime * 10;
            //}
        //}
    }
    #region triggerStuff
    private void OnTriggerEnter(Collider other) {
        enterTrigger(other);
    }
    private void OnTriggerExit(Collider other){
        exitTrigger(other);
    }
    public void enterTrigger(Collider other){
        if(isTransitioning || collisionDisabler){return;}
        else {
            switch (other.gameObject.tag){
                case "Fuel":
                    fuelHandler(other.gameObject);
                    break;
                case "Food":
                    foodHandler(other.gameObject);
                    break;
                case "Bone":
                    boneHandler(other.gameObject);
                    break;
                case "Ball":
                    ballhandler(other.gameObject);
                    break;
                case "Finish":
                    if (!dataHandler.isDead) { 
                        startSuccessSequence(sceneLoadData.LastLoadedLevelInt);
                    }
                    break;
                case "LowGravArea":
                    movement.EnableLowGravMode();
                    break;
                case "Harmful":
                    HurtingCollision = true;
                    break;
                default:
                    break;

            }
        }
    }
    public void exitTrigger(Collider other){
        if(isTransitioning || collisionDisabler){
            return;
        } else {
            switch (other.gameObject.tag){
                case "Friendly":
                    FriendlyCollision = false;
                    break;
                case "Finish":
                    break;
                case "Player":
                    break;
                case "EnemyWeakspot":
                    break;
                case "LowGravArea":
                    movement.DisableLowGravMode();
                    break;
                case "Harmful":
                    HurtingCollision = false;
                    break;
                default:
                    break;
            }
        }
    }
    #endregion
    #region colliderStuff
    private void OnCollisionEnter(Collision other) {
        enterCollider(other);
    }
    
    private void OnCollisionExit(Collision other) {
        exitCollider(other);
    }
    private void OnParticleCollision(GameObject other) {
        if(!collectibleData.GameplayTestingMode){
            RunHurt(10,other.transform.position);
            HurtingCollision = true;
                    }
    }
    public void enterCollider(Collision other){
        //Debug.Log(other.gameObject);
        if(isTransitioning || collisionDisabler){
            return;
        } else {
            switch (other.gameObject.tag){
                case "Friendly":
                    FriendlyCollision = true;
                    break;
                case "Finish":
                    startSuccessSequence(sceneLoadData.LastLoadedLevelInt);
                    break;
                case "Player":
                    break;
                case "EnemyWeakspot":
                    break;
                case "Harmful":
                    if(!collectibleData.GameplayTestingMode){
                        RunHurt(10,other.collider.ClosestPoint(transform.position));
                        HurtingCollision = true;
                    }
                    break;
                case "OneHitKill":
                    OneHitKill();
                    break;
                default:
                    if(!collectibleData.GameplayTestingMode){
                        FriendlyCollision = true;
                    }
                    break;
            }
        }
    }

    public void exitCollider(Collision other){
        if(isTransitioning || collisionDisabler){
            return;
        } else {
            switch (other.gameObject.tag){
                case "Friendly":
                    FriendlyCollision = false;
                    break;
                case "Finish":
                    break;
                case "Player":
                    break;
                case "EnemyWeakspot":
                    break;
                default:
                    if(!collectibleData.GameplayTestingMode){
                        HurtingCollision = false;
                    }
                    break;
            }
        }
    }
#endregion

    private void startSuccessSequence(int completedLevel)
    {
        //Stop all effects and controls.
        FX.StopRocketSounds();
        FX.StopPrimaryThrusters();
        FX.StopAllRotParticles();
        movement.disableRotation = true;
        movement.disableThrust = true;
        //play the complete level effects
        //FX.corgiAudio.PlayOneShot(FX.successSound,0.5f);
        //FX.successParticles.Play();
        //backend marking
        isTransitioning = true;
        dataHandler.MarkLevelComplete(completedLevel);
        dataHandler.levelComplete = true; //This marks the level as complete.
    }


    public void StartCrashSequence(){
        //FX.corgiAudio.PlayOneShot(FX.explosionSound,0.1f);
        //FX.explosionParticles.Play();
        //FX.explosionLight.enabled = true;
        crashLightHandler = true;
        hideCorgi();
        isTransitioning = true;
    }
    void fuelHandler(GameObject fuelObject){
        dataHandler.ChangeFuelAmount(fuelObject.GetComponent<PowerupRotator>().increaseFuel);
        Destroy(fuelObject);

    }
    void foodHandler(GameObject foodObject){

        dataHandler.ChangeTummyAmount(foodObject.GetComponent<PowerupRotator>().increaseTreats);
        Destroy(foodObject);
    }
    void ballhandler (GameObject ballObject){
        dataHandler.hasTemporaryBall = true;
        collectibleData.HASBALL = true;
        Destroy(ballObject);
    }
    void boneHandler(GameObject boneObject){

        dataHandler.AddBone();
        Destroy(boneObject);
    }
    void hideCorgi(){
        FX.StopAllRotParticles();
        FX.StopPrimaryThrusters();
        FX.StopRocketSounds();
        FX.HideCorgi();
    }

    void RunHurt(int damageAmount,Vector3 collisionPoint){
        dataHandler.ChangeTummyAmount(damageAmount*-1);
        rb.AddExplosionForce(50.0f,collisionPoint,10.0f,10.0f,ForceMode.Force);
    }
    void OneHitKill()
    {
        dataHandler.OneHitKill = true;
        dataHandler.ChangeTummyAmount(dataHandler.maxTummy*-1);
    }

}
