using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CowboyBossHitLiveState : CowboyBossSuperState{
    public CowboyBossHitLiveState(CowboyBoss cowboyBoss, CowboyBossStateMachine cowboyBossStateMachine) : base(cowboyBoss, cowboyBossStateMachine){
    }
    public override void enter(){
        cowboyBoss.vulnerable = false;
        cowboyBoss.anim.Play("CBB.CBBHitFallAlive");
        base.enter();
    }
    public override void Update(){
        base.Update();
    }

    public override void FixedUpdate(){
        if(ReachedGoalAnimTime(0.95f)){
            cowboyBoss.stateMachine.changeState(cowboyBoss.cowboyBossMoveState);
        }
        base.FixedUpdate();
    }
}
