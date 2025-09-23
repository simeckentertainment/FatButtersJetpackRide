using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaMonsterFallState : LMMovingState
{
    public LavaMonsterFallState(LavaMonster lavaMonster, LavaMonsterStateMachine lavaMonsterStateMachine) : base(lavaMonster, lavaMonsterStateMachine)
    {

    }

    public override void enter()
    {
        lavaMonster.anim.Play("FallToDeath");
        base.enter();
    }
    public override void Update()
    {

        base.Update();
    }


    private void RunAudio()
    {
    }



    public override void FixedUpdate(){
        if(GetAnimTime() > 0.95f)
        {
            MonoBehaviour.Destroy(lavaMonster.gameObject);
        }
        base.FixedUpdate();
    }

}
