using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaMonsterSpawnState : LMStillState
{
    public LavaMonsterSpawnState(LavaMonster lavaMonster, LavaMonsterStateMachine lavaMonsterStateMachine) : base(lavaMonster, lavaMonsterStateMachine){

    }



    public override void enter(){
        lavaMonster.anim.Play("RiseAndRoar");
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
        if(GetAnimTime() > 0.95f){
            lavaMonster.stateMachine.changeState(lavaMonster.lavaMonsterWalkState);
        }
        base.FixedUpdate();
    }


}
