using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SBIdleState : SBUnprovokedState
{
    public SBIdleState(ScreamBubble screamBubble, ScreamBubbleStateMachine screamBubbleStateMachine) : base(screamBubble, screamBubbleStateMachine){

    }
    Vector3 targetCoords;
    Vector3 oldCoords;
    Vector3 movementVector;
    float wanderMaxRange = 10;
    Vector3 startPos;
    AudioClip currentClip;
    

    public override void enter()
    {
        PlayNewSound();

        startPos = screamBubble.transform.position;
        oldCoords = screamBubble.transform.position;

        DetermineNextCoords();
        base.enter();
    }
    public override void Update(){
        base.Update();
    }
    public override void FixedUpdate()
    {
        screamBubble.transform.forward = screamBubble.rb.linearVelocity.normalized;
        screamBubble.rb.AddForce((targetCoords-screamBubble.transform.position)*.25f,ForceMode.Force);
        if(Helper.isWithinMarginOfError(screamBubble.transform.position,targetCoords,1.0f)){
            oldCoords = targetCoords;
            DetermineNextCoords(); 
        }
        if(screamBubble.hitWall){
            oldCoords = targetCoords;
            DetermineNextCoords();
            screamBubble.hitWall = false;
        }
        if(screamBubble.bubbleAudio.time > currentClip.length*0.99f){
            PlayNewSound();
        }
        if (screamBubble.PlayerInSightDistance){
            RaycastHit LineOfSightChecker;
            if (Physics.Raycast(screamBubble.transform.position, (screamBubble.target.transform.position - screamBubble.transform.position).normalized, out LineOfSightChecker, Vector3.Distance(screamBubble.target.transform.position,screamBubble.transform.position))){
                Debug.DrawRay(screamBubble.transform.position, (screamBubble.target.transform.position - screamBubble.transform.position).normalized * Vector3.Distance(screamBubble.target.transform.position,screamBubble.transform.position), Color.yellow);
                if (LineOfSightChecker.collider.CompareTag("Player"))
                {
                    screamBubble.targetAcquired = true;
                }
            }
        }
        if (screamBubble.targetAcquired)
        {
            screamBubble.rb.linearVelocity = Vector3.zero;
            screamBubble.stateMachine.changeState(screamBubble.sBNoticePlayerState);
        }
        base.FixedUpdate();
    }


    void DetermineNextCoords(){
        ;
        float x = startPos.x + Random.Range(wanderMaxRange*-1,wanderMaxRange);
        float y = startPos.y + Random.Range(wanderMaxRange*-1,wanderMaxRange);
        Vector3 newCoords = new Vector3(x,y,0);
        movementVector = (newCoords-oldCoords).normalized;
        targetCoords = newCoords;

    }

    void PlayNewSound(){
        currentClip = screamBubble.idleSounds[Random.Range(0,screamBubble.idleSounds.Length)];
        screamBubble.bubbleAudio.clip = currentClip;
        screamBubble.bubbleAudio.Play();
    }
}
