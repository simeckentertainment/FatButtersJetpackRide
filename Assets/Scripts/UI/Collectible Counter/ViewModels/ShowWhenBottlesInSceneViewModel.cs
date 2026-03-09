public class ShowWhenBottlesInSceneViewModel : HideableViewModel<CollectibleCounterModel>
{
    protected override bool IsVisible()
    {
        return Model.TotalFuels > 0;
    }
}
