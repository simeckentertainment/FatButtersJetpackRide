using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RumbaWithKnifeMadRollState : RumbaWithKnifeAngryState{
    public RumbaWithKnifeMadRollState(RumbaWithKnife rumba, RumbaWithKnifeStateMachine rumbaWithKnifeStateMachine) : base(rumba, rumbaWithKnifeStateMachine){
    }
    Vector3 startLoc;
    Vector3 goalLoc;

    bool anim1Complete;

    public override void enter()
    {

        startLoc = rumba.transform.position;
        goalLoc = rumba.wanderGoalLoc;
        Debug.Log($"Start Location: {startLoc}, Goal Location: {goalLoc}");
  
        anim1Complete = false;
        PlayAnim("AngryStartDriveAnim");
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
        MoveToSpotForThisFrame(rumba.madSpeed);

        if(Vector3.Distance(rumba.transform.position, startLoc) >= rumba.WanderDistance * 0.9f)
        {
            rumba.stateMachine.changeState(rumba.rumbaMadIdleState);
        }

        base.FixedUpdate();
    }

    bool anim1Runner()
    {
    if (CheckAnimName("MadStartDriveAnim") && AnimFinished())
        {
            PlayAnim("AngryDriveAnim");
            return true;
       } else
        {
            return false;
        }
    }

}
