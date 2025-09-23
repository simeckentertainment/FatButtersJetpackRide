using UnityEditor;
using UnityEngine;



//The Level 20 boss.
//Boss can be beaten by touching his helmet three times.
//Boss can hurt you with any of his attacks, or his rocket.
public class CowboyBoss : MonoBehaviour
{


    [System.NonSerialized] public CowboyBossStateMachine stateMachine; //This gets set at start.
    public Role role;

    [Header("Level Select stuff")]
    [SerializeField] public RuntimeAnimatorController levelSelectAnimController;
    [SerializeField] public AnimationClip figure8AnimClip;
    [SerializeField] public ParticleSystem rocketFireParticles;
    [Header("Battle Stuff")]
    public bool BattleHasBegun;
    [SerializeField] public bool hitThisFrame;
    public int Maxhealth;
    public int Health;
    [SerializeField] public RuntimeAnimatorController BattleAnimController;
    [SerializeField] public Animator anim;
    [SerializeField] public Transform[] RocketAimPoints;
    [System.NonSerialized] public bool LaughingRocket;
    public Player player;
    public Vector3 OriginalLocalCamPos;
    public Quaternion OriginalCameraAngle;
    public GameObject cam;
    public GameObject RockThrowingCowboy;
    public GameObject[] SpawnedCowboys;
    public GameObject[] RTCowboySpawns;
    public GameObject HideySpot;
    public Vector3 LaughingSpotLoc;
    public Quaternion LaughingSpotRot;
    public bool vulnerable;
    public ForceFieldRelease LeftForcefield;
    public ForceFieldRelease RightForcefield;


    [Header("Graphics stuff")]
    public SkinnedMeshRenderer[] cowboyParts;
    public ParticleSystem AimLaser;
    public ParticleSystem LaserDisk;
    public ParticleSystem GrowLaser;
    public GameObject GrowLaserHitbox;
    public ParticleSystem StreakLaser;
    public CowboyBossIdleState cowboyBossIdleState { get; set; }
    public CBBLevelSelectSubState cBBLevelSelectSubState { get; set; }
    public CowboyBossIntroCinematicState cowboyBossIntroCinematicState { get; set; }
    public CowboyBossWaitForPlayerState cowboyBossWaitForPlayerState { get; set; }
    public CowboyBossPrepToFireLaserState cowboyBossPrepToFireLaserState { get; set; }
    public CowboyBossChargeLaserState cowboyBossChargeLaserState { get; set; }
    public CowboyBossFireLaserState cowboyBossFireLaserState { get; set; }
    public CowboyBossRecoverFromLaserState cowboyBossRecoverFromLaserState { get; set; }
    public CowboyBossRunAndSpawnMinionState cowboyBossRunAndSpawnMinionState { get; set; }
    public CowboyBossHideAndWaitState cowboyBossHideAndWaitState { get; set; }
    public CowboyBossReturnFromHidingState cowboyBossReturnFromHidingState { get; set; }
    public CowboyBossMoveToLaughingPositionState cowboyBossMoveToLaughingPositionState { get; set; }
    public CowboyBossCrazyLaughState cowboyBossCrazyLaughState { get; set; }
    public CowboyBossMoveState cowboyBossMoveState { get; set; }
    public CowboyBossGoToTauntPosState cowboyBossGoToTauntPosState { get; set; }
    public CowboyBossTauntState cowboyBossTauntState { get; set; }
    public CowboyBossDeathState cowboyBossDeathState { get; set; }
    public CowboyBossHitState cowboyBossHitState { get; set; }
    public CowboyBossHitFallState cowboyBossHitFallState { get; set; }
    public CowboyBossHitLiveState cowboyBossHitLiveState { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        //setting-setting
        SetStates();
        DeterminePurpose();

    }
    void FixedUpdate()
    {
        if (role == Role.LevelSelect) { return; }
        if (!vulnerable)
        {
            hitThisFrame = false;
        }
        if (player.saveManager.collectibleData.HASBALL)
        {
            Health = 0;
        }
        if (hitThisFrame)
        {
            stateMachine.changeState(cowboyBossHitState);
        }
    }

    private void SetSettings()
    {
        vulnerable = false;
        LaughingRocket = false;
        LaughingSpotLoc = transform.position;
        LaughingSpotRot = transform.rotation;
    }

