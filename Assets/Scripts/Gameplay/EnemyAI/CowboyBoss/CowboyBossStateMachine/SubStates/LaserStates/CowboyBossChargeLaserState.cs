using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class CowboyBossChargeLaserState : CowboyBossFireLaserSuperState{
    public CowboyBossChargeLaserState(CowboyBoss cowboyBoss, CowboyBossStateMachine cowboyBossStateMachine) : base(cowboyBoss, cowboyBossStateMachine){
    }
    float TimeToSpendCharging;
    float timerCounter;
    float timerPercent;
    bool DiskIsPlaying;
    ParticleSystem.ColorOverLifetimeModule streakColor;
    Gradient streakGradient;
    GradientAlphaKey[] streakAlphaKeys;


    //2: Giant laser (big attack)
    public override void enter()
    {
        DiskIsPlaying = false;
        cowboyBoss.AimLaser.Play();
        cowboyBoss.StreakLaser.Play();
        cowboyBoss.vulnerable = true;
        timerCounter = 0f;
        TimeToSpendCharging = 300f;
        base.enter();
    }



    public override void Update(){
        base.Update();
    }

    public override void FixedUpdate(){
        timerCounter++;
        timerPercent = timerCounter/TimeToSpendCharging;
        //with 300 frames charging, every 20% is a second.


        if(timerPercent > 0.2f & !DiskIsPlaying){
            cowboyBoss.LaserDisk.Play();

        }
        if(timerCounter > TimeToSpendCharging){cowboyBoss.stateMachine.changeState(cowboyBoss.cowboyBossFireLaserState);}
        base.FixedUpdate();
    }

}
