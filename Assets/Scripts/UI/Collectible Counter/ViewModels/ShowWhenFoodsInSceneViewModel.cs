public class ShowWhenFoodsInSceneViewModel : HideableViewModel<CollectibleCounterModel>
{
    protected override bool IsVisible()
    {
        return Model.TotalFoods > 0;
    }
}
