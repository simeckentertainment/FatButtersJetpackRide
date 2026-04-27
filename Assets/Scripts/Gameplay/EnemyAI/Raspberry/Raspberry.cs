using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raspberry : MonoBehaviour{
    [System.NonSerialized] public RaspberryStateMachine stateMachine; //This gets set at start.
    public RaspberryIdleState raspberryIdleState { get; set; }
    // Start is called before the first frame update
    void Start(){
       stateMachine = GetComponent<RaspberryStateMachine>();
       raspberryIdleState = new RaspberryIdleState(this, stateMachine);
       stateMachine.Initialize(raspberryIdleState);
    }

    // Update is called once per frame
    void Update(){
    }
    private void OnCollisionEnter(Collision other) {
    }
}




