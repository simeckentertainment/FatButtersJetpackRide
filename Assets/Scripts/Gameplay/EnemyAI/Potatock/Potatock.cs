using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potatock : MonoBehaviour{
    [System.NonSerialized] public PotatockStateMachine stateMachine; //This gets set at start.
    public PotatockIdleState potatockIdleState { get; set; }
    // Start is called before the first frame update
    void Start(){
       stateMachine = GetComponent<PotatockStateMachine>();
       potatockIdleState = new PotatockIdleState(this, stateMachine);
       stateMachine.Initialize(potatockIdleState);
    }

    // Update is called once per frame
    void Update(){
    }
    private void OnCollisionEnter(Collision other) {
    }
}




