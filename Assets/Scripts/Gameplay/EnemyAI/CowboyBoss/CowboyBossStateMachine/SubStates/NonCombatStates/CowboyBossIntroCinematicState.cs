using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Cinemachine;
public class CowboyBossIntroCinematicState : CowboyBossCinematicSuperState{
    public CowboyBossIntroCinematicState(CowboyBoss cowboyBoss, CowboyBossStateMachine cowboyBossStateMachine) : base(cowboyBoss, cowboyBossStateMachine){
    }
    float CameraMoveCounter;
    float CameraMoveThreshold = 100f;
    Transform CameraGoalPos;
    Vector3 originalCameraWorldSpacePos;
    Quaternion originalCameraRot;
    bool animationRun;
    public override void enter(){
        cowboyBoss.vulnerable = false;
        cowboyBoss.LeftForcefield.Triggered = true;
        animationRun = false;
        CameraMoveCounter = 0f;
        CameraGoalPos = GameObject.Find("BossBattleCameraPos").transform;
        cowboyBoss.OriginalLocalCamPos = cowboyBoss.cam.transform.localPosition;
        cowboyBoss.OriginalCameraAngle = cowboyBoss.cam.transform.rotation;
        originalCameraWorldSpacePos = cowboyBoss.cam.transform.position;
        //disable the cinemaMachine to get our angle.
        cowboyBoss.cam.GetComponent<CinemachineBrain>().enabled = false;
        base.enter();
    }
    public override void Update(){
        base.Update();
    }

    public override void FixedUpdate(){
        if(CameraMoveCounter < CameraMoveThreshold){
            CameraMoveCounter++;
            float currentTime = CameraMoveCounter/CameraMoveThreshold;
            cowboyBoss.cam.transform.position = Vector3.Lerp(originalCameraWorldSpacePos,CameraGoalPos.position,currentTime);
            cowboyBoss.cam.transform.rotation = Quaternion.Lerp(cowboyBoss.OriginalCameraAngle, CameraGoalPos.rotation,currentTime);
        } else {
            if(!animationRun){
            cowboyBoss.anim.Play("CBB.CBBIntroCinematicState",0);
            animationRun = true;
            }
        }


        base.FixedUpdate();
    }
}
