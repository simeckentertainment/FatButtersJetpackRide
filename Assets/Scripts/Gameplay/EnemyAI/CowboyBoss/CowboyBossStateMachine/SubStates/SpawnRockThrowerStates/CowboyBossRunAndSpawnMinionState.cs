using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CowboyBossRunAndSpawnMinionState : CowboyBossSpawnMinionSuperState{
    public CowboyBossRunAndSpawnMinionState(CowboyBoss cowboyBoss, CowboyBossStateMachine cowboyBossStateMachine) : base(cowboyBoss, cowboyBossStateMachine){
    }
    Vector3 startPos;
    List<Vector3> AimPoints;
    Vector3 endPos;
    float MoveSpeedMultiplier;
    float moveCounterThreshold;
    float moveCounter;
    //4: Spawn in three rock throwing cowboys along the ground while the boss hides. You need to kill the cowboys before the boss comes back out.
    public override void enter(){
        cowboyBoss.vulnerable = false;
        cowboyBoss.anim.Play("CBB.CBBRocketRideState");
        moveCounter = 0;
        MoveSpeedMultiplier = 1;
        startPos = cowboyBoss.transform.position;
        endPos = cowboyBoss.HideySpot.transform.position;
        moveCounterThreshold = Vector3.Distance(startPos,endPos)*MoveSpeedMultiplier;
        cowboyBoss.SpawnedCowboys = new GameObject[3]{
            MonoBehaviour.Instantiate(cowboyBoss.RockThrowingCowboy),
            MonoBehaviour.Instantiate(cowboyBoss.RockThrowingCowboy),
            MonoBehaviour.Instantiate(cowboyBoss.RockThrowingCowboy)
        };
        for(int i=0;i<cowboyBoss.RTCowboySpawns.Length;i++){
            cowboyBoss.SpawnedCowboys[i].transform.position = cowboyBoss.RTCowboySpawns[i].transform.position;
        }
        base.enter();
    }
    public override void Update(){
        base.Update();
    }

    public override void FixedUpdate(){
        moveCounter++;
        cowboyBoss.transform.LookAt(endPos);
        cowboyBoss.transform.position = Vector3.Lerp(startPos,endPos,moveCounter/moveCounterThreshold);
        if(moveCounter >= moveCounterThreshold){
            cowboyBoss.transform.position = endPos;
            cowboyBoss.stateMachine.changeState(cowboyBoss.cowboyBossHideAndWaitState);
        }
        base.FixedUpdate();
    }
}
