using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SeBNoticePlayerState : SeBProvokedState
{

    public SeBNoticePlayerState(SegwayBear segwayBear, SegwayBearStateMachine segwayBearStateMachine) : base(segwayBear, segwayBearStateMachine){

    }
    AudioClip currentClip;

    public override void enter(){

        PlayNewSound();
        base.enter();
    }
    public override void Update(){
        if(segwayBear.bearAudio.time > currentClip.length*0.98f){
            segwayBear.stateMachine.changeState(segwayBear.seBChargeLaserState);
        }
        base.Update();
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    void PlayNewSound(){
        currentClip = segwayBear.BearSounds[2];
        segwayBear.bearAudio.clip = currentClip;
        segwayBear.bearAudio.Play();
    }
}
