public class HurtIndicatorHideableViewModel : HideableViewModel<GameplayUIModel>
{
    protected override bool IsVisible()
    {
        return Model.IsRunningHurt;
    }
}
