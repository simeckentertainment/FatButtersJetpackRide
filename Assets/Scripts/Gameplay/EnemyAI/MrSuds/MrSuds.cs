using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MrSuds : MonoBehaviour{
    [System.NonSerialized] public MrSudsStateMachine stateMachine; //This gets set at start.
    public MrSudsIdleState mrSudsIdleState { get; set; }
    // Start is called before the first frame update
    void Start(){
       stateMachine = GetComponent<MrSudsStateMachine>();
       mrSudsIdleState = new MrSudsIdleState(this, stateMachine);
       stateMachine.Initialize(mrSudsIdleState);
    }

    // Update is called once per frame
    void Update(){
    }
    private void OnCollisionEnter(Collision other) {
    }
}




