using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CowboyBossCinematicSuperState : CowboyBossMasterState{
    public CowboyBossCinematicSuperState(CowboyBoss cowboyBoss, CowboyBossStateMachine cowboyBossStateMachine) : base(cowboyBoss, cowboyBossStateMachine){
    }


    public override void enter(){
        base.enter();
    }

    public override void Update(){
        base.Update();
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
