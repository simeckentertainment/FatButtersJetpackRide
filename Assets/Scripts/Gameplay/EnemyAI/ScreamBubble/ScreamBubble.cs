using UnityEngine;

public class ScreamBubble : HarmfulObject
{
    [SerializeField] public ScreamBubbleStateMachine stateMachine;
    [SerializeField] public Rigidbody rb;
    [SerializeField] public Collider attackCollider;
    public bool hitWall;
    public bool popped;
    public bool PlayerInSightDistance;
    public bool targetAcquired;
    public GameObject target;
    [SerializeField] public AudioSource bubbleAudio;
    [SerializeField] public AudioClip[] idleSounds;
    [SerializeField] public AudioClip AttackSound;
    [SerializeField] public AudioClip raspberrySound;
    [SerializeField] public AudioClip[] noticeSounds;
    [SerializeField] public AudioClip bubblePop;
    //[SerializeField] public Transform destinationVisualizer; //dev use only
    [SerializeField] public MeshRenderer bubbleRenderer;
    [SerializeField] public GameObject[] PhysicsObjects;

    public SBIdleState sBIdleState { get; private set; }
    public SBNoticePlayerState sBNoticePlayerState { get; private set; }
    public SBChasePlayerState sBChasePlayerState { get; private set;}
    public SBPopState sBPopState { get; private set;}

    private void Start()
    {
        PlayerInSightDistance = false;
        sBIdleState = new SBIdleState(this, stateMachine);
        sBNoticePlayerState = new SBNoticePlayerState(this,stateMachine);
        sBChasePlayerState = new SBChasePlayerState(this,stateMachine);
        sBPopState = new SBPopState(this,stateMachine);
        stateMachine.Initialize(sBIdleState);
        targetAcquired = false;

        bubbleAudio.velocityUpdateMode = AudioVelocityUpdateMode.Dynamic;
    }

    private void Update()
    {
        bubbleAudio.volume = SaveManager.Instance.collectibleData.SFXVolumeLevel;
        if (popped)
        {
            stateMachine.changeState(sBPopState);
        }
    }

    protected override void OnPlayerTouched(Player player)
    {
        base.OnPlayerTouched(player);
        popped = true;
    }

    public void PlayAudio(AudioClip clip)
    {
        bubbleAudio.clip = clip;
        bubbleAudio.Play();
    }
}
