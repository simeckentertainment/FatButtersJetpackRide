using UnityEngine;

public class MoveWhenGameplayUIStateActiveViewModel : SinusoidalSlideableViewModel<GameplayUIModel>
{
    [SerializeField] private GameplayUIState uiState;

    protected override bool IsActive()
    {
        return uiState == Model.UIState;
    }
}