using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RumbaWithKnifeAliveState : RumbaWithKnifeMasterState{
    public RumbaWithKnifeAliveState(RumbaWithKnife rumba, RumbaWithKnifeStateMachine rumbaWithKnifeStateMachine) : base(rumba, rumbaWithKnifeStateMachine){
    }


    public override void enter(){
        base.enter();
    }

    public override void Update(){
        base.Update();
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
