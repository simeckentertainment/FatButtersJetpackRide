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
    bool ignoreLeft;
    bool ignoreRight;


    public override void enter(){
        //Picks a direction to go in.
        //Regardless of the direction it picks, it will also pick a random direction to face.
        //If the direction it picks is not the same as the goal direction, it will spin around, with a 33% chance of just spinning in place.
        //If the direction it picks IS the same as the goal direction, it will just walk that way.
        //The Rumba may end up spinning for a while, and that's OK.
        base.enter(); //This belongs first for calibration purposes.
        ignoreLeft = false;
        ignoreRight = false;
        DetermineMaxWanderLocs(); //The Rumba figures out where it CAN go
        if(!ignoreLeft || !ignoreRight)
        {
            PickDirection();//Then it picks a direction to go in.
            PickAction(); //chooses whether to turn or spin and sends us off to the appropriate state.
        } else
        {
            rumba.stateMachine.changeState(rumba.rumbaSoftlockState);
        }
        
        
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
            Debug.Log(leftHit.collider.gameObject.name);
            Debug.Log(Vector3.Distance(rumba.transform.position, leftHit.point));


            if(!Physics.Raycast(rumba.LeftCastPosObj.position, Vector3.down, out leftHit, 2.0f)){ignoreLeft = true;}

            if(!Physics.Raycast(rumba.RightCastPosObj.position, Vector3.down, out rightHit, 2.0f)){ignoreRight = true;}



            if(Vector3.Distance(rumba.transform.position, leftHit.point) < rumba.wallDistanceTrigger)
            {
                ignoreLeft = true;
            } else
            {
                ignoreLeft = false;
            }

            LeftMaxWanderLoc = new Vector3(leftHit.point.x, rumba.transform.position.y, 0f);
        }
        else
        {
            LeftMaxWanderLoc = new Vector3(rumba.transform.position.x - rumba.WanderDistance, rumba.transform.position.y,0f);
        }

        if(Physics.Raycast(rumba.transform.position, Vector3.right, out rightHit, rumba.WanderDistance))
        {
            Debug.Log(rightHit.collider.gameObject.name);
            Debug.Log(Vector3.Distance(rumba.transform.position, rightHit.point));
            if(Vector3.Distance(rumba.transform.position, rightHit.point) < rumba.wallDistanceTrigger)
            {
                ignoreRight = true;
            } else
            {
                ignoreRight = false;
            }
            RightMaxWanderLoc = new Vector3(rightHit.point.x, rumba.transform.position.y, 0f);
        }
        else
        {
            RightMaxWanderLoc = new Vector3(rumba.transform.position.x + rumba.WanderDistance, rumba.transform.position.y,0f);
        }

        Debug.Log("left: " + ignoreLeft + " | right: " + ignoreRight);
    }

    void PickDirection()
    { // I don't LIKE this code, but it works.

        int rand1 = Random.Range(0, 2);
        if(rand1 == 0 && !ignoreLeft)
        {

            rumba.wanderGoalLoc = new Vector3(Mathf.Lerp(rumba.transform.position.x, LeftMaxWanderLoc.x, Random.Range(0.0f,1.0f)),rumba.transform.position.y, 0f);
        }
        
        if (rand1 != 0 && !ignoreRight)
        {
            rumba.wanderGoalLoc = new Vector3(Mathf.Lerp(rumba.transform.position.x, RightMaxWanderLoc.x, Random.Range(0.0f,1.0f)),rumba.transform.position.y, 0f);
        }

        int rand2 = Random.Range(0, 3);
        if(rand2 == 0 && !ignoreLeft)
        {
            rumba.direction = RumbaWithKnife.Direction.Left;
        }
        else if(rand2 == 1 && !ignoreRight)
        {
            rumba.direction = RumbaWithKnife.Direction.Right;
        }
         else //50% chance of just spinning for no reason.
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
