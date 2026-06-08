using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RumbaWithKnifeRollState : RumbaWithKnifeAliveState{
    public RumbaWithKnifeRollState(RumbaWithKnife rumba, RumbaWithKnifeStateMachine rumbaWithKnifeStateMachine) : base(rumba, rumbaWithKnifeStateMachine){
    }
    Vector3 startLoc;
    Vector3 goalLoc;
    RaycastHit leftHit;
    RaycastHit rightHit;

    bool anim1Complete;

    public override void enter()
    {
        rumba.ignoreLeft = rumba.ignoreRight = false;
        startLoc = rumba.transform.position;
        goalLoc = rumba.wanderGoalLoc;
        Debug.Log($"Start Location: {startLoc}, Goal Location: {goalLoc}");

        anim1Complete = false;
        PlayAnim("SlowStartDriveAnim");
        base.enter();
    }

    public override void Update(){
        base.Update();
    }

    public override void FixedUpdate(){

        if (!anim1Complete)
        {
            anim1Complete = anim1Runner();
        }
        MoveThisFrame();

        if( CheckForObstacles() || CheckForDestinationReached())
        {
            rumba.stateMachine.changeState(rumba.rumbaIdleState);
        }

        base.FixedUpdate();
    }

    bool anim1Runner()
    {
    if (CheckAnimName("SlowStartDriveAnim") && AnimFinished())
        {
            anim1Complete = true;
            PlayAnim("SlowDriveAnim");
            return true;
       } else
        {
            return false;
        }
    }
    bool CheckForObstacles()
    {
        if(rumba.wallDetected != RumbaWithKnife.Direction.None && rumba.wallDetected == rumba.direction) //if there's a wall in our way...
        {
            return true;
        }
        if(rumba.cliffDetected != RumbaWithKnife.Direction.None && rumba.cliffDetected == rumba.direction) //if there's a cliff in front of us...
        {
            return true;
        }

        //None of the above, we're good to go.
        return false;
    }
    bool CheckForDestinationReached()
    {
        return Vector3.Distance(rumba.transform.position, goalLoc) <= (rumba.WanderDistance * 0.1f);
    }




}
