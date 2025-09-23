using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CowboyBossRecoverFromLaserState : CowboyBossFireLaserSuperState{
    public CowboyBossRecoverFromLaserState(CowboyBoss cowboyBoss, CowboyBossStateMachine cowboyBossStateMachine) : base(cowboyBoss, cowboyBossStateMachine){
    }
    //2: Giant laser (big attack)
    float timerThreshold = 120f;
    float timerCounter;
    float timerPercent;
    ParticleSystemRenderer streakRenderer;
    Material streakMat;
    float initAlphaAmount;
    //Just hang there for 120 frames recovering.
    public override void enter(){
        timerCounter = 0;
        cowboyBoss.vulnerable = true;
        streakRenderer = cowboyBoss.StreakLaser.GetComponent<ParticleSystemRenderer>();
        streakMat = streakRenderer.material;
        initAlphaAmount = streakMat.color.a;
        
        base.enter();
    }
    public override void Update(){
        base.Update();
    }

    public override void FixedUpdate(){
        timerCounter++;
        timerPercent = timerCounter/timerThreshold;
        streakMat.color = new Color(1.0f,1.0f,1.0f,1.0f-timerPercent);
        if(timerCounter >= timerThreshold){
            streakMat.color = new Color(1.0f,1.0f,1.0f,initAlphaAmount);
            cowboyBoss.AimLaser.Clear();
            cowboyBoss.StreakLaser.Clear();
            cowboyBoss.LaserDisk.Clear();
            cowboyBoss.GrowLaser.Clear();
            cowboyBoss.stateMachine.changeState(cowboyBoss.cowboyBossMoveState);


            }

        //!!IMPORTANT!! THREE STEP PROCESS
        //1)Take this time to reduce the alpha on StreakLaser's material.
        //2)Stop the emitter.
        //3)kill all particles


        //4) On the Grow laser, do the same except just do it on entry.
        //Do that, and the laser should work correctly.
        base.FixedUpdate();
    }
}
