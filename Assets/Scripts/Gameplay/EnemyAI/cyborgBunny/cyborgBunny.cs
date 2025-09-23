using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
Cyborg Bunny's main script.
*/
public class CyborgBunny : MonoBehaviour
{
    [System.NonSerialized] public CyborgBunnyStateMachine stateMachine; //This gets set at start.
    [System.NonSerialized] public Player player;
    [SerializeField] public float spawnAnimLength;
    [SerializeField] GameObject newBunnySpawnPoint;
    [SerializeField] public Rigidbody rb;
    [SerializeField] public Animator anim;
    public bool playerSeen;
    [SerializeField] public ParticleSystem floatationParticles;
    [SerializeField] public ParticleSystem[] eyeLasers;
    [SerializeField] public float LaserFireFrequency;
    [SerializeField] public float AggroYDistanceFromPlayer;
    [SerializeField] public float bunnySpawnFrequency;
    public CyborgBunnySpawnState cyborgBunnySpawnState { get; set; }
    public CyborgBunnyIdleState cyborgBunnyIdleState { get; set; }
    public CyborgBunnyDeathState cyborgBunnyDeathState { get; set; }
    public CyborgBunnyNoticePlayerState cyborgBunnyNoticePlayerState { get; set; }
    public CyborgBunnyChasePlayerState cyborgBunnyChasePlayerState { get; set; }
    public CyborgBunnyFireLaserState cyborgBunnyFireLaserState { get; set; }
    [SerializeField] public GameObject[] lightningPlates;
    [SerializeField] public Transform LineOfSightSourceObj;
    [SerializeField] public float LineOfSightMaxDistance;
    [SerializeField] public bool defeated;
    [SerializeField] public float speedMult;
    [SerializeField] public ParticleSystem[] lasers;
    // Start is called before the first frame update
    void Start()
    {
        defeated = false;
        playerSeen = false;
        stateMachine = GetComponent<CyborgBunnyStateMachine>(); //
        cyborgBunnySpawnState = new CyborgBunnySpawnState(this, stateMachine);
        cyborgBunnyIdleState = new CyborgBunnyIdleState(this, stateMachine);
        cyborgBunnyDeathState = new CyborgBunnyDeathState(this, stateMachine);
        cyborgBunnyNoticePlayerState = new CyborgBunnyNoticePlayerState(this, stateMachine);
        cyborgBunnyChasePlayerState = new CyborgBunnyChasePlayerState(this, stateMachine);
        cyborgBunnyFireLaserState = new CyborgBunnyFireLaserState(this, stateMachine);
        player = FindAnyObjectByType<Player>();
        StopLasers();
        stateMachine.Initialize(cyborgBunnySpawnState);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (defeated) { stateMachine.changeState(cyborgBunnyDeathState); }
    }
    private void OnCollisionEnter(Collision other)
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PlayerDamageTrigger")
        {
            defeated = true;
        }
    }
    public void MultiplyRabbit()
    {
        //Spawns a new cyborg bunny. Doesn't run if defeated.
        if (defeated) { return; }
        GameObject newBunny = Instantiate(this.transform.parent.gameObject, newBunnySpawnPoint.transform.position, Quaternion.identity, null);
        newBunny.GetComponentInChildren<CyborgBunny>().transform.localPosition = Vector3.zero;

    }

    public void StopMomentum()
    {
        rb.linearVelocity = Vector3.zero;
    }

    public void FireLasers()
    {
        foreach (ParticleSystem laser in lasers)
        {
            laser.Play();
        }
    }
        void StopLasers()
    {
        foreach (ParticleSystem laser in lasers)
        {
            laser.Stop();
        }
    }

}




