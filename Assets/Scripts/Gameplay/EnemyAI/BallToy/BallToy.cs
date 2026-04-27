using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallToy : MonoBehaviour{
    [System.NonSerialized] public BallToyStateMachine stateMachine; //This gets set at start.
    public BallToyIdleState ballToyIdleState { get; set; }
    // Start is called before the first frame update
    void Start(){
       stateMachine = GetComponent<BallToyStateMachine>();
       ballToyIdleState = new BallToyIdleState(this, stateMachine);
       stateMachine.Initialize(ballToyIdleState);
    }

    // Update is called once per frame
    void Update(){
    }
    private void OnCollisionEnter(Collision other) {
    }
}




