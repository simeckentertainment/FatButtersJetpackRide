using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SegwayBear : MonoBehaviour
{

    [SerializeField] public SegwayBearStateMachine stateMachine;
    [SerializeField] public Rigidbody rb;
    public SeBIdleState seBIdleState { get; private set; }
    public SeBNoticePlayerState seBNoticePlayerState { get; private set; }
    public SeBChargeLaserState seBChargeLaserState { get; private set;}
    public SeBFireLaserState seBFireLaserState { get; private set;}
    public SeBBeatenState seBBeatenState { get; private set;}
    public MeshRenderer[] boundaries;
    [SerializeField] public WheelMachine axel;
    [SerializeField] public AngleMachine angleMachine;
    [SerializeField] public float idleDestination;
    [SerializeField] public MeshRenderer[] LaserRings;
    [SerializeField] public ParticleSystem[] LaserParticles;
    public AudioSource bearAudio;
    public AudioClip[] BearSounds;
    public Transform[] PitchCubes;
    public bool detectedPlayer;
    public Transform target;
    public bool AdjustingLaser;
    public Vector3 LaserTarget;
    List<float> LaserAdjustmentAngles;

    [Header("Death Stuff")]
    public bool beaten;
    public GameObject booBearModel;
    public GameObject[] wheels;
    public GameObject[] DeathPhysicsPieces;

    [SerializeField] public float yOffset = 5f;


    public IEnumerator RefreezeXRotationAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        rb.constraints |= RigidbodyConstraints.FreezeRotationX;
    }

    // Start is called before the first frame update
    void Start()
    {
        LaserAdjustmentAngles = new List<float>();
        AdjustingLaser = false;
        seBIdleState = new SeBIdleState(this, stateMachine);
        seBNoticePlayerState = new SeBNoticePlayerState(this,stateMachine);
        seBChargeLaserState = new SeBChargeLaserState(this,stateMachine);
        seBFireLaserState = new SeBFireLaserState(this,stateMachine);
        seBBeatenState = new SeBBeatenState(this,stateMachine);
        foreach(ParticleSystem ps in LaserParticles){
            ps.Stop();
        }
        stateMachine.Initialize(seBIdleState);
        rb.centerOfMass = new Vector3(0, yOffset, 0); // yOffset may need tuning

    }

    // Update is called once per frame
    void Update()
    {
        if(AdjustingLaser){
            RunLaserAdjustment();
        }

    }
    public void SetDestination(float destination, float speed){
        float currentPos = transform.position.x;
        if(destination < currentPos){ //going to move left.
            if(rb.linearVelocity.normalized.x > 0){
                angleMachine.StartNewNoCompPeriod(40);
            }
            axel.rotateAdditive = speed;
        } else if (destination > currentPos){ //going to move right.
        if(rb.linearVelocity.normalized.x < 0){
            angleMachine.StartNewNoCompPeriod(40);
        }
            axel.rotateAdditive = speed*-1;

        } else {
            axel.rotateAdditive = 0.0f;
        }
    }
    public float GetAudioPercentage(){
        float maxTime = bearAudio.clip.length;
        float currentTime = bearAudio.time;
        return currentTime/maxTime;
    }


    public void SetLaserTarget(Vector3 target){
        
    }
    void RunLaserAdjustment(){

    }
}




public class BearSpeed{
    public const float slow = 30f;
    public const float medium = 60f;
    public const float fast = 90f;
    public const float breakneck = 180f;
}
