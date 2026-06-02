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
    [SerializeField] public float calmSpeed;
    [SerializeField] public float playerDetectedSpeed;
    [SerializeField] public float MadSpeed;
    [SerializeField] public float HP = 2.0f;
    [SerializeField] public float leftFacingRot;
    [SerializeField] public float rightFacingRot;
    [SerializeField] public float calmTurnFrameCountMax;
    [SerializeField] public float angryTurnFrameCountMax;
    public Vector3 spawnLoc {get; private set;}
    public Vector3 wanderGoalLoc;

    [SerializeField] public Direction direction = Direction.Left;
    [SerializeField] public float WanderDistance;

    public bool PlayerDetected { get; private set; }
    public RumbaWithKnifeIdleState rumbaIdleState { get; set; }
    public RumbaWithKnifeDeadState rumbaDeadState { get; set; }
    public RumbaWithKnifeTurnState rumbaTurnState { get; set; }
    public RumbaWithKnifeRollState rumbaRollState { get; set; }
        public RumbaWithKnifeSpinState rumbaSpinState { get; set; }
    // Start is called before the first frame update
    void Start(){
        SetSpawnLoc();
        stateMachine = GetComponent<RumbaWithKnifeStateMachine>();
        rumbaIdleState = new RumbaWithKnifeIdleState(this, stateMachine);
        rumbaDeadState = new RumbaWithKnifeDeadState(this, stateMachine);
        rumbaTurnState = new RumbaWithKnifeTurnState(this, stateMachine);
        rumbaRollState = new RumbaWithKnifeRollState(this, stateMachine);
        rumbaSpinState = new RumbaWithKnifeSpinState(this, stateMachine);
        stateMachine.Initialize(rumbaIdleState);
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

    public enum Direction
    {
        Left,
        Right,
        Spinning,
    }
}




