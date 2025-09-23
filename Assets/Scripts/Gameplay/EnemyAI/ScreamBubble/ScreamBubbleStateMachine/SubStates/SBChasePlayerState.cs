using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SBChasePlayerState : SBProvokedState
{
public SBChasePlayerState(ScreamBubble screamBubble, ScreamBubbleStateMachine screamBubbleStateMachine) : base(screamBubble, screamBubbleStateMachine){

    }

    public override void enter(){
        PlayNewSound();

        base.enter();
    }
    public override void Update(){
        screamBubble.transform.forward = screamBubble.rb.linearVelocity.normalized;
        screamBubble.rb.AddForce((screamBubble.target.transform.position-screamBubble.transform.position)*10.0f,ForceMode.Force);
        }
    
    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }


    void DetermineNextCoords(){
    }

    void PlayNewSound(){
        screamBubble.bubbleAudio.Stop();
        screamBubble.bubbleAudio.clip = screamBubble.AttackSound;
        screamBubble.bubbleAudio.Play();
    }
}
