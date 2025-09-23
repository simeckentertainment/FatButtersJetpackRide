using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CowboyBossHitFallState : CowboyBossSuperState{
    public CowboyBossHitFallState(CowboyBoss cowboyBoss, CowboyBossStateMachine cowboyBossStateMachine) : base(cowboyBoss, cowboyBossStateMachine){
    }
    RaycastHit floorChecker;
    Vector3 startPos;
    Vector3 fallPoint;
    float fallThreshold;
    float fallCounter;

    public override void enter(){
        cowboyBoss.vulnerable = false;
        fallCounter = 0;
        startPos = cowboyBoss.transform.position;
        floorChecker = new RaycastHit();
        if(Physics.Raycast(cowboyBoss.transform.position,Vector3.down,out floorChecker,1000.0f)){
            fallPoint = floorChecker.point;
        }
        fallThreshold = Vector3.Distance(startPos,fallPoint)*2.0f;
        cowboyBoss.anim.Play("CBB.CBBHitFallLoop");
        base.enter();
    }
    public override void Update(){
        base.Update();
    }

    public override void FixedUpdate(){
        fallCounter++;
        Debug.DrawRay(startPos, Vector3.down*1000.0f, Color.red);
        cowboyBoss.transform.position = Vector3.Lerp(startPos, fallPoint,fallCounter/fallThreshold);
        if(fallCounter >= fallThreshold){
            if(cowboyBoss.Health>0){
                cowboyBoss.stateMachine.changeState(cowboyBoss.cowboyBossHitLiveState);
            } else {
                cowboyBoss.stateMachine.changeState(cowboyBoss.cowboyBossDeathState);
            }

        }
        base.FixedUpdate();
    }
}
