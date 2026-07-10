using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor.Build.Pipeline.Tasks;
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
    ParallelWorker NewTargetPW; //A Coroutine wrapper class.
    

    public override void enter()
    {
        PlayNewAnimWithSound();
        startPos = screamBubble.transform.position;
        oldCoords = screamBubble.transform.position;
        BeginHuntForNextTarget();
        base.enter();
    }

    private void BeginHuntForNextTarget()
    {
        NewTargetPW = ParallelWorker.StartParallelWorker(DetermineNextCoords());
    }

    public override void Update(){
        base.Update();
    }
    public override void FixedUpdate()
    {
        if (!NewTargetPW.done) //if we're still looking...
        {
            if(GetAnimNormalizedTime() >= 0.99f){
                PlayNewAnimWithSound();
            }
            screamBubble.rb.linearVelocity = Vector3.zero;
        }


        if (NewTargetPW.done || NewTargetPW == null) //If we're not actively looking for our next target, we're going there.
        {
            screamBubble.transform.forward = screamBubble.rb.linearVelocity.normalized;
            screamBubble.rb.AddForce((targetCoords-screamBubble.transform.position)*.25f,ForceMode.Force);
            if (Helper.isWithinMarginOfError(screamBubble.transform.position, targetCoords, 1.0f))
            {
                oldCoords = targetCoords;
                BeginHuntForNextTarget();
            }
            if(screamBubble.hitWall){
                oldCoords = targetCoords;
                BeginHuntForNextTarget();
                screamBubble.hitWall = false;
            }
            if(GetAnimNormalizedTime() >= 0.99f){
                PlayNewAnimWithSound();
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
        }
        base.FixedUpdate();
    
    }

    IEnumerator DetermineNextCoords()
    {
        while (true)
        {
            if (EnsureClearLineOfSightToNewTarget())
            {
                Vector3 newCoords = movementTargetPos;
                movementVector = (newCoords - oldCoords).normalized;
                targetCoords = newCoords;
                yield break; //We got it! Kill the loop!
            }
            yield return null; //It's okay buddy, try again till ya get it.
        }
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

    void PlayNewAnimWithSound(){
        int arrayIndex = Random.Range(1,screamBubble.idleSounds.Length);
        //We've picked an idle animation and cound combo now.
        PlayAnim("SBIdle" + arrayIndex);
        currentClip = screamBubble.idleSounds[arrayIndex-1];
        screamBubble.bubbleAudio.clip = currentClip;
        screamBubble.bubbleAudio.Play();
    }
    void PlayNewAnimWithSound(int arrayIndex){
        //We've picked an idle animation and cound combo now.
        PlayAnim("SBIdle" + arrayIndex);
        currentClip = screamBubble.idleSounds[arrayIndex-1];
        screamBubble.bubbleAudio.clip = currentClip;
        screamBubble.bubbleAudio.Play();
    }
}
