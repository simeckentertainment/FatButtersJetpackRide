using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SeBIdleState : SeBUnprovokedState
{
    public SeBIdleState(SegwayBear segwayBear, SegwayBearStateMachine segwayBearStateMachine) : base(segwayBear, segwayBearStateMachine){
    }
    Vector3 targetCoords;
    Vector3 oldCoords;
    Vector3 movementVector;
    float LeftBoundary;
    float rightBoundary;
    float targetPos;
    Vector3 startPos;
    AudioClip currentClip;

    public override void enter(){
        DetermineNextCoords();
        segwayBear.SetDestination(targetPos,BearSpeed.fast);
        PlayNewSound();

        base.enter();
    }
    public override void Update(){

        if(Helper.isWithinMarginOfError(targetPos,segwayBear.transform.position.x,0.2f)){
            DetermineNextCoords();
            segwayBear.SetDestination(targetPos,BearSpeed.fast);
        }
        if(segwayBear.bearAudio.time > currentClip.length*0.98f){
            PlayNewSound();
        }
        if(segwayBear.detectedPlayer){
            segwayBear.stateMachine.changeState(segwayBear.seBNoticePlayerState);
        }
        base.Update();
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }


    void DetermineNextCoords(){
        LeftBoundary = segwayBear.boundaries[0].transform.position.x;
        rightBoundary = segwayBear.boundaries[1].transform.position.x;
        targetPos = Random.Range(LeftBoundary,rightBoundary);
        segwayBear.idleDestination = targetPos;

    }

    void PlayNewSound(){
        currentClip = segwayBear.BearSounds[Random.Range(0,2)];
        segwayBear.bearAudio.clip = currentClip;
        segwayBear.bearAudio.Play();
    }
}
