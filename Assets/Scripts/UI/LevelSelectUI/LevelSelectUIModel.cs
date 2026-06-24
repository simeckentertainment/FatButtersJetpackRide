using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectUIModel : Model
{
    [SerializeField] private LevelSelectScroller levelSelectScroller;
    [SerializeField] private List<LevelButtonIDHolder> levelButtons;
    [SerializeField] private LevelSelectButtonViewModel levelButtonViewModelPrefab;
    [SerializeField] private Selectable levelSelectButtonUpSelect;
    [SerializeField] private Selectable levelSelectButtonDownSelect;
    [SerializeField] private Camera cam;
    [SerializeField] private Transform uiButtonHolder;

    private List<LevelSelectButtonViewModel> buttonViewModels = new List<LevelSelectButtonViewModel>();

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

    private void Awake()
    {
        foreach (var levelButton in levelButtons)
        {
            var newViewModel = Instantiate(levelButtonViewModelPrefab, uiButtonHolder);
            newViewModel.levelId = levelButton;
            newViewModel.cam = cam;
            newViewModel.upSelect = levelSelectButtonUpSelect;
            newViewModel.downSelect = levelSelectButtonDownSelect;

            buttonViewModels.Add(newViewModel);
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

    public LevelSelectButtonViewModel GetLevelSelectButtonViewModelWithId(int id)
    {
        foreach (var buttonViewModel in buttonViewModels)
        {
            if (buttonViewModel.levelId.levelID == id)
            {
                return buttonViewModel;
            }
        }

        return null;
    }

    public LevelSelectButtonViewModel GetMostCentralLevelSelectButton()
    {
        var minDistance = float.MaxValue;
        LevelSelectButtonViewModel minDistanceButton = null;
        foreach (var buttonViewModel in buttonViewModels)
        {
            var distance = Mathf.Abs(buttonViewModel.transform.position.x - transform.position.x);
            if (distance < minDistance)
            {
                minDistance = distance;
                minDistanceButton = buttonViewModel;
            }
        }

        return minDistanceButton;
    }
}

public enum LevelSelectUIState
{
    None,
    Base,
    Settings,
    Shop
}