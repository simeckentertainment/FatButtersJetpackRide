using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaspberryIdleState : RaspberrySuperState{
    public RaspberryIdleState(Raspberry raspberry, RaspberryStateMachine raspberryStateMachine) : base(raspberry, raspberryStateMachine){
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
