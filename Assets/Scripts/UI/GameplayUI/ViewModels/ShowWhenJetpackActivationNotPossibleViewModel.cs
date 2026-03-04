public class ShowWhenJetpackActivationNotPossibleViewModel : HideableViewModel<GameplayUIModel>
{
    protected override bool IsVisible()
    {
        return !Model.PlayerCanUseJetpack;
    }
}
