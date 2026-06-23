using System.Collections.Generic;
using UnityEngine;

public class LevelSelectUIModel : Model
{
    [SerializeField] private LevelSelectScroller levelSelectScroller;
    [SerializeField] private List<LevelButtonIDHolder> levelButtons;
    [SerializeField] private LevelSelectButtonViewModel levelButtonViewModelPrefab;
    [SerializeField] private Camera cam;
    [SerializeField] private Transform uiButtonHolder;

    // initialize the UI state to none so that LevelSelectAssetVisibilityManager can enable UI elements when ready
    private LevelSelectUIState _uiState = LevelSelectUIState.None;
    public LevelSelectUIState UIState
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

    public float LevelSelectScrollValue
    {
        get
        {
            return levelSelectScroller.GetLeftRightScrollAmount();
        }
        set
        {
            levelSelectScroller.SetLeftRightScrollAmount(value);
            Refresh();
        }
    }

    private void Start()
    {
        foreach (var levelButton in levelButtons)
        {
            var newViewModel = Instantiate(levelButtonViewModelPrefab, uiButtonHolder);
            newViewModel.levelId = levelButton;
            newViewModel.cam = cam;
        }
    }

    public void GoToMainMenu()
    {
        Levels.Load(Levels.TitleScreen);
    }

    public void GoToLevel(int levelId)
    {
        Levels.Load(levelId);
    }

    public void ScrollToLevel(LevelButtonIDHolder levelId)
    {
        var rightBound = levelSelectScroller.RightBound.position.x;
        var leftBound = levelSelectScroller.LeftBound.position.x;
        var targetPosition = levelId.transform.position.x;
        if (targetPosition > rightBound)
        {
            targetPosition = rightBound;
        }
        if (targetPosition < leftBound)
        {
            targetPosition = leftBound;
        }

        var total = rightBound - leftBound;
        var progress = targetPosition - leftBound;

        LevelSelectScrollValue = progress / total;
    }
}

public enum LevelSelectUIState
{
    None,
    Base,
    Settings,
    Shop
}