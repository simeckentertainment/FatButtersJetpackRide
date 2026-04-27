using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallToyStateMachine : MonoBehaviour{
    public BallToyMasterState currentState;
    public void Initialize(BallToyMasterState startState){
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
   public void changeState(BallToyMasterState nextState){
        if(currentState != nextState){
            currentState.exit();
            currentState = nextState;
            nextState.enter();
        }
    }
}

