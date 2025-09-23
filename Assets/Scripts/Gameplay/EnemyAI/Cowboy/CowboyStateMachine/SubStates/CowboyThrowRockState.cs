using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CowboyThrowRockState : CBAliveState
{
    public CowboyThrowRockState(Cowboy cowboy, CowboyStateMachine cowboyStateMachine) : base(cowboy, cowboyStateMachine){

    }

    bool rockThrown;

    public override void enter(){
        cowboy.anim.Play("ThrowRock");
        rockThrown =false;
        base.enter();
    }
    public override void Update()
    {
        if(Helper.isWithinMarginOfError(cowboy.anim.GetCurrentAnimatorStateInfo(0).normalizedTime % 1,0.1f,0.02f)){
            rockThrown = false;
        }
        if(Helper.isWithinMarginOfError(cowboy.anim.GetCurrentAnimatorStateInfo(0).normalizedTime % 1,0.5f,0.02f) & rockThrown == false){
            //Experiencing a bug where the rock doesn't hurt when player isn't moving, so let's move the target point down a bit.
            Vector3 adjustedPos = cowboy.player.transform.position;

            cowboy.rockSpawner.ThrowRock(adjustedPos);
            rockThrown = true;
        }  

        RunAudio();
        if(cowboy.playerDist > cowboy.aiTriggerDistance){
            cowboy.stateMachine.changeState(cowboy.cowboyIdleState);
        }
        base.Update();
    }


    private void RunAudio()
    {
    }



    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }


}
