using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameplayUIModel gameplayUI;
    [SerializeField] private FailMenuModel failMenu;
    [SerializeField] private SuccessMenuModel successMenu;
    [SerializeField] private InfoModel infoMenu;

    public FailReason FailReason { get; set; }

    private GameplayUIState CurrentState
    {
        get 
        {
            return gameplayUI.UIState;
        }
        set
        {
            gameplayUI.UIState = value;
        }
    }

    public void ActivateHurt()
    {
        gameplayUI.IsRunningHurt = true;
    }

    public void SetEndLevelStats(int newbones)
    {
        successMenu.NewBones = newbones;
    }

    public void ActivateWinMenu()
    {
        CurrentState = GameplayUIState.Success;
    }

    public void ActivateFailMenu()
    {
        failMenu.FailReason = FailReason;
        CurrentState = GameplayUIState.Fail;
    }

    public void ShowInfoText(string title, string text)
    {
        CurrentState = GameplayUIState.Info;
        infoMenu.SetText(title, text);
        PauseUtility.Pause();
    }

    public void DismissInfoText()
    {
        CurrentState = GameplayUIState.Base;
        PauseUtility.Resume();
    }
}
