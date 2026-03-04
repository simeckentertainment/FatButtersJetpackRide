public class ShowWhenAllCollectiblesAreCollectedViewModel : HideableViewModel<CollectibleCounterModel>
{
    protected override bool IsVisible()
    {
        return Model.AllCollectiblesCollected;
    }
}
