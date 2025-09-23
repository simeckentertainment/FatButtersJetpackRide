using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreamBubble : MonoBehaviour
{

    [SerializeField] public ScreamBubbleStateMachine stateMachine;
    [SerializeField] SaveManager saveManager;
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



    // Start is called before the first frame update
    void Start()
    {
        PlayerInSightDistance = false;
        if (saveManager == null)
        {
            saveManager = Helper.NabSaveData().GetComponent<SaveManager>();
        }
        sBIdleState = new SBIdleState(this, stateMachine);
        sBNoticePlayerState = new SBNoticePlayerState(this,stateMachine);
        sBChasePlayerState = new SBChasePlayerState(this,stateMachine);
        sBPopState = new SBPopState(this,stateMachine);
        stateMachine.Initialize(sBIdleState);
        targetAcquired = false;
    }

    // Update is called once per frame
    void Update()
    {
        bubbleAudio.volume = saveManager.collectibleData.SFXVolumeLevel;
        if (popped){
            stateMachine.changeState(sBPopState);
        }
    }
    private void OnCollisionEnter(Collision other) {

        switch(other.gameObject.tag){
            case "Player":
                popped = true;
                break;
            case "OneHitKill":
                popped = true;
                break;
            default:
                hitWall = true;
            break;
        }
    }
    public void PlayAudio(AudioClip clip){
        bubbleAudio.clip = clip;
        bubbleAudio.Play();
    }
}
