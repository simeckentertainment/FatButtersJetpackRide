using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DebuggerMasterState{
    protected Debugger debugger;
    protected DebuggerStateMachine debuggerStateMachine;
    protected int durationOfState = 0;
    public DebuggerMasterState(Debugger debugger, DebuggerStateMachine debuggerStateMachine){
        this.debugger = debugger;
        this.debuggerStateMachine = debuggerStateMachine;
    }
    public virtual void enter(){
        durationOfState = 0;
    }
    public virtual void enterNoanimate(){
        durationOfState = 0;
    }
    // Start is called before the first frame update
    void Start(){
        
    }
    // Update is called once per frame
    public virtual void Update(){

    }
    public virtual void FixedUpdate(){
        durationOfState++;
    }
    public virtual void exit(){

    }
}
