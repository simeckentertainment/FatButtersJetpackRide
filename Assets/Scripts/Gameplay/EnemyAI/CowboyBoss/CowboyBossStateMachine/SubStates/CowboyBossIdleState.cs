using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CowboyBossIdleState : CowboyBossSuperState{
    public CowboyBossIdleState(CowboyBoss cowboyBoss, CowboyBossStateMachine cowboyBossStateMachine) : base(cowboyBoss, cowboyBossStateMachine){
    }
    //This is a decision-making state. We don't actually spend any time here.
    //We've got a few possible things to decide on
    //1: (move) Pick a random goal cube and go there.
    //2: (attack) Giant laser
    //3: (attack) Spawn in three rock throwing cowboys along the ground while the boss hides. You need to kill the cowboys before the boss comes back out.
    //4: (attack) Stand on the boss platform and laugh while the rocket goes crazy.
    //5: (opening) Stand on the boss platform and laugh at the player WITHOUT the rocket going crazy.
    //6: (opening) Stand in the middle of the arena on his rocket, taunting the player.
    public override void enter(){
        Debug.Log("Idle state!");
        int choicePicked = PickChoice() ; //PickChoice()   //Change to a number for dev testing.
        switch(choicePicked){
            case 1:
                //Move to a random goal cube. //done.
                cowboyBoss.stateMachine.changeState(cowboyBoss.cowboyBossMoveState);
                break;
            case 2:
                //Attack with giant laser. //done
                cowboyBoss.stateMachine.changeState(cowboyBoss.cowboyBossPrepToFireLaserState);
                break;
            case 3:
                //Spawn in rock throwers. //done.
                cowboyBoss.stateMachine.changeState(cowboyBoss.cowboyBossRunAndSpawnMinionState);
                break;
            case 4:
                //Laugh with crazy rocket. //Laugh done, rocket not.
                cowboyBoss.LaughingRocket = true;
                cowboyBoss.stateMachine.changeState(cowboyBoss.cowboyBossMoveToLaughingPositionState);
                break;
            case 5:
                //Laugh no rocket. // done
                cowboyBoss.LaughingRocket = false;
                cowboyBoss.stateMachine.changeState(cowboyBoss.cowboyBossMoveToLaughingPositionState);
                break;
            case 6:
                //Taunt. Might get rid of this state. Currently inaccessible.
                cowboyBoss.stateMachine.changeState(cowboyBoss.cowboyBossGoToTauntPosState);
                break;
            default:
                //If the system breaks down, just go to the move state.
                cowboyBoss.stateMachine.changeState(cowboyBoss.cowboyBossMoveState);
                break;
        }
        base.enter();
    }
    public override void Update(){
        base.Update();
    }

    public override void FixedUpdate(){
        base.FixedUpdate();
    }

    int PickChoice(){
        //50% chance to move, 30% chance to attack, 20% chance to leave an opening.
        int categoryPicker = Random.Range(0,100);
        if(categoryPicker <= 50){
            //move
            return 1;
        } else if (categoryPicker > 50 & categoryPicker <= 80){
            //attack
            // 25% chance of giant laser
            // 45% chance of crazy rocket
            // 30% chance of cowboy spawning
            int attackPicker = Random.Range(0,100);

            if(attackPicker <= 25){return 2;} 
            else if (attackPicker > 25 & attackPicker <= 70){return 3;} 
            else {return 4;}
        } else {
            //leave opening. 50/50 shot betwixt the two.
            //int openingPicker = Random.Range(0,100);
            //if(openingPicker <=50){
                return 5;
            //} else {
                //return 6;
            //}
        }
    }
}
