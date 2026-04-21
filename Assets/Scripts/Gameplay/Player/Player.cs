using Solo.MOST_IN_ONE;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    private static Player _instance;
    public static Player Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<Player>();
            }

            return _instance;
        }
    }

    public PlayerStateMachine stateMachine;
    public PlayerIdleState playerIdleState { get; set; }
    public PlayerWalkState playerWalkState{get;set;}

    public PlayerFallState playerFallState { get; set; }
    public PlayerEnterDangleState playerEnterDangleState { get; set; }
    public PlayerDangleState playerDangleState { get; set; }
    public PlayerHurtState playerHurtState { get; set; }
    public PlayerOHKState playerOHKState { get; set; }
    public PlayerThrustState playerThrustState { get; set; }
    public PlayerTummyDeathState playerTummyDeathState { get; set; }
    public PlayerWinState playerWinState {get; set;}
    [Header("Utility classes. Should be set in inspector.")]
    [SerializeField] public Rigidbody rb;
    [SerializeField] public InputDriver input;
    [SerializeField] public AudioSource sfx;
    [SerializeField] public AudioSource grrSfx;
    [SerializeField] public CorgiEffectHolder vfx;
    [SerializeField] public UIManager UI;

    [Header("Skin stuff")]
    public int skindex; //A cheeky way of saying "The active skin index number"
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
    [System.NonSerialized] public float baseThrustWithUpgrades; // Base thrust including upgrades (used for boost calculations)
    [SerializeField] private float jumpForce = 12;

    public float maxFuel;
    [System.NonSerialized] public float fuelPercent;
    [System.NonSerialized] public float tummyPercent;
    public float tummy;
    public float maxTummy;
    public List<Collider> CollidersInJetpackKillZone;
    [System.NonSerialized] public int thrusterRechargeCounter = 0;
    [System.NonSerialized] public float animationPercentage;
    [SerializeField] private float thrusterRechargeDelay;
    [SerializeField] private float thrusterRechargeRate;

    [Header("Rotation stuff")]
    [System.NonSerialized] public float GravityRoll;
    [SerializeField] public float KeyboardRollOffset;
    public int KeyboardSensitivity;
    public bool corgiTurned;

    [Header("Walk mechanics")]
    [SerializeField] public float walkDirection; // Walk direction (-1 or 1)
    [System.NonSerialized] public float walkCurrentSpeed; // Current smoothed walk speed

    [SerializeField] public float slowWalkSpeed;
    [SerializeField] public float mediumWalkSpeed;
    [SerializeField] public float fastWalkSpeed;

    [SerializeField] public float SlowWalkMinAngle;
    [SerializeField] public float MediumWalkMinAngle;
    [SerializeField] public float FastWalkMinAngle;

    [Header("Collision bools")]
    public bool HarmfulTouch;
    public float HarmfulDamageAmount;
    public Vector3 HarmfulTouchObjectPosition;
    public float FoodAdditionAmount;
    public bool FuelTouch;
    public bool FinishTouch;
    public bool OHKTouch;
    public bool BallTouch;
    public bool hasTemporaryBall;
    public bool hasPermaBall;
    public int ballTimerMax = 600;
    public bool killThrustTriggerTouch;
    public enum PlayerDirection{Left,Right};
    public PlayerDirection playerDirection;
    public bool LowGravMode;

    public bool TouchingGround => currentGroundColliders.Count > 0;
    public bool IsGrounded => GroundNear || TouchingGround;
    public bool GroundNear { get; set; }

    public int BonesCollected { get; private set; }
    public int FoodsCollected { get; private set; }
    public int BallsCollected { get; private set; }
    public int FuelsCollected { get; private set; }
    public int EnemiesDefeated { get; private set; }

    public UnityEvent OnPickupCollected { get; set; } = new UnityEvent();
    public UnityEvent OnFuelUpdated { get; set; } = new UnityEvent();
    public UnityEvent OnJetpackStatusUpdated { get; set; } = new UnityEvent();

    private CollectibleData collectibleData => SaveManager.Instance.collectibleData;

    private HashSet<(int, int)> currentGroundColliders = new HashSet<(int, int)>();

    public bool CanJump => IsGrounded && !IsJumping;

    public bool IsJumping
    {
        get
        {
            return anim.GetBool("IsJumping");
        }
        set
        {
            anim.SetBool("IsJumping", value);
        }
    }

    private bool _jetpackActivationPossible;
    public bool JetpackActivationPossible
    {
        get
        {
            return _jetpackActivationPossible;
        }
        set
        {
            if (value != _jetpackActivationPossible)
            {
                _jetpackActivationPossible = value;
                OnJetpackStatusUpdated.Invoke();
            }
        }
    }

    [System.NonSerialized] int fallDelayCounter = 0;
    [SerializeField]int fallDelayThreshold;
    public bool IsFalling()
    {
        if (!IsGrounded)
        {
            fallDelayCounter++;
        }
        else
        {
            fallDelayCounter = 0;
        }

        return fallDelayCounter >= fallDelayThreshold;
    }

    private float _fuel;
    public float Fuel
    {
        get
        {
            return _fuel;
        }
        set
        {
            _fuel = value;
            if (_fuel > maxFuel)
            {
                _fuel = maxFuel;
            }
            if(_fuel < 0.0f)
            {
                Fuel = 0.0f;
            }

            OnFuelUpdated.Invoke();
        }
    }

    public bool IsAlive => stateMachine.currentState.IsAliveState;

    void Awake()
    {
        vfx.StopRocketSounds();
    }

    void Start()
    {
        //transform.Rotate(Vector3.back * 0.1f);
        corgiTurned = false;
        skindex = collectibleData.CurrentSkin;
        vfx.ApplySkin(skindex);
        JetpackActivationPossible = true;
        ApplyStoreUpgrades();
        playerIdleState = new PlayerIdleState(this, stateMachine);
        playerWalkState = new PlayerWalkState(this, stateMachine);
        playerFallState = new PlayerFallState(this, stateMachine);
        playerEnterDangleState = new PlayerEnterDangleState(this, stateMachine);
        playerDangleState = new PlayerDangleState(this, stateMachine);
        playerHurtState = new PlayerHurtState(this, stateMachine);
        playerOHKState = new PlayerOHKState(this, stateMachine);
        playerThrustState = new PlayerThrustState(this, stateMachine);
        playerTummyDeathState = new PlayerTummyDeathState(this, stateMachine);
        playerWinState = new PlayerWinState(this, stateMachine);
        stateMachine.Initialize(playerIdleState);

        _instance = this;
    }
    void FixedUpdate()
    {
        thrusterRechargeCounter++;
        if (thrusterRechargeCounter > thrusterRechargeDelay)
        {
            Fuel += thrusterRechargeRate;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Shelf"))
        {
            Debug.Log("Shelf touch true");

            transform.SetParent(collision.transform, true);
        }
    }


    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Shelf"))
        {
            Debug.Log("Shelf touch false");

            transform.SetParent(null, true);
        }
    }

    private void OnDestroy()
    {
        _instance = null;
    }

    public void AddGroundCollider(Collider sourceObject, Collider other)
    {
        var tuple = GetCollisionId(sourceObject, other);

        if (!TouchingGround)
        {
            MOST_HapticFeedback.Generate(MOST_HapticFeedback.HapticTypes.SoftImpact);
        }

        if (!currentGroundColliders.Contains(tuple))
        {
            currentGroundColliders.Add(tuple);
        }
    }

    public void RemoveGroundCollider(Collider sourceObject, Collider other)
    {
        var tuple = GetCollisionId(sourceObject, other);

        if (currentGroundColliders.Contains(tuple))
        {
            currentGroundColliders.Remove(tuple);
        }
    }

    public void Jump()
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
        IsJumping = true;
    }

    private (int, int) GetCollisionId(Collider sourceObject, Collider other)
    {
        var sourceId = sourceObject.GetInstanceID();
        var otherId = other.GetInstanceID();

        return (sourceId, otherId);
    }

    public void PickUpBones(int count = 1)
    {
        BonesCollected += count;
        OnPickupCollected.Invoke();
    }

    public void PickUpFoods(float treats, int count = 1)
    {
        FoodsCollected += count;

        tummy += treats;
        if (tummy > maxTummy)
        {
            tummy = maxTummy;
        }

        OnPickupCollected.Invoke();
    }

    public void PickUpFuel(float fuelAmount, int count = 1)
    {
        FuelsCollected += count;
        Fuel += fuelAmount;

        OnPickupCollected.Invoke();
    }

    public void PickUpBalls(int count = 1)
    {
        BallsCollected += count;
        BallTouch = true;

        OnPickupCollected.Invoke();
    }

    public void AddEnemiesDefeated(int count = 1)
    {
        EnemiesDefeated += count;
        OnPickupCollected.Invoke();
    }

    #region DataStuff
    void ApplyStoreUpgrades()
    {
        // Level 1 = base thrust (25), Level 2 = base + 1 (26), etc.
        // So we subtract 1 from the upgrade level since level 1 is the starting level
        baseThrustWithUpgrades = baseThrust + (collectibleData.thrustUpgradeLevel - 1);
        thrust = baseThrustWithUpgrades; // Initialize thrust to base upgraded value
        maxFuel = collectibleData.fuelUpgradeLevel * 20.0f;
        Fuel = maxFuel;
        fuelPercent = Fuel / maxFuel;
        maxTummy = collectibleData.treatsUpgradeLevel;
        tummy = maxTummy;
        tummyPercent = tummy / maxTummy;
        if (collectibleData.HASBALL)
        {
            hasPermaBall = true;
        }
    }
    public void ResetRechargeCounter()
    {
        thrusterRechargeCounter = 0;
    }
    #endregion
    
}