using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CowboyBossPrepToFireLaserState : CowboyBossFireLaserSuperState{
    public CowboyBossPrepToFireLaserState(CowboyBoss cowboyBoss, CowboyBossStateMachine cowboyBossStateMachine) : base(cowboyBoss, cowboyBossStateMachine){
    }
    Vector3 startPos;
    float MoveSpeedMultiplier;
    float moveCounterThreshold;
    float moveCounter;
    bool Part1Complete; //moving to position.
    bool Part2InProgress;
    bool AimLaserRunning;
    Vector3 GoalPos = new Vector3(175f,10f,0f);
    //2: Giant laser (big attack)
    public override void enter(){
        AimLaserRunning = false;
        cowboyBoss.vulnerable = true;
        moveCounter = 0;
        Part1Complete = false;
        Part2InProgress = false;
        startPos = cowboyBoss.transform.position;
        cowboyBoss.anim.Play("CBB.CBBRocketRideState");
        MoveSpeedMultiplier = 1f;
        moveCounterThreshold = Vector3.Distance(startPos, GoalPos) * MoveSpeedMultiplier;
        base.enter();
    }
    public override void Update(){
        base.Update();
    }

    public override void FixedUpdate(){
        if(!Part1Complete){RunPart1();}
        if(Part1Complete & !Part2InProgress){DoPartTransition();}
        if (Part2InProgress){RunPart2();}
        if(CheckAnimName("CBB.CBBPrepLaserState") & ReachedGoalAnimTime(0.75f) & !AimLaserRunning){
            AimLaserRunning = true;
        }
        base.FixedUpdate();
    }
    void RunPart1(){
        moveCounter++;
        cowboyBoss.transform.LookAt(GoalPos);
        cowboyBoss.transform.position = Vector3.Lerp(startPos,GoalPos,moveCounter/moveCounterThreshold);
        if(moveCounter >= moveCounterThreshold){
            cowboyBoss.transform.position = GoalPos;
            cowboyBoss.transform.rotation = cowboyBoss.LaughingSpotRot;
            Part1Complete = true;
        }
    }
    private void DoPartTransition(){
        cowboyBoss.anim.Play("CBB.CBBPrepLaserState");
        Part2InProgress = true;
    }
    void RunPart2(){
        if(ReachedGoalAnimTime(0.99f) & CheckAnimName("CBB.CBBPrepLaserState")){
            cowboyBoss.stateMachine.changeState(cowboyBoss.cowboyBossChargeLaserState);
        } 
    }
}
