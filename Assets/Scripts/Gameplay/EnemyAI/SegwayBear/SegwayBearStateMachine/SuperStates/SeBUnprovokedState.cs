using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeBUnprovokedState : SegwayBearState
{
    public SeBUnprovokedState(SegwayBear segwayBear, SegwayBearStateMachine segwayBearStateMachine) : base(segwayBear, segwayBearStateMachine){

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
