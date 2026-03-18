using UnityEngine;

public class KissyFish : HarmfulObject
{
    public Vector3 ApexTargetCoords;
    [SerializeField] public KissyFishStateMachine stateMachine;
    [SerializeField] public Rigidbody rb;
    public KissyFishFlyState kissyFishFlyState { get; private set; }
    public KissyFishCollideWIthPlayerState kissyFishCollideWIthPlayerState {get; private set;}
    public KissyFishFlopState kissyFishFlopState  {get; private set;}

    [SerializeField] public AudioSource fishAudio;
    [SerializeField] public AudioClip[] JumpSounds;
    [SerializeField] public AudioClip DeathSound;
    [SerializeField] public AudioClip FlopSound;
    public float launchMagnitude;
    public KissyFishSpawner spawner;
    public bool touchedWater;
    public int lifeTime;
    int lifeTimeCounter;

    private void Start()
    {
        lifeTimeCounter = 0;
        touchedWater = false;
        kissyFishFlyState = new KissyFishFlyState(this, stateMachine);
        kissyFishCollideWIthPlayerState = new KissyFishCollideWIthPlayerState(this, stateMachine);
        kissyFishFlopState = new KissyFishFlopState(this, stateMachine);
        stateMachine.Initialize(kissyFishFlyState);
    }

    private void Update()
    {
        lifeTimeCounter++;
        if (lifeTimeCounter>lifeTime)
        {
            Destroy(gameObject);
        }
    }

    protected override void OnPlayerTouched(Player player)
    {
        base.OnPlayerTouched(player);
        stateMachine.changeState(kissyFishFlopState);
        Destroy(this.gameObject);
    }

    protected override void OnCollisionEnter(Collision other)
    {
        base.OnCollisionEnter(other);
        stateMachine.changeState(kissyFishFlopState);
    }
}

