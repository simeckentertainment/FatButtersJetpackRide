using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RumbaWithKnifeIdleState : RumbaWithKnifeCalmState{
    public RumbaWithKnifeIdleState(RumbaWithKnife rumba, RumbaWithKnifeStateMachine rumbaWithKnifeStateMachine) : base(rumba, rumbaWithKnifeStateMachine){
    }
    Vector3 LeftMaxWanderLoc;
    Vector3 RightMaxWanderLoc;

    RaycastHit leftHit;
    RaycastHit rightHit;


    public override void enter(){
        Debug.Log("Idling!");
        //Picks a direction to go in.
        //Regardless of the direction it picks, it will also pick a random direction to face.
        //If the direction it picks is not the same as the goal direction, it will spin around, with a 33% chance of just spinning in place.
        //If the direction it picks IS the same as the goal direction, it will just walk that way.
        //The Rumba may end up spinning for a while, and that's OK.

        DetermineMaxWanderLocs(); //The Rumba figures out where it CAN go
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


    void DetermineMaxWanderLocs()
    {
        if(Physics.Raycast(rumba.transform.position, Vector3.left, out leftHit, rumba.WanderDistance))
        {
            Debug.Log("LeftHit: " + leftHit.point + "on " + leftHit.collider.name);
            LeftMaxWanderLoc = leftHit.point;
        }
        else
        {
            Debug.Log("No Left Hit. Setting LeftMaxWanderLoc to " + (rumba.spawnLoc.x - rumba.WanderDistance));
            LeftMaxWanderLoc = new Vector3(rumba.spawnLoc.x - rumba.WanderDistance, rumba.spawnLoc.y,0f);
        }

        if(Physics.Raycast(rumba.transform.position, Vector3.right, out rightHit, rumba.WanderDistance))
        {
            Debug.Log("RightHit: " + rightHit.point + "on " + rightHit.collider.name);
            RightMaxWanderLoc = rightHit.point;
        }
        else
        {
            Debug.Log("No Right Hit. Setting RightMaxWanderLoc to " + (rumba.spawnLoc.x + rumba.WanderDistance));
            RightMaxWanderLoc = new Vector3(rumba.spawnLoc.x + rumba.WanderDistance, rumba.spawnLoc.y,0f);
        }
    }

    void PickDirection()
    {
        int rand1 = Random.Range(0, 2);
        if(rand1 == 0)
        {
            rumba.wanderGoalLoc = LeftMaxWanderLoc;
        }
        else
        {
            rumba.wanderGoalLoc = RightMaxWanderLoc;
        }

        int rand2 = Random.Range(0, 3);
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
            rumba.stateMachine.changeState(rumba.rumbaSpinState);
        }
         else
        {
            rumba.stateMachine.changeState(rumba.rumbaTurnState);
        }
    }
}
