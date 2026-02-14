using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    public PlayerState currentState;
    public void Initialize(PlayerState startState)
    {
        currentState = startState;
        currentState.enter();
    }


    // Update is called once per frame
    public void Update()
    {
        currentState.Update();
    }
    public void FixedUpdate()
    {
        
        currentState.FixedUpdate();
    }
    public void changeState(PlayerState nextState)
    {
        if(currentState != nextState){
            currentState.exit();
            currentState = nextState;
            nextState.enter();
        }
    }

}
