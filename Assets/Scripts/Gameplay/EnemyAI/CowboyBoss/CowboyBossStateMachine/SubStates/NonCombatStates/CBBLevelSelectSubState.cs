using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CBBLevelSelectSubState : CowboyBossLevelSelectSuperState{
    public CBBLevelSelectSubState(CowboyBoss cowboyBoss, CowboyBossStateMachine cowboyBossStateMachine) : base(cowboyBoss, cowboyBossStateMachine){
    }
    public override void enter(){
        cowboyBoss.anim.runtimeAnimatorController = cowboyBoss.levelSelectAnimController;
        cowboyBoss.rocketFireParticles.Play();
        base.enter();
    }
    public override void Update(){

        base.Update();
    }

    public override void FixedUpdate(){
        base.FixedUpdate();
    }
}
