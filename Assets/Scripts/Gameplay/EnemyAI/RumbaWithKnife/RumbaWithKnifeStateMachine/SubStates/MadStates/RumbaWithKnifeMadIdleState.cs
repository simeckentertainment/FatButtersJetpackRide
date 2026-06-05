using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RumbaWithKnifeMadIdleState : RumbaWithKnifeAngryState{
    public RumbaWithKnifeMadIdleState(RumbaWithKnife rumba, RumbaWithKnifeStateMachine rumbaWithKnifeStateMachine) : base(rumba, rumbaWithKnifeStateMachine){
    }
    Vector3 LeftMaxWanderLoc;
    Vector3 RightMaxWanderLoc;

    RaycastHit leftHit;
    RaycastHit rightHit;
    bool ignoreLeft;
    bool ignoreRight;

    public override void enter(){
        //Picks a direction to go in.
        //Regardless of the direction it picks, it will also pick a random direction to face.
        //If the direction it picks is not the same as the goal direction, it will spin around, with a 33% chance of just spinning in place.
        //If the direction it picks IS the same as the goal direction, it will just walk that way.
        //The Rumba may end up spinning for a while, and that's OK.
        base.enter(); //This should be first for Raycast calibration purposes.
        DetermineMaxWanderLocs(); //The Rumba figures out where it CAN go
        PickDirection();//Then it picks a direction to go in.
        PickAction(); //chooses whether to turn or spin and sends us off to the appropriate state.

    }
    public override void Update(){
        base.Update();
    }

    public override void FixedUpdate(){
        base.FixedUpdate();
    }


    void DetermineMaxWanderLocs()
    {
        if(Physics.Raycast(rumba.LeftCastPosObj.position, Vector3.left, out leftHit, rumba.WanderDistance))
        {
            LeftMaxWanderLoc = leftHit.point;
        }
        else
        {
            LeftMaxWanderLoc = new Vector3(rumba.spawnLoc.x - rumba.WanderDistance, rumba.spawnLoc.y,0f);
        }

        if(Physics.Raycast(rumba.transform.position, Vector3.right, out rightHit, rumba.WanderDistance))
        {
            RightMaxWanderLoc = rightHit.point;
        }
        else
        {
            RightMaxWanderLoc = new Vector3(rumba.spawnLoc.x + rumba.WanderDistance, rumba.spawnLoc.y,0f);
        }
    }

    void PickDirection()
    {
        int rand1 = Random.Range(0, 2);
        if(rand1 == 0)
        {
            rumba.wanderGoalLoc = new Vector3(Mathf.Lerp(rumba.transform.position.x, LeftMaxWanderLoc.x, Random.Range(0.0f,1.0f)),rumba.transform.position.y, 0f);
        }
        else
        {
            rumba.wanderGoalLoc = new Vector3(Mathf.Lerp(rumba.transform.position.x, RightMaxWanderLoc.x, Random.Range(0.0f,1.0f)),rumba.transform.position.y, 0f);
        }

        int rand2 = Random.Range(0, 5);
        if(rand2 == 0)
        {
            rumba.direction = RumbaWithKnife.Direction.Left;
        }
        else if(rand2 == 1)
        {
            rumba.direction = RumbaWithKnife.Direction.Right;
        }
        else
        {
            rumba.direction = RumbaWithKnife.Direction.Spinning;
        }
    }
    void PickAction()
    {
        if(rumba.direction == RumbaWithKnife.Direction.Spinning)
        {
            rumba.stateMachine.changeState(rumba.rumbaMadSpinState);
        }
         else
        {
            rumba.stateMachine.changeState(rumba.rumbaMadTurnState);
        }
    }
}
