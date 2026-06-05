using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RumbaWithKnife : MonoBehaviour{
    //The Rumba with a Knife wanders aimlessly until it detects the player.
    //Then it Chases the player.
    //Once the player gets one hit on it, it gets mad and chases the player faster.
    //Once the player gets a second hit on it, it dies and explodes.
    //The player can get hurt by colliding with the knife!



    [System.NonSerialized] public RumbaWithKnifeStateMachine stateMachine; //This gets set at start.
    [SerializeField] Rigidbody rb;
    [SerializeField] public Animator anim;
    [SerializeField] public MeshRenderer rumbaMesh; //Can't access the mat without this.
    [SerializeField] public ParticleSystem[] MadParticleSpouts;
    [SerializeField] public Transform[] CastPosObjs;
    [SerializeField] public Transform LeftCastPosObj; //This will change whenever we turn.
    [SerializeField] public Transform RightCastPosObj;//This will change whenever we turn.
    [SerializeField] public float wallDistanceTrigger;
    [SerializeField] public float calmSpeed;
    [SerializeField] public float madSpeed;
    [System.NonSerialized] public float HP = 2.0f;
    [System.NonSerialized] public float leftFacingRot = 270f;
    [System.NonSerialized] public float rightFacingRot = 90f;
    [System.NonSerialized] public float calmTurnFrameCountMax = 20;
    [System.NonSerialized] public float angryTurnFrameCountMax = 60;

    public Vector3 spawnLoc {get; private set;}
    private bool angered;
    [System.NonSerialized] public Vector3 wanderGoalLoc;

    public Direction direction = Direction.Left;
    [SerializeField] public float WanderDistance;

    public bool PlayerDetected { get; private set; }
    public RumbaWithKnifeIdleState rumbaIdleState { get; set; }
    public RumbaWithKnifeDeadState rumbaDeadState { get; set; }
    public RumbaWithKnifeTurnState rumbaTurnState { get; set; }
    public RumbaWithKnifeRollState rumbaRollState { get; set; }
    public RumbaWithKnifeSpinState rumbaSpinState { get; set; }
    public RumbaWithKnifeGetMadState rumbaGetMadState { get; set; }
    public RumbaWithKnifeMadIdleState rumbaMadIdleState { get; set; }
    public RumbaWithKnifeMadTurnState rumbaMadTurnState { get; set; }
    public RumbaWithKnifeMadRollState rumbaMadRollState { get; set; }
    public RumbaWithKnifeMadSpinState rumbaMadSpinState { get; set; }
    public RumbaWithKnifeSoftlockState rumbaSoftlockState {get; set;}
    // Start is called before the first frame update
    void Start(){
        angered = false;
        SetSpawnLoc();
        SetAngerSpouts(false);
        stateMachine = GetComponent<RumbaWithKnifeStateMachine>();
        rumbaIdleState = new RumbaWithKnifeIdleState(this, stateMachine);
        rumbaDeadState = new RumbaWithKnifeDeadState(this, stateMachine);
        rumbaTurnState = new RumbaWithKnifeTurnState(this, stateMachine);
        rumbaRollState = new RumbaWithKnifeRollState(this, stateMachine);
        rumbaSpinState = new RumbaWithKnifeSpinState(this, stateMachine);
        rumbaGetMadState = new RumbaWithKnifeGetMadState(this, stateMachine);
        rumbaMadIdleState = new RumbaWithKnifeMadIdleState(this, stateMachine);
        rumbaMadTurnState = new RumbaWithKnifeMadTurnState(this, stateMachine);
        rumbaMadRollState = new RumbaWithKnifeMadRollState(this, stateMachine);
        rumbaMadSpinState = new RumbaWithKnifeMadSpinState(this, stateMachine);
        rumbaSoftlockState = new RumbaWithKnifeSoftlockState(this, stateMachine);
        stateMachine.Initialize(rumbaIdleState);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!angered && Helper.isWithinMarginOfError(HP, 1.0f, 0.0001f))
        {
            Debug.Log(HP);
            angered = true;
            stateMachine.changeState(rumbaGetMadState);
        }
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

    public void SetAngerSpouts(bool setvar)
    { //Turns on the RAGE
        if(setvar){
            foreach (ParticleSystem spout in MadParticleSpouts){spout.Play();}
        } else
        {
            foreach(ParticleSystem spout in MadParticleSpouts){spout.Stop();}
        }
    }

        public virtual void CalibrateRaycastNodes()
    { //Whichever one has a greater value goes on the right.
        if(CastPosObjs[0].transform.position.x > CastPosObjs[1].transform.position.x)
        {
            RightCastPosObj = CastPosObjs[0];
            LeftCastPosObj = CastPosObjs[1];
        } else
        {
            RightCastPosObj = CastPosObjs[1];
            LeftCastPosObj = CastPosObjs[0];
        }
    }
}




