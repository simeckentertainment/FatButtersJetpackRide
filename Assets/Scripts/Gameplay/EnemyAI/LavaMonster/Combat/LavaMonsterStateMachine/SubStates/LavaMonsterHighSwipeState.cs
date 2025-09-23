using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaMonsterHighSwipeState : LMStillState
{
    public LavaMonsterHighSwipeState(LavaMonster lavaMonster, LavaMonsterStateMachine lavaMonsterStateMachine) : base(lavaMonster, lavaMonsterStateMachine){

    }



    public override void enter(){
        base.enter();
    }
    public override void Update()
    {

        base.Update();
    }


    private void RunAudio()
    {
    }



    public override void FixedUpdate()
    {
        float time = lavaMonster.anim.GetCurrentAnimatorStateInfo(0).normalizedTime;
        if (time >= 0.95f & lavaMonster.anim.GetCurrentAnimatorStateInfo(0).IsName("HighSwipeAttack"))
        {
            lavaMonster.attackNow = false;
            lavaMonster.stateMachine.changeState(lavaMonster.lavaMonsterWalkState);
        }
        base.FixedUpdate();
    }


}
