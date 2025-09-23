using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CowboyBossHitState : CowboyBossSuperState{
    public CowboyBossHitState(CowboyBoss cowboyBoss, CowboyBossStateMachine cowboyBossStateMachine) : base(cowboyBoss, cowboyBossStateMachine){
    }
    RaycastHit floorChecker;
    RaycastHit ceilingChecker;
    Vector3 startPos;
    Vector3 ceilingPoint;
    Vector3 eulerRot;
    Vector3 GoalRot = new Vector3(0f, -90f, 0f);

    public override void enter(){
        cowboyBoss.vulnerable = false;
        eulerRot = cowboyBoss.transform.rotation.eulerAngles;
        startPos = cowboyBoss.transform.position;
        cowboyBoss.Health -=1;
        cowboyBoss.vulnerable = false;
        floorChecker = new RaycastHit();
        SetEmission(true);

        //remove all particles from the laser, just in case.
        cowboyBoss.AimLaser.Stop();
        cowboyBoss.AimLaser.Clear();
        cowboyBoss.StreakLaser.Stop();
        cowboyBoss.StreakLaser.Clear();
        cowboyBoss.LaserDisk.Stop();
        cowboyBoss.LaserDisk.Clear();
        cowboyBoss.GrowLaser.Stop();
        cowboyBoss.GrowLaser.Clear();


        if(Physics.Raycast(cowboyBoss.transform.position,Vector3.up,out floorChecker,1000.0f)){
            ceilingPoint = ceilingChecker.point;
        }
        cowboyBoss.anim.Play("CBB.CBBHitInit");
        base.enter();
    }
    public override void Update(){
        base.Update();
    }

    public override void FixedUpdate(){
        cowboyBoss.transform.rotation = Quaternion.Euler(Vector3.Lerp(eulerRot,GoalRot,GetAnimTime()));
        //if(cowboyBoss.transform.position.y < ceilingPoint.y-1.0f){
            cowboyBoss.transform.position = new Vector3(cowboyBoss.transform.position.x,cowboyBoss.transform.position.y+0.1f,cowboyBoss.transform.position.z);
        //} else {
            //cowboyBoss.stateMachine.changeState(cowboyBoss.cowboyBossHitFallState);
        //}
        if(ReachedGoalAnimTime(0.95f)){
            SetEmission(false);
            cowboyBoss.stateMachine.changeState(cowboyBoss.cowboyBossHitFallState);
        }
        base.FixedUpdate();
    }


    void SetEmission(bool whatDo){
            foreach(SkinnedMeshRenderer s in cowboyBoss.cowboyParts){
            Material m = s.material;
            if(whatDo){ m.EnableKeyword("_EMISSION");} else {m.DisableKeyword("_EMISSION");}
        }
    }


    
}
