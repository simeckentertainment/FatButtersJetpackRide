using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RumbaWithKnifeStateMachine : MonoBehaviour{
    public RumbaWithKnifeMasterState currentState;
    public void Initialize(RumbaWithKnifeMasterState startState){
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
   public void changeState(RumbaWithKnifeMasterState nextState){
        if(currentState != nextState){
            currentState.exit();
            currentState = nextState;
            nextState.enter();
        }
    }
}

