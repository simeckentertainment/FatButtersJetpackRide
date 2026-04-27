using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuggerIdleState : DebuggerSuperState{
    public DebuggerIdleState(Debugger debugger, DebuggerStateMachine debuggerStateMachine) : base(debugger, debuggerStateMachine){
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
