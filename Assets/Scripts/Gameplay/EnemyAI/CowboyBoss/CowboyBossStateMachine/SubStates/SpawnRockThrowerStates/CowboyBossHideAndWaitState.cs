using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CowboyBossHideAndWaitState : CowboyBossSpawnMinionSuperState{
    public CowboyBossHideAndWaitState(CowboyBoss cowboyBoss, CowboyBossStateMachine cowboyBossStateMachine) : base(cowboyBoss, cowboyBossStateMachine){
    }

    bool cowboy1;
    bool cowboy2;
    bool cowboy3;
    Cowboy[] cowboys;
    //Stays out of the room and waits patiently for the player to kill the minions.
    public override void enter(){
        cowboyBoss.vulnerable = false;
        cowboy1 = true;
        cowboy2 = true;
        cowboy3 = true;
        cowboys = new Cowboy[3] {
            cowboyBoss.SpawnedCowboys[0].GetComponent<Cowboy>(),
            cowboyBoss.SpawnedCowboys[0].GetComponent<Cowboy>(),
            cowboyBoss.SpawnedCowboys[0].GetComponent<Cowboy>()
        };
        base.enter();
    }
    public override void Update(){
        base.Update();
    }

    public override void FixedUpdate(){
        if(cowboy1){
            if(CheckForDeath(cowboys[0])){cowboy1 = false;};
        }
        if(cowboy2){
            if(CheckForDeath(cowboys[1])){cowboy2 = false;}
        }
        if(cowboy3){
            if(CheckForDeath(cowboys[2])){cowboy3 = false;}
        }
        if(!cowboy1 & !cowboy2 & !cowboy3){
            cowboyBoss.stateMachine.changeState(cowboyBoss.cowboyBossReturnFromHidingState);
        }
        base.FixedUpdate();
    }

    bool CheckForDeath(Cowboy cowboy){
        if(cowboy.isDead){
            return true;
        } else {
            return false;
        }
    }
}
