using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CowboyBossMoveState : CowboyBossCinematicSuperState{
    public CowboyBossMoveState(CowboyBoss cowboyBoss, CowboyBossStateMachine cowboyBossStateMachine) : base(cowboyBoss, cowboyBossStateMachine){
    }
    Vector3 startPos;
    Vector3 endPos;
    float MoveSpeedMultiplier;
    float moveCounterThreshold;
    float moveCounter;
    public override void enter(){
        cowboyBoss.vulnerable = true;
        cowboyBoss.anim.Play("CBB.CBBRocketRideState");
        moveCounter = 0;
        MoveSpeedMultiplier = 2;
        startPos = cowboyBoss.transform.position;
        endPos = cowboyBoss.RocketAimPoints[Random.Range(0,cowboyBoss.RocketAimPoints.Length-1)].transform.position;
        moveCounterThreshold = Vector3.Distance(startPos,endPos)*MoveSpeedMultiplier;

        base.enter();
    }
    public override void Update(){

        base.Update();
    }

    public override void FixedUpdate(){
        Debug.DrawRay(startPos, Vector3.down*1000.0f, Color.red);
        moveCounter++;
        cowboyBoss.transform.LookAt(endPos);
        cowboyBoss.transform.position = Vector3.Slerp(startPos,endPos,moveCounter/moveCounterThreshold);
        if(moveCounter >= moveCounterThreshold){
            cowboyBoss.transform.position = endPos;
            cowboyBoss.transform.rotation = cowboyBoss.LaughingSpotRot;
            cowboyBoss.stateMachine.changeState(cowboyBoss.cowboyBossIdleState);
        }
        base.FixedUpdate();
    }
}
