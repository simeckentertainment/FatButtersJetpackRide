using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuggerSuperState : DebuggerMasterState{
    public DebuggerSuperState(Debugger debugger, DebuggerStateMachine debuggerStateMachine) : base(debugger, debuggerStateMachine){
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
