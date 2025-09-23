using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CowboyBossReturnFromHidingState : CowboyBossSpawnMinionSuperState{
    public CowboyBossReturnFromHidingState(CowboyBoss cowboyBoss, CowboyBossStateMachine cowboyBossStateMachine) : base(cowboyBoss, cowboyBossStateMachine){
    }

    //4: Spawn in three rock throwing cowboys along the ground while the boss hides. You need to kill the cowboys before the boss comes back out.
    public override void enter(){
        cowboyBoss.vulnerable = true;
        foreach(GameObject c in cowboyBoss.SpawnedCowboys){
            MonoBehaviour.Destroy(c);
        }

        cowboyBoss.stateMachine.changeState(cowboyBoss.cowboyBossMoveState);
        base.enter();
    }
    public override void Update(){
        base.Update();
    }

    public override void FixedUpdate(){
        base.FixedUpdate();
    }
}
