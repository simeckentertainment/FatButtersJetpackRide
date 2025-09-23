using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CowboyUpDieState : CBDeadState
{
    public CowboyUpDieState(Cowboy cowboy, CowboyStateMachine cowboyStateMachine) : base(cowboy, cowboyStateMachine){

    }

    public override void enter(){
        cowboy.anim.Play("TopHit");
        base.enter();
    }
    public override void Update()
    {

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
