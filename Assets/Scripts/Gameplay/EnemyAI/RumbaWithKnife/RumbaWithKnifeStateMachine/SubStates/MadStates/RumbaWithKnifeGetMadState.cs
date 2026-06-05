using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RumbaWithKnifeGetMadState : RumbaWithKnifeAngryState{
    public RumbaWithKnifeGetMadState(RumbaWithKnife rumba, RumbaWithKnifeStateMachine rumbaWithKnifeStateMachine) : base(rumba, rumbaWithKnifeStateMachine){
    }

     public override void enter()
    {        PlayAnim("LookAroundAnim1");
        base.enter();
    }


    public override void Update(){
        base.Update();
    }

public override void FixedUpdate(){


if(CheckAnimName("NoticePlayerAnim") && AnimFinished())
        {
            rumba.stateMachine.changeState(rumba.rumbaMadIdleState);
        }
    base.FixedUpdate();
}



}
