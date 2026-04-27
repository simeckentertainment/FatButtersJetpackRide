using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debugger : MonoBehaviour{
    [System.NonSerialized] public DebuggerStateMachine stateMachine; //This gets set at start.
    public DebuggerIdleState debuggerIdleState { get; set; }
    // Start is called before the first frame update
    void Start(){
       stateMachine = GetComponent<DebuggerStateMachine>();
       debuggerIdleState = new DebuggerIdleState(this, stateMachine);
       stateMachine.Initialize(debuggerIdleState);
    }

    // Update is called once per frame
    void Update(){
    }
    private void OnCollisionEnter(Collision other) {
    }
}




