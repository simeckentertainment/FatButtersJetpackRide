using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MrSudsStateMachine : MonoBehaviour{
    public MrSudsMasterState currentState;
    public void Initialize(MrSudsMasterState startState){
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
   public void changeState(MrSudsMasterState nextState){
        if(currentState != nextState){
            currentState.exit();
            currentState = nextState;
            nextState.enter();
        }
    }
}

