using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RumbaWithKnifeRollState : RumbaWithKnifeCalmState{
    public RumbaWithKnifeRollState(RumbaWithKnife rumba, RumbaWithKnifeStateMachine rumbaWithKnifeStateMachine) : base(rumba, rumbaWithKnifeStateMachine){
    }
    Vector3 startLoc;
    Vector3 goalLoc;
    float DistToGoal;

    bool anim1Complete;

    public override void enter()
    {
        Debug.Log("Rolling!");

        startLoc = rumba.transform.position;
        goalLoc = rumba.wanderGoalLoc;
        Debug.Log($"Start Location: {startLoc}, Goal Location: {goalLoc}");
        SetDistToGoal();
        anim1Complete = false;
        PlayAnim("SlowStartDriveAnim");
        base.enter();
    }

    private void SetDistToGoal()
    {
        DistToGoal = Vector3.Distance(rumba.transform.position, goalLoc);
    }

    public override void Update(){
        base.Update();
    }

    public override void FixedUpdate(){

        if (!anim1Complete)
        {
            anim1Complete = anim1Runner();
        }
        MoveToSpotForThisFrame();
        SetDistToGoal();
        if(Vector3.Distance(rumba.transform.position, startLoc) >= rumba.WanderDistance * 0.9f)
        {
            rumba.stateMachine.changeState(rumba.rumbaIdleState);
        }

        base.FixedUpdate();
    }


    bool anim1Runner()
    {
    if (rumba.anim.GetCurrentAnimatorStateInfo(0).IsName("SlowStartDriveAnim") && rumba.anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            PlayAnim("SlowDriveAnim");
            return true;
       } else
        {
            return false;
        }
    }

    void MoveToSpotForThisFrame()
    {
        Vector3 newPos = rumba.transform.position + rumba.transform.forward * rumba.calmSpeed * Time.fixedDeltaTime;
        newPos.z = 0f;
        rumba.transform.position = newPos;
    }



}
