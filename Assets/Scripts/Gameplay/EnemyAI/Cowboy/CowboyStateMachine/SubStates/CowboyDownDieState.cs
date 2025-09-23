using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CowboyDownDieState : CBDeadState
{
    public CowboyDownDieState(Cowboy cowboy, CowboyStateMachine cowboyStateMachine) : base(cowboy, cowboyStateMachine){

    }

    public override void enter(){
        cowboy.anim.Play("MidHit");
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