    private void DeterminePurpose()
    {
        switch (role)
        {
            case Role.LevelSelect:
                anim.runtimeAnimatorController = levelSelectAnimController;
                stateMachine.Initialize(cBBLevelSelectSubState);
                break;
            case Role.Battle:
                Health = Maxhealth;
                anim.runtimeAnimatorController = BattleAnimController;
                //Object-gathering...
                GetBattleObjects();
                SetSettings();
                //It's ready. Now hurry up and wait.
                stateMachine.Initialize(cowboyBossWaitForPlayerState);
                break;
            default:
                break;
        }
    }

    private void SetStates()
    {
        stateMachine = GetComponent<CowboyBossStateMachine>();
        cowboyBossIdleState = new CowboyBossIdleState(this, stateMachine);
        cBBLevelSelectSubState = new CBBLevelSelectSubState(this, stateMachine);
        cowboyBossIntroCinematicState = new CowboyBossIntroCinematicState(this, stateMachine);
        cowboyBossWaitForPlayerState = new CowboyBossWaitForPlayerState(this, stateMachine);
        cowboyBossPrepToFireLaserState = new CowboyBossPrepToFireLaserState(this, stateMachine);
        cowboyBossChargeLaserState = new CowboyBossChargeLaserState(this, stateMachine);
        cowboyBossFireLaserState = new CowboyBossFireLaserState(this, stateMachine);
        cowboyBossRecoverFromLaserState = new CowboyBossRecoverFromLaserState(this, stateMachine);
        cowboyBossRunAndSpawnMinionState = new CowboyBossRunAndSpawnMinionState(this, stateMachine);
        cowboyBossHideAndWaitState = new CowboyBossHideAndWaitState(this, stateMachine);
        cowboyBossReturnFromHidingState = new CowboyBossReturnFromHidingState(this, stateMachine);
        cowboyBossMoveToLaughingPositionState = new CowboyBossMoveToLaughingPositionState(this, stateMachine);
        cowboyBossCrazyLaughState = new CowboyBossCrazyLaughState(this, stateMachine);
        cowboyBossMoveState = new CowboyBossMoveState(this, stateMachine);
        cowboyBossGoToTauntPosState = new CowboyBossGoToTauntPosState(this, stateMachine);
        cowboyBossTauntState = new CowboyBossTauntState(this, stateMachine);
        cowboyBossDeathState = new CowboyBossDeathState(this, stateMachine);
        cowboyBossHitState = new CowboyBossHitState(this, stateMachine);
        cowboyBossHitFallState = new CowboyBossHitFallState(this, stateMachine);
        cowboyBossHitLiveState = new CowboyBossHitLiveState(this, stateMachine);
    }
    public enum Role { LevelSelect, Battle };


    //Set by Animation event. DO NOT TOUCH.
    public void StartBattleProper() { stateMachine.changeState(cowboyBossMoveState); }

    void GetBattleObjects()
    {
#if UNITY_EDITOR
        //RockThrowingCowboy = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Objects/PuzzlePieces/prefabs/enemies/Cowboy/RockThrowingCowboy.prefab");
#endif
#if !UNITY_EDITOR
        //RockThrowingCowboy = ActiveAssetBundles.ActiveBundles["enemies"].LoadAsset<GameObject>("Assets/Objects/PuzzlePieces/prefabs/enemies/Cowboy/RockThrowingCowboy.prefab");
#endif


        RTCowboySpawns = new GameObject[3] {
            GameObject.Find("RTCowboySpawn1"),
            GameObject.Find("RTCowboySpawn2"),
            GameObject.Find("RTCowboySpawn3")
            };
        player = FindObjectOfType<Player>();
        HideySpot = GameObject.Find("BossHidePos");
        cam = GameObject.FindGameObjectWithTag("MainCamera");
        RocketAimPoints = new Transform[]{
            GameObject.Find("RocketAimPoint0").transform,
            GameObject.Find("RocketAimPoint1").transform,
            GameObject.Find("RocketAimPoint2").transform,
            GameObject.Find("RocketAimPoint3").transform,
            GameObject.Find("RocketAimPoint4").transform,
            GameObject.Find("RocketAimPoint5").transform,
            GameObject.Find("RocketAimPoint6").transform,
            GameObject.Find("RocketAimPoint7").transform,
            GameObject.Find("RocketAimPoint8").transform,
            GameObject.Find("RocketAimPoint9").transform,
        };
        ForceFieldRelease[] fields = FindObjectsByType<ForceFieldRelease>(FindObjectsSortMode.InstanceID);
        if (fields.Length == 0) { return; }
        foreach (ForceFieldRelease f in fields)
        {
            if (f.transform.position.x > transform.position.x)
            { //This is looking for the postmatch forcefield
                RightForcefield = f;
            }
            else
            {
                LeftForcefield = f;
            }
        }
    }

}




