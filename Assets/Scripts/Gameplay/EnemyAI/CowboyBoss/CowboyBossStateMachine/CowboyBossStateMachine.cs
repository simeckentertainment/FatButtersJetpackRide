using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CowboyBossStateMachine : MonoBehaviour{
    public CowboyBossMasterState currentState;
    public void Initialize(CowboyBossMasterState startState){
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
   public void changeState(CowboyBossMasterState nextState){
        if(currentState != nextState){
            currentState.exit();
            currentState = nextState;
            nextState.enter();
        }
    }
}

