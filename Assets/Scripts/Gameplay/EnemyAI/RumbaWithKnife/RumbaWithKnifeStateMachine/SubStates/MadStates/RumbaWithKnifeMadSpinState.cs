using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RumbaWithKnifeMadSpinState : RumbaWithKnifeAngryState{
    public RumbaWithKnifeMadSpinState(RumbaWithKnife rumba, RumbaWithKnifeStateMachine rumbaWithKnifeStateMachine) : base(rumba, rumbaWithKnifeStateMachine){
    }

     public override void enter()
    {
        PlayAnim("AngrySpinAnim");
        base.enter();
    }

    public override void Update(){
        base.Update();
    }

    public override void FixedUpdate(){
        if(CheckAnimName("AngrySpinAnim") && AnimFinished())
        {
            rumba.stateMachine.changeState(rumba.rumbaMadTurnState);
        }
        base.FixedUpdate();
    }

}
