using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuggerStateMachine : MonoBehaviour{
    public DebuggerMasterState currentState;
    public void Initialize(DebuggerMasterState startState){
        currentState = startState;
        currentState.enter();
}

// Update is called once per frame
    public void Update(){
        currentState.Update();
    }
    public void FixedUpdate(){
        currentState.FixedUpdate();
    }
   public void changeState(DebuggerMasterState nextState){
        if(currentState != nextState){
            currentState.exit();
            currentState = nextState;
            nextState.enter();
        }
    }
}

