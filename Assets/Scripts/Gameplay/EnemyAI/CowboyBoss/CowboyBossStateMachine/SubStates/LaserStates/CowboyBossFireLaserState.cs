using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CowboyBossFireLaserState : CowboyBossFireLaserSuperState{
    public CowboyBossFireLaserState(CowboyBoss cowboyBoss, CowboyBossStateMachine cowboyBossStateMachine) : base(cowboyBoss, cowboyBossStateMachine){
    }



    //2: Giant laser (big attack)
    float TimeToSpendFiring;
    float timerCounter;
    public override void enter(){
        cowboyBoss.GrowLaserHitbox.SetActive(true);
        cowboyBoss.GrowLaser.Play();
        cowboyBoss.vulnerable = true;
        timerCounter = 0f;
        TimeToSpendFiring = 200f;
        base.enter();
    }
    public override void Update(){
        base.Update();
    }

    public override void FixedUpdate(){
        timerCounter++;
        if(timerCounter > TimeToSpendFiring){cowboyBoss.stateMachine.changeState(cowboyBoss.cowboyBossRecoverFromLaserState);}
        base.FixedUpdate();
    }
    public override void exit(){
        cowboyBoss.GrowLaserHitbox.SetActive(false);
        cowboyBoss.GrowLaser.Stop();
        cowboyBoss.AimLaser.Stop();
        cowboyBoss.StreakLaser.Stop();
        cowboyBoss.LaserDisk.Stop();
        base.exit();
    }
}
