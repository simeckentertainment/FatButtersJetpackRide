using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RumbaWithKnifeIdleState : RumbaWithKnifeAliveState{
    public RumbaWithKnifeIdleState(RumbaWithKnife rumba, RumbaWithKnifeStateMachine rumbaWithKnifeStateMachine) : base(rumba, rumbaWithKnifeStateMachine){
    }

    public override void enter(){
        //Picks a direction to go in.
        //Regardless of the direction it picks, it will also pick a random direction to face.
        //If the direction it picks is not the same as the goal direction, it will spin around, with a 33% chance of just spinning in place.
        //If the direction it picks IS the same as the goal direction, it will just walk that way.
        //The Rumba may end up spinning for a while, and that's OK.
        PickDirection();//Then it picks a direction to go in.
        PickAction(); //chooses whether to turn or spin and sends us off to the appropriate state.

        
        base.enter();
    }
    public override void Update(){
        base.Update();
    }

    public override void FixedUpdate(){
        base.FixedUpdate();
    }

    void PickDirection()
    {

        Vector3 LeftWanderLoc = new Vector3(Mathf.Lerp(rumba.transform.position.x, rumba.wanderLeftMax.x, Random.Range(0.0f,1.0f)),rumba.transform.position.y, 0f);
        Vector3 RightWanderLoc = new Vector3(Mathf.Lerp(rumba.transform.position.x, rumba.wanderRightMax.x, Random.Range(0.0f,1.0f)),rumba.transform.position.y, 0f);

        //If we're only ignoring one direction, pick the other one!
        if(rumba.ignoreLeft || rumba.ignoreRight){
            //rumba.direction = rumba.ignoreLeft ? RumbaWithKnife.Direction.Right :RumbaWithKnife.Direction.Left;
            rumba.wanderGoalLoc = rumba.ignoreLeft ? RightWanderLoc : LeftWanderLoc;
        }
        //If we're ignoring neither direction, we have choices!
        if(Random.Range(0, 2) == 0)
        {
            rumba.wanderGoalLoc = new Vector3(Mathf.Lerp(rumba.transform.position.x, rumba.wanderLeftMax.x, Random.Range(0.0f,1.0f)),rumba.transform.position.y, 0f);
        } else {
            rumba.wanderGoalLoc = new Vector3(Mathf.Lerp(rumba.transform.position.x, rumba.wanderRightMax.x, Random.Range(0.0f,1.0f)),rumba.transform.position.y, 0f);
        }

        //Note: This completely ignores the logic above, but the end result SHOULD be that it sits there spinning and turning for an extra second or two which is funny.
        switch(Random.Range(0, 3))
        {
            case 0:
                rumba.direction = RumbaWithKnife.Direction.Left;
                break;
            case 1:
                rumba.direction = RumbaWithKnife.Direction.Right;
                break;
            default:
                rumba.direction = RumbaWithKnife.Direction.Spinning;
                break;
        }
    }
    void PickAction()
    {
        if(rumba.direction == RumbaWithKnife.Direction.Spinning)
        {
            rumba.stateMachine.changeState(rumba.rumbaSpinState);
        }
         else
        {
            rumba.stateMachine.changeState(rumba.rumbaTurnState);
        }
    }
}
