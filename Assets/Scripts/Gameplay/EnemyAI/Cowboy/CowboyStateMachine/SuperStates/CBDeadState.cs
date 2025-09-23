using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CBDeadState : CowboyState
{
    public CBDeadState(Cowboy cowboy, CowboyStateMachine cowboyStateMachine) : base(cowboy, cowboyStateMachine){

    }
    public override void enter(){
        
        base.enter();
    }

    public override void Update(){
        base.Update();
    }
    public override void FixedUpdate()
    {
        if(cowboy.anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f){cowboy.isDead = true;}
        base.FixedUpdate();
    }
}
