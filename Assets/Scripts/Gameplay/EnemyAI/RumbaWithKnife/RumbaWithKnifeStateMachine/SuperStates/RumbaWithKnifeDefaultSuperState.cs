using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RumbaWithKnifeSuperState : RumbaWithKnifeMasterState{
    public RumbaWithKnifeSuperState(RumbaWithKnife rumbaWithKnife, RumbaWithKnifeStateMachine rumbaWithKnifeStateMachine) : base(rumbaWithKnife, rumbaWithKnifeStateMachine){
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
