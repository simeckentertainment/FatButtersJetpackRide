using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallToyIdleState : BallToySuperState{
    public BallToyIdleState(BallToy ballToy, BallToyStateMachine ballToyStateMachine) : base(ballToy, ballToyStateMachine){
    }

    public override void enter(){
        base.enter();
    }
    public override void Update(){
        base.Update();
    }

    public override void FixedUpdate(){
        base.FixedUpdate();
    }
}
