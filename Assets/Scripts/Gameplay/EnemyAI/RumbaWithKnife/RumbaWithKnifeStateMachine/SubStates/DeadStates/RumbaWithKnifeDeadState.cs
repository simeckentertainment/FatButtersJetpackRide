using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RumbaWithKnifeDeadState : RumbaWithKnifeDeathState{
    public RumbaWithKnifeDeadState(RumbaWithKnife rumba, RumbaWithKnifeStateMachine rumbaWithKnifeStateMachine) : base(rumba, rumbaWithKnifeStateMachine){
    }


    public override void enter(){
        SetWanderLocs();
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
    void SetWanderLocs()
    {
    }
}
