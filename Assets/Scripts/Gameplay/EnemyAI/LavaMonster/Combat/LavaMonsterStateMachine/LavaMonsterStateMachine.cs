using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaMonsterStateMachine : MonoBehaviour
{
    public LavaMonsterState currentState;
    public void Initialize(LavaMonsterState startState)
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
    public void changeState(LavaMonsterState nextState)
    {
        currentState.exit();
        currentState = nextState;
        nextState.enter();
    }

}
