using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LMStillState : LavaMonsterState
{
    public LMStillState(LavaMonster lavaMonster, LavaMonsterStateMachine lavaMonsterStateMachine) : base(lavaMonster, lavaMonsterStateMachine){

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
