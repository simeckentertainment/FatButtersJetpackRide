using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KissyFishStateMachine : MonoBehaviour
{
    public KissyFishState currentState;
    public void Initialize(KissyFishState startState)
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
    public void changeState(KissyFishState nextState)
    {
        currentState.exit();
        currentState = nextState;
        nextState.enter();
    }

}
