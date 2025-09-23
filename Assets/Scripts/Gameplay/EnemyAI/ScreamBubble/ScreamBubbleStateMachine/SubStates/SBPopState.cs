using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;

public class SBPopState : SBProvokedState
{
    public SBPopState(ScreamBubble screamBubble, ScreamBubbleStateMachine screamBubbleStateMachine) : base(screamBubble, screamBubbleStateMachine){

    }

    public override void enter(){
        screamBubble.popped = false;
        screamBubble.rb.linearVelocity = Vector3.zero;
        screamBubble.bubbleAudio.loop = false;
        screamBubble.bubbleAudio.clip = screamBubble.bubblePop;
        screamBubble.bubbleAudio.Play();
        screamBubble.bubbleRenderer.enabled = false;
        screamBubble.attackCollider.enabled = false;
        //screamBubble.rb.isKinematic = true;
        screamBubble.GetComponent<Collider>().enabled = false;
        foreach (GameObject obj in screamBubble.PhysicsObjects){
            obj.GetComponent<ParentConstraint>().constraintActive = false;
            obj.GetComponent<Rigidbody>().useGravity = true;
            obj.GetComponent<Collider>().enabled = true;
            obj.tag = "Friendly";
        }

        base.enter();
    }
    public override void Update(){

    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }


    void DetermineNextCoords(){
    }

    void PlayNewSound(){
    }
}
