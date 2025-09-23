using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CowboyBossDeathState : CowboyBossSuperState{
    public CowboyBossDeathState(CowboyBoss cowboyBoss, CowboyBossStateMachine cowboyBossStateMachine) : base(cowboyBoss, cowboyBossStateMachine){
    }
    //This is the state where we are dead. Nothing happens here except allowing the player to move on.
    public override void enter(){
        cowboyBoss.vulnerable = false;
        cowboyBoss.anim.Play("CBB.CBBHitFallDead");
        //open the forcefield.
        cowboyBoss.RightForcefield.Triggered = true;
        base.enter();
    }
    public override void Update(){
        base.Update();
    }

    public override void FixedUpdate(){
        base.FixedUpdate();
    }
}
