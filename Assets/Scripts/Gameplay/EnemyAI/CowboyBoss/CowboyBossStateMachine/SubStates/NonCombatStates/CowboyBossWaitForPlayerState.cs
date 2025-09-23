using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CowboyBossWaitForPlayerState : CowboyBossCinematicSuperState{
    public CowboyBossWaitForPlayerState(CowboyBoss cowboyBoss, CowboyBossStateMachine cowboyBossStateMachine) : base(cowboyBoss, cowboyBossStateMachine){
    }

    public override void enter(){
        cowboyBoss.vulnerable = false;
        cowboyBoss.anim.runtimeAnimatorController = cowboyBoss.BattleAnimController;
        base.enter();
    }
    public override void Update(){
        base.Update();
    }

    public override void FixedUpdate(){
        if(cowboyBoss.BattleHasBegun){
            cowboyBoss.stateMachine.changeState(cowboyBoss.cowboyBossIntroCinematicState);
        }
        base.FixedUpdate();
    }
}
