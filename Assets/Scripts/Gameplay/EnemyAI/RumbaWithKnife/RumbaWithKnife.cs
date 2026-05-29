using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RumbaWithKnife : MonoBehaviour{
    //The Rumba with a Knife wanders aimlessly until it detects the player.
    //Then it Chases the player.
    //Once the player gets one hit on it, it gets mad and chases the player faster.
    //Once the player gets a second hit on it, it dies and explodes.
    //The player can get hurt by colliding with the knife!



    [System.NonSerialized] public RumbaWithKnifeStateMachine stateMachine; //This gets set at start.
    [SerializeField] public Player player;
    [SerializeField] public RumbaPlayerDetector playerDetector;
    [SerializeField] public Animator anim;
    [SerializeField] public float idleSpeed;
    [SerializeField] public float playerDetectedSpeed;
    [SerializeField] public float MadSpeed;
    [SerializeField] public float HP = 2.0f;
    public Vector3 spawnLoc {get; private set;}
    public float WanderDistance;

    public bool PlayerDetected { get; private set; }
    public RumbaWithKnifeIdleState rumbaWithKnifeIdleState { get; set; }
    public RumbaWithKnifeDeadState rumbaWithKnifeDeadState { get; set; }
    // Start is called before the first frame update
    void Start(){
       SetSpawnLoc();
       stateMachine = GetComponent<RumbaWithKnifeStateMachine>();
       rumbaWithKnifeIdleState = new RumbaWithKnifeIdleState(this, stateMachine);
       rumbaWithKnifeDeadState = new RumbaWithKnifeDeadState(this, stateMachine);
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
    void SetSpawnLoc()
    {
        spawnLoc = transform.position;
    }
}




