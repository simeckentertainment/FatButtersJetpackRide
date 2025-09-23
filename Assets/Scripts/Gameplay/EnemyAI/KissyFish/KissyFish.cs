using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KissyFish : MonoBehaviour
{
    public Vector3 ApexTargetCoords;
    [SerializeField] public KissyFishStateMachine stateMachine;
    [SerializeField] public Rigidbody rb;
    public KissyFishFlyState kissyFishFlyState { get; private set; }
    public KissyFishCollideWIthPlayerState kissyFishCollideWIthPlayerState {get; private set;}
    public KissyFishFlopState kissyFishFlopState  {get; private set;}
    //public Collider waterCollider;

    [SerializeField] public AudioSource fishAudio;
    [SerializeField] public AudioClip[] JumpSounds;
    [SerializeField] public AudioClip DeathSound;
    [SerializeField] public AudioClip FlopSound;
    public float launchMagnitude;
    public KissyFishSpawner spawner;
    public bool touchedWater;
    public int lifeTime;
    int lifeTimeCounter;


    // Start is called before the first frame update
    void Start()
    {
        lifeTimeCounter = 0;
        touchedWater = false;
        kissyFishFlyState = new KissyFishFlyState(this, stateMachine);
        kissyFishCollideWIthPlayerState = new KissyFishCollideWIthPlayerState(this, stateMachine);
        kissyFishFlopState = new KissyFishFlopState(this, stateMachine);
        stateMachine.Initialize(kissyFishFlyState);
    }

    // Update is called once per frame
    void Update()
    {
        lifeTimeCounter++;
        if(lifeTimeCounter>lifeTime){
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision other) {
        switch(other.gameObject.tag){
            case "Water":
                stateMachine.changeState(kissyFishFlopState);
                //touchedWater = true;
                break;
            case "Player":
                stateMachine.changeState(kissyFishFlopState);
                break;
            default:
                stateMachine.changeState(kissyFishFlopState);
                break;
        }

    }


}

