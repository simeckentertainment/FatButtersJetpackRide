using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RumbaWithKnifeIdleState : RumbaWithKnifeSuperState{
    public RumbaWithKnifeIdleState(RumbaWithKnife rumba, RumbaWithKnifeStateMachine rumbaWithKnifeStateMachine) : base(rumba, rumbaWithKnifeStateMachine){
    }

    public override void enter(){
        PlayAnim("SpinAnim");
        base.enter();
    }
    public override void Update(){

        if (PlayerDetected())
        {
            //Change to player detected state.
        }
        base.Update();
    }

    public override void FixedUpdate(){
        base.FixedUpdate();
    }
}
