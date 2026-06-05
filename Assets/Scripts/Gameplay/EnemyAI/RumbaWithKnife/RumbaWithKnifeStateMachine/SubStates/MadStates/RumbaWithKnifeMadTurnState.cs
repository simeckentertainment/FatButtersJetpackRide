using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RumbaWithKnifeMadTurnState : RumbaWithKnifeAngryState{
    public RumbaWithKnifeMadTurnState(RumbaWithKnife rumba, RumbaWithKnifeStateMachine rumbaWithKnifeStateMachine) : base(rumba, rumbaWithKnifeStateMachine){
    }
    RumbaWithKnife.Direction intendedDirection;
    RumbaWithKnife.Direction actualDirection;
    float fromVal;
    float toVal;
    bool timerRunning;
    float timer;

     public override void enter()
    {
        PlayAnim("AngryDriveAnim");
        SetIntendedDirection();
        DetermineActualDirection();
        if(intendedDirection == actualDirection)
        {
            //If the Rumba is already facing the direction it wants to go in, it doesn't need to turn. It can just start rolling.
            rumba.stateMachine.changeState(rumba.rumbaMadRollState);
            return;
        }
        timer = 0;
        SetToFromVals();
        base.enter();
    }


void SetIntendedDirection()
    {
        if(rumba.transform.position.x > rumba.wanderGoalLoc.x)
        {
            intendedDirection = RumbaWithKnife.Direction.Left;
        } else
        {
            intendedDirection = RumbaWithKnife.Direction.Right;
        }
    }
    private void SetToFromVals()
    {
        if (intendedDirection == RumbaWithKnife.Direction.Left)
        {
            fromVal = rumba.transform.rotation.eulerAngles.y;
            toVal = rumba.leftFacingRot;
        }
        else
        {
            fromVal = rumba.transform.rotation.eulerAngles.y;
            toVal = rumba.rightFacingRot;
        }
        timerRunning = !Helper.isWithinMarginOfError(rumba.transform.rotation.eulerAngles.y, toVal, 1.0f); //
    }

    public override void Update(){
        base.Update();
    }

public override void FixedUpdate(){
    if (timerRunning)
    {
        float timerPercent = timer / rumba.angryTurnFrameCountMax;
        float rotVal = Mathf.LerpAngle(fromVal, toVal, timerPercent);
        SetRumbaRotation(rotVal);
        timer++;
        if (timerPercent >= 1f || Mathf.Abs(Mathf.DeltaAngle(rumba.transform.rotation.eulerAngles.y, toVal)) < 1f)
        {
            timerRunning = false;
        }
    }
    else
    {
        rumba.stateMachine.changeState(rumba.rumbaMadRollState);
    }
    base.FixedUpdate();
}

    void DetermineActualDirection()
    {
        if(rumba.transform.forward.x < 0)
            actualDirection = RumbaWithKnife.Direction.Left;
        else
            actualDirection = RumbaWithKnife.Direction.Right;
    }


}
