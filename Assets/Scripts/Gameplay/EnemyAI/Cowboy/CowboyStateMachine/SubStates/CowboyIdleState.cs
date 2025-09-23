using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CowboyIdleState : CBAliveState
{
    public CowboyIdleState(Cowboy cowboy, CowboyStateMachine cowboyStateMachine) : base(cowboy, cowboyStateMachine){

    }



    public override void enter(){
        cowboy.anim.Play("Idle");
        base.enter();
    }
    public override void Update()
    {
        if(cowboy.playerDist < cowboy.aiTriggerDistance){
            cowboy.stateMachine.changeState(cowboy.cowboyChaseState);
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
