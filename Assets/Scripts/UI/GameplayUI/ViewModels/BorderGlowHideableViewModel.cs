public class BorderGlowHideableViewModel : HideableViewModel<GameplayUIModel>
{
    protected override bool IsVisible()
    {
        return Model.BallActive && Model.UIState == GameplayUIState.Base;
    }
}
