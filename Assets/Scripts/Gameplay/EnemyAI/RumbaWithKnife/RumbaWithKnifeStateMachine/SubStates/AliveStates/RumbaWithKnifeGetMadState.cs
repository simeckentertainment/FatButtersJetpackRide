using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RumbaWithKnifeGetMadState : RumbaWithKnifeAliveState{
    public RumbaWithKnifeGetMadState(RumbaWithKnife rumba, RumbaWithKnifeStateMachine rumbaWithKnifeStateMachine) : base(rumba, rumbaWithKnifeStateMachine){
    }

     public override void enter()
    {
        PlayAnim("LookAroundAnim1");
        rumba.SetAngerSpouts(true);
        rumba.rumbaMesh.material.EnableKeyword("_EMISSION");
        rumba.rumbaMesh.material.SetColor("_EmissionColor", Color.red);
        base.enter();
    }


    public override void Update(){
        base.Update();
    }

public override void FixedUpdate(){
    if(CheckAnimName("NoticePlayerAnim") && AnimFinished())
            {
                rumba.stateMachine.changeState(rumba.rumbaIdleState);
            }
        base.FixedUpdate();
    }

}
