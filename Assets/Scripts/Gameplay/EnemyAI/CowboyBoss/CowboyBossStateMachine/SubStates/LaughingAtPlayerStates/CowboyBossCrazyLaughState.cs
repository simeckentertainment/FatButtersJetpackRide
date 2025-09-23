using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CowboyBossCrazyLaughState : CowboyBossSuperState{
    public CowboyBossCrazyLaughState(CowboyBoss cowboyBoss, CowboyBossStateMachine cowboyBossStateMachine) : base(cowboyBoss, cowboyBossStateMachine){
    }



    //This is a decision-making state. We don't actually spend any time here.
    //We've got a few possible attacks to decide on:
    //1: Pick a random collision cube and go there.
    //2: Giant laser (big attack)
    //3: Stand on the boss platform and laugh while the rocket goes crazy.
    //4: Spawn in three rock throwing cowboys along the ground while the boss hides. You need to kill the cowboys before the boss comes back out.
    public override void enter(){
        cowboyBoss.vulnerable = true;
        cowboyBoss.anim.Play("CBB.CBBLaughAtPlayer");
        base.enter();
    }
    public override void Update(){
        base.Update();
    }

    public override void FixedUpdate(){
        if(cowboyBoss.LaughingRocket){
            RunRocket();
        }
        if(cowboyBoss.anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.985f){
            cowboyBoss.stateMachine.changeState(cowboyBoss.cowboyBossMoveState);
        }
        base.FixedUpdate();
    }
    void RunRocket(){

    }
}
