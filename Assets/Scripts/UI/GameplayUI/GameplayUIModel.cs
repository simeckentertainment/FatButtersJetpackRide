using UnityEngine;

public class GameplayUIModel : Model
{
    [SerializeField] private Player player;
    [SerializeField] private float thrustWarningActivationDelay = 0.3f;
    [SerializeField] private float showHurtDuration = 0.15f;
    [SerializeField] private float powerupGlowAlpha = 0.2f;
    [SerializeField] private float hurtGlowAlpha = 0.6f;

    private float currentThrustDuration;

    private float currentAlpha = 1;

    private float currentHurtDuration = 0;

    private CollectibleData collectibleData => SaveManager.Instance.collectibleData;

    private GameplayUIState _uiState;
    public GameplayUIState UIState
    {
        get 
        {
            return _uiState;
        }
        set
        {
            _uiState = value;
            Refresh();
        }
    }

    public bool GlowActive => IsRunningHurt || BallActive;

    private bool _isRunningHurt;
    public bool IsRunningHurt
    {
        get
        {
            return _isRunningHurt;
        }
        set
        {
            _isRunningHurt = value;
            if (_isRunningHurt)
            {
                currentHurtDuration = showHurtDuration;
                currentAlpha = hurtGlowAlpha;
                GlowColor = Color.red;
            }
            Refresh();
        }
    }

    private bool _ballActive;
    public bool BallActive
    {
        get
        {
            return _ballActive;
        }
        set
        {
            _ballActive = value;
            if (_ballActive)
            {
                currentAlpha = powerupGlowAlpha;
            }
            Refresh();
        }
    }

    private Color _glowColor;
    public Color GlowColor
    {
        get
        {
            return _glowColor;
        }
        set
        {
            _glowColor = value;
            _glowColor.a = currentAlpha;
            Refresh();
        }
    }

    public float FuelPercent => player.Fuel / player.maxFuel;

    public bool OnScreenControlsEnabled
    {
        get
        {
            // We'll refresh this each time we change UIState
            return collectibleData.OnScreenControlsEnabled;
        }
    }

    public bool CorgiSenseEnabled
    {
        get
        {
            // We'll refresh this each time we change UIState
            return collectibleData.CorgiSenseEnabled;
        }
    }

    public bool PlayerHasBall => player.hasPermaBall || player.hasTemporaryBall;

    public bool PlayerCanUseJetpack => player.JetpackActivationPossible;

    public bool PlayerIsUsingJetpack => player.input.GoThrust;

    public bool PlayerHasBeenUsingJetpack => PlayerIsUsingJetpack && currentThrustDuration > thrustWarningActivationDelay;

    private void Awake()
    {
        player.OnFuelUpdated.AddListener(Refresh);
        player.OnJetpackStatusUpdated.AddListener(Refresh);
    }

    private void FixedUpdate()
    {
        if (PlayerIsUsingJetpack)
        {
            currentThrustDuration += Time.deltaTime;
        }
        else
        {
            currentThrustDuration = 0;
        }
        if (IsRunningHurt)
        {
            currentHurtDuration -= Time.deltaTime;
            if (currentHurtDuration <= 0)
            {
                IsRunningHurt = false;
            }
        }
    }

    private void OnDestroy()
    {
        player.OnFuelUpdated.RemoveListener(Refresh);
        player.OnJetpackStatusUpdated.RemoveListener(Refresh);
    }

    public void SetPaused(bool paused)
    {
        if (paused)
        {
            UIState = GameplayUIState.Settings;
            PauseUtility.Pause();
        }
        else
        {
            UIState = GameplayUIState.Base;
            PauseUtility.Resume();
        }
    }
}
