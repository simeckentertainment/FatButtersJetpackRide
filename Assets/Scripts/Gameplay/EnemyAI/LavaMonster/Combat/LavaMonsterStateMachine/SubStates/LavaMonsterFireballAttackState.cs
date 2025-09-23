using FireballMovement;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class LavaMonsterFireballAttackState : LMStillState
{
    bool fireballLaunched;
    GameObject Fireball;
    public LavaMonsterFireballAttackState(LavaMonster lavaMonster, LavaMonsterStateMachine lavaMonsterStateMachine) : base(lavaMonster, lavaMonsterStateMachine){

    }



    public override void enter(){
        fireballLaunched = false;
        Fireball = lavaMonster.FireballModel;
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

        if(time >= 0.67 & lavaMonster.anim.GetCurrentAnimatorStateInfo(0).IsName("FireballAttack") & !fireballLaunched)
        {
            fireballLaunched = true;
            Fireball.transform.position = lavaMonster.upDownDeterminator.transform.position;
            MonoBehaviour.Instantiate(Fireball);
            
        }
        if (time >= 0.95f & lavaMonster.anim.GetCurrentAnimatorStateInfo(0).IsName("FireballAttack"))
        {
            lavaMonster.attackNow = false;
            lavaMonster.stateMachine.changeState(lavaMonster.lavaMonsterWalkState);
        }
        base.FixedUpdate();
    }


}
