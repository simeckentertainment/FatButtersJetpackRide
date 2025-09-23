using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;



public class Player : MonoBehaviour
{
    [SerializeField] public SaveManager saveManager;
    public PlayerStateMachine stateMachine;
    public PlayerIdleState playerIdleState { get; set; }
    public PlayerFallState playerFallState { get; set; }
    public PlayerEnterDangleState playerEnterDangleState { get; set; }
    public PlayerDangleState playerDangleState { get; set; }
    public PlayerHurtState playerHurtState { get; set; }
    public PlayerNoFuelState playerNoFuelState { get; set; }
    public PlayerOHKState playerOHKState { get; set; }
    public PlayerThrustState playerThrustState { get; set; }
    public PlayerTummyDeathState playerTummyDeathState { get; set; }
    public PlayerWinState playerWinState {get; set;}
    [Header("Utility classes. Should be set in inspector.")]
    [SerializeField] public Rigidbody rb;
    [SerializeField] public InputDriver input;
    [SerializeField] public AudioSource sfx;
    [SerializeField] public CorgiEffectHolder vfx;
    [SerializeField] public UIManager UI;
    [SerializeField] public Button PauseButton;

    [Header("Skin stuff")]
    public int skindex; //A    cheeky way of saying "The active skin index number"
    public Animator anim;
    [Header("Skin-Specific fields")]
    public Animator secondaryAnim;
    public GravyBoatRotator gbr1;
    public GravyBoatRotator gbr2;
    [SerializeField] public AudioClip[] borks;
    [System.NonSerialized] public int baseThrust = 25;
    [SerializeField] public GameObject[] CollidersAndTriggers;
    [Header("Important internal data")]
    public float thrust;
    public float fuel;
    public float maxFuel;
    [System.NonSerialized] public float fuelPercent;
    [System.NonSerialized] public float tummyPercent;
    public float tummy;
    public float maxTummy;
    public int tempBones;
    [System.NonSerialized] public float animationPercentage;
    [Header("Rotation stuff")]
    [System.NonSerialized] public float GravityRoll;
    [System.NonSerialized] public float KeyboardRollOffset;
    public int KeyboardSensitivity;
    public bool corgiTurned;

    [Header("Collision bools")]
    public bool GroundTouch;
    public bool HarmfulTouch;
    public float HarmfulDamageAmount;
    public Vector3 HarmfulTouchObjectPosition;
    public bool BoneTouch;
    public bool FoodTouch;
    public float FoodAdditionAmount;
    public bool JerryCanTouch;
    public float FuelAdditionAmount;
    public bool FinishTouch;
    public bool OHKTouch;
    public bool BallTouch;
    public bool hasTemporaryBall;
    public bool hasPermaBall;
    public int ballTimerMax = 600;
    public bool OtherObjectTouch;
    public enum PlayerDirection{Left,Right};
    public PlayerDirection playerDirection;
    public bool LowGravMode;

    // Start is called before the first frame update

    void Awake()
    {
        if (saveManager == null)
        {
            saveManager = Helper.NabSaveData().GetComponent<SaveManager>();
        }
        vfx.StopRocketSounds();
    }
    void Start()
    {
        //transform.Rotate(Vector3.back * 0.1f);
        corgiTurned = false;
        skindex = saveManager.collectibleData.CurrentSkin;
        vfx.ApplySkin(skindex);
        
        ApplyStoreUpgrades();
        playerIdleState = new PlayerIdleState(this, stateMachine);
        playerFallState = new PlayerFallState(this, stateMachine);
        playerEnterDangleState = new PlayerEnterDangleState(this, stateMachine);
        playerDangleState = new PlayerDangleState(this, stateMachine);
        playerHurtState = new PlayerHurtState(this, stateMachine);
        playerNoFuelState = new PlayerNoFuelState(this, stateMachine);
        playerOHKState = new PlayerOHKState(this, stateMachine);
        playerThrustState = new PlayerThrustState(this, stateMachine);
        playerTummyDeathState = new PlayerTummyDeathState(this, stateMachine);
        playerWinState = new PlayerWinState(this, stateMachine);
        stateMachine.Initialize(playerIdleState);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Shelf"))
        {
            Debug.Log("Ground touch true");

            transform.SetParent(collision.transform, true);
        }
    }


    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Shelf"))
        {
            Debug.Log("Ground touch false");

            transform.SetParent(null, true);
        }
    }


    #region DataStuff
    void ApplyStoreUpgrades(){
        thrust = baseThrust+saveManager.collectibleData.thrustUpgradeLevel;
        maxFuel = saveManager.collectibleData.fuelUpgradeLevel*20.0f;
        fuel = maxFuel;
        fuelPercent = fuel/maxFuel;
        maxTummy = saveManager.collectibleData.treatsUpgradeLevel;
        tummy = maxTummy;
        tummyPercent = tummy/maxTummy;
        if(saveManager.collectibleData.HASBALL){
            hasPermaBall = true;
        }
    }
    #endregion
    
}