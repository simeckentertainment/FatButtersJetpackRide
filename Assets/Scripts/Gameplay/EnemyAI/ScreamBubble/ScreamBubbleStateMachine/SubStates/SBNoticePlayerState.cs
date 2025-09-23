using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SBNoticePlayerState : SBProvokedState
{
    public SBNoticePlayerState(ScreamBubble screamBubble, ScreamBubbleStateMachine screamBubbleStateMachine) : base(screamBubble, screamBubbleStateMachine){

    }
    int timer;
    int timerMax = 20;

    public override void enter(){
        PlayNewSound();
        base.enter();
    }
    public override void Update(){
        timer++;
        if(timer<timerMax*.5){
        screamBubble.rb.AddForce(Vector3.up*5.0f,ForceMode.Force);
        } else {
            screamBubble.rb.linearVelocity = Vector3.zero;
        }
        if(timer >= timerMax){
            screamBubble.stateMachine.changeState(screamBubble.sBChasePlayerState);
        }
        base.Update();
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }


    void DetermineNextCoords(){
    }

    void PlayNewSound(){
        screamBubble.bubbleAudio.Stop();
        screamBubble.bubbleAudio.clip = screamBubble.noticeSounds[Random.Range(0,screamBubble.noticeSounds.Length)];
        screamBubble.bubbleAudio.Play();
    }
}
