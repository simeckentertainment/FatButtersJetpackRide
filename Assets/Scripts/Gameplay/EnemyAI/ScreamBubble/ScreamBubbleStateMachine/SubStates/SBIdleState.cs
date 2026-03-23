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
    Vector3 movementTargetPos;
    

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


    void DetermineNextCoords()
    {
        bool lineOfSightClear = false;
        while (!lineOfSightClear)
        {
           lineOfSightClear = EnsureClearLineOfSightToNewTarget(); 
        }
        
        Vector3 newCoords = movementTargetPos;
        movementVector = (newCoords - oldCoords).normalized;
        targetCoords = newCoords;

    }

    private bool EnsureClearLineOfSightToNewTarget()
    {
        movementTargetPos = new Vector3(startPos.x + Random.Range(wanderMaxRange * -1, wanderMaxRange), startPos.y + Random.Range(wanderMaxRange * -1, wanderMaxRange), 0.0f);
        RaycastHit LineOfSightChecker;
        if (Physics.Raycast(screamBubble.transform.position, ((movementTargetPos - screamBubble.transform.position).normalized), out LineOfSightChecker, Vector3.Distance(movementTargetPos, screamBubble.transform.position)))
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    void PlayNewSound(){
        currentClip = screamBubble.idleSounds[Random.Range(0,screamBubble.idleSounds.Length)];
        screamBubble.bubbleAudio.clip = currentClip;
        screamBubble.bubbleAudio.Play();
    }
}
