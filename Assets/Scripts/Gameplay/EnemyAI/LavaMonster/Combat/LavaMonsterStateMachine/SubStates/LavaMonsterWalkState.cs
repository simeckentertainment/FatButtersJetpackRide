using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaMonsterWalkState : LMMovingState
{
    public LavaMonsterWalkState(LavaMonster lavaMonster, LavaMonsterStateMachine lavaMonsterStateMachine) : base(lavaMonster, lavaMonsterStateMachine)
    {

    }



    public override void enter()
    {
        lavaMonster.anim.Play("FirstStep");
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
        if (GetAnimTime() < 0.2f)
        {
            SetNextMove();
        }
        if (GetAnimTime() > 0.95f)
        {
            if (lavaMonster.attackNow)
            {
                PickAttack();
            }
        }
        base.FixedUpdate();
    }
    void SetNextMove() {
        lavaMonster.attackNow = lavaMonster.CheckPlayerDistance() < lavaMonster.maximumAttackDistance ? true : false;
    }


    void PickAttack() { //The Up/Down Determinator is tied to the monster's chin.
        if(lavaMonster.CheckPlayerHeight() > lavaMonster.upDownDeterminator.position.y) {
            TransitionToSwipeAttack();
        } else {
            TransitionToFireballAttack();
        }
    }
    void TransitionToSwipeAttack() {
        //My first conditional. Yay!
        string animName = lavaMonster.anim.GetCurrentAnimatorStateInfo(0).IsName("LeftStep") ? "RightStepHighTrans" : "LeftStepHighTrans";
        lavaMonster.anim.Play(animName, 0, 0.0f);
        lavaMonster.stateMachine.changeState(lavaMonster.lavaMonsterHighSwipeState);
    }
    void TransitionToFireballAttack() {
        string animName = lavaMonster.anim.GetCurrentAnimatorStateInfo(0).IsName("LeftStep") ? "RightStepFireballTrans" : "LeftStepFireballTrans";
        lavaMonster.anim.Play(animName, 0, 0.0f);
        lavaMonster.stateMachine.changeState(lavaMonster.lavaMonsterFireballAttackState);
    }
}
