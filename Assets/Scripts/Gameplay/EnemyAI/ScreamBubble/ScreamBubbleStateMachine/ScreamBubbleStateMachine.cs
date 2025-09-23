using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreamBubbleStateMachine : MonoBehaviour
{
    public ScreamBubbleState currentState;
    public void Initialize(ScreamBubbleState startState)
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
    public void changeState(ScreamBubbleState nextState)
    {
        currentState.exit();
        currentState = nextState;
        nextState.enter();
    }

}
