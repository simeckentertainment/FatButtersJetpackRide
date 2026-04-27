using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MrSudsIdleState : MrSudsSuperState{
    public MrSudsIdleState(MrSuds mrSuds, MrSudsStateMachine mrSudsStateMachine) : base(mrSuds, mrSudsStateMachine){
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
