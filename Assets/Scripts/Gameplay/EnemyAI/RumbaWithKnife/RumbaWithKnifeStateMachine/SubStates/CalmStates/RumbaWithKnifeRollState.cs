using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RumbaWithKnifeRollState : RumbaWithKnifeCalmState{
    public RumbaWithKnifeRollState(RumbaWithKnife rumba, RumbaWithKnifeStateMachine rumbaWithKnifeStateMachine) : base(rumba, rumbaWithKnifeStateMachine){
    }
    Vector3 startLoc;
    Vector3 goalLoc;
    RaycastHit leftHit;
    RaycastHit rightHit;

    bool anim1Complete;

    public override void enter()
    {

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
        MoveToSpotForThisFrame(rumba.calmSpeed);

        if(!Physics.Raycast(rumba.LeftCastPosObj.position, Vector3.down, out rightHit, 2.0f) || !Physics.Raycast(rumba.RightCastPosObj.position, Vector3.down, out leftHit, 2.0f) )
        {
            rumba.stateMachine.changeState(rumba.rumbaIdleState);
        }


        if(Vector3.Distance(rumba.transform.position, goalLoc) < rumba.WanderDistance * 0.1f)
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





}
