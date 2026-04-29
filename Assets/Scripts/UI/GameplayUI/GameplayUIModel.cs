using UnityEngine;

public class GameplayUIModel : Model
{
    [SerializeField] private Player player;
    [SerializeField] private float thrustWarningActivationDelay = 0.3f;

    private float currentThrustDuration;

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
