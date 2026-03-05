public class ShowWhenBottlesInSceneViewModel : HideableViewModel<CollectibleCounterModel>
{
    protected override bool IsVisible()
    {
        return Model.TotalEnemies > 0;
    }
}
