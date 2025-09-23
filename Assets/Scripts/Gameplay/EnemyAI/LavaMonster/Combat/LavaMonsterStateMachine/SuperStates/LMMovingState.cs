using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LMMovingState : LavaMonsterState
{
    public LMMovingState(LavaMonster lavaMonster, LavaMonsterStateMachine lavaMonsterStateMachine) : base(lavaMonster, lavaMonsterStateMachine){

    }
    public override void enter(){
        base.enter();
    }

    public override void Update(){
        base.Update();
    }
    public override void FixedUpdate()
    {
        lavaMonster.transform.Translate(lavaMonster.WalkSpeed, 0, 0);
        base.FixedUpdate();
    }
}
