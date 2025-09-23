using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KFIgnoreState : KissyFishState
{
    public KFIgnoreState(KissyFish kissyFish, KissyFishStateMachine kissyFishStateMachine) : base(kissyFish, kissyFishStateMachine){

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
