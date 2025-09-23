using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SegwayBearStateMachine : MonoBehaviour
{
    public SegwayBearState currentState;
    public void Initialize(SegwayBearState startState)
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
    public void changeState(SegwayBearState nextState)
    {
        currentState.exit();
        currentState = nextState;
        nextState.enter();
    }

}
