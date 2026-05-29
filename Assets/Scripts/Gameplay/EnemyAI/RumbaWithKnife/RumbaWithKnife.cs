using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RumbaWithKnife : MonoBehaviour{
    [System.NonSerialized] public RumbaWithKnifeStateMachine stateMachine; //This gets set at start.
    [SerializeField] public Player player;
    [SerializeField] public RumbaPlayerDetector playerDetector;
    [SerializeField] public Animator anim;
    [SerializeField] public float idleSpeed;
    [SerializeField] public float playerDetectedSpeed;
    [SerializeField] public float MadSpeed;

    public bool PlayerDetected { get; private set; }
    public RumbaWithKnifeIdleState rumbaWithKnifeIdleState { get; set; }
    // Start is called before the first frame update
    void Start(){
       stateMachine = GetComponent<RumbaWithKnifeStateMachine>();
       rumbaWithKnifeIdleState = new RumbaWithKnifeIdleState(this, stateMachine);
       stateMachine.Initialize(rumbaWithKnifeIdleState);
    }

    // Update is called once per frame
    void Update(){
    }
    private void OnCollisionEnter(Collision other) {
    }

    public void SetPlayerDetected(bool value){
        PlayerDetected = value;
    }
}




