using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CowboyBossGoToTauntPosState : CowboyBossCinematicSuperState{
    public CowboyBossGoToTauntPosState(CowboyBoss cowboyBoss, CowboyBossStateMachine cowboyBossStateMachine) : base(cowboyBoss, cowboyBossStateMachine){
    }
    Vector3 startPos;
    List<Vector3> AimPoints;
    Vector3 endPos;
    float MoveSpeedMultiplier;
    float moveCounterThreshold;
    float moveCounter;
    //This state exists to bring the boss to the laughing position. If he's already there, skip.
    public override void enter(){
        cowboyBoss.vulnerable = true;
        AimPoints = new List<Vector3>();






        MoveSpeedMultiplier = 1;
        startPos = cowboyBoss.transform.position;
        endPos = cowboyBoss.LaughingSpotLoc;
        moveCounter = 0;
        moveCounterThreshold = Vector3.Distance(startPos,endPos) * MoveSpeedMultiplier;
        if(startPos == endPos){ 
            cowboyBoss.stateMachine.changeState(cowboyBoss.cowboyBossCrazyLaughState);
        } else {
            cowboyBoss.anim.Play("CBB.CBBRocketRideState");
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
            cowboyBoss.transform.rotation = cowboyBoss.LaughingSpotRot;
            cowboyBoss.stateMachine.changeState(cowboyBoss.cowboyBossCrazyLaughState);
        }
        base.FixedUpdate();
    }
}
