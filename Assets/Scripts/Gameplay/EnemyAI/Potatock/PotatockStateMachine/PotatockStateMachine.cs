using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotatockStateMachine : MonoBehaviour{
    public PotatockMasterState currentState;
    public void Initialize(PotatockMasterState startState){
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
   public void changeState(PotatockMasterState nextState){
        if(currentState != nextState){
            currentState.exit();
            currentState = nextState;
            nextState.enter();
        }
    }
}

