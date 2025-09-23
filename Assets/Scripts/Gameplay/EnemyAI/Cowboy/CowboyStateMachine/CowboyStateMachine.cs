using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CowboyStateMachine : MonoBehaviour
{
    public CowboyState currentState;
    public void Initialize(CowboyState startState)
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
    public void changeState(CowboyState nextState)
    {
        if(currentState != nextState){
            currentState.exit();
            currentState = nextState;
            nextState.enter();
        }
    }

}
