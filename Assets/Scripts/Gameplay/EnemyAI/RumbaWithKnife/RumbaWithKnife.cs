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
    [SerializeField] public Rigidbody rb;
    [SerializeField] public Animator anim;
    [SerializeField] public MeshRenderer rumbaMesh; //Can't access the mat without this.
    [SerializeField] public ParticleSystem[] MadParticleSpouts;
    [SerializeField] public float wallDistanceTrigger;
    [SerializeField] public float calmSpeed;
    [SerializeField] public float madSpeed;
    [System.NonSerialized] public float HP = 2.0f;
    [System.NonSerialized] public float leftFacingRot = 270f;
    [System.NonSerialized] public float rightFacingRot = 90f;
    [System.NonSerialized] public float calmTurnFrameCountMax = 20;
    [System.NonSerialized] public float angryTurnFrameCountMax = 60;
    [SerializeField] public float deathRageCountMax = 120;
    [System.NonSerialized] public Direction cliffDetected = Direction.None;
    [System.NonSerialized] public Direction wallDetected = Direction.None;
    [SerializeField] public bool ignoreLeft;
    [SerializeField] public bool ignoreRight;
    [SerializeField] public GameObject[] deathExplosionObjects;

    public Vector3 spawnLoc {get; private set;}
    public bool angered {get; private set;}
    [System.NonSerialized] public Vector3 wanderGoalLoc;
    [System.NonSerialized] public Vector3 wanderRightMax;
    [System.NonSerialized] public Vector3 wanderLeftMax;

    public Direction direction = Direction.Right;
    [SerializeField] public float WanderDistance;
    public RumbaWithKnifeIdleState rumbaIdleState { get; set; }
    public RumbaWithKnifeDeadState rumbaDeadState { get; set; }
    public RumbaWithKnifeTurnState rumbaTurnState { get; set; }
    public RumbaWithKnifeRollState rumbaRollState { get; set; }
    public RumbaWithKnifeSpinState rumbaSpinState { get; set; }
    public RumbaWithKnifeGetMadState rumbaGetMadState { get; set; }
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
        rumbaSoftlockState = new RumbaWithKnifeSoftlockState(this, stateMachine);
        stateMachine.Initialize(rumbaIdleState);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!angered && Helper.isWithinMarginOfError(HP, 1.0f, 0.1f)) 
        {
            angered = true;
            stateMachine.changeState(rumbaGetMadState);
        }

        if(Helper.isWithinMarginOfError(HP, 0.0f, 0.1f)){
            stateMachine.changeState(rumbaDeadState);
        }
        if(ignoreLeft && ignoreRight) //If we're softlocked
        {
            stateMachine.changeState(rumbaSoftlockState);

        }

    }

    void SetSpawnLoc()
    {
        spawnLoc = transform.position;
    }

    public enum Direction
    {
        None,
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
}




