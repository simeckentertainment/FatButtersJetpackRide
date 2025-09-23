using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CowboyBossSpawnMinionSuperState : CowboyBossMasterState{
    public CowboyBossSpawnMinionSuperState(CowboyBoss cowboyBoss, CowboyBossStateMachine cowboyBossStateMachine) : base(cowboyBoss, cowboyBossStateMachine){
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
