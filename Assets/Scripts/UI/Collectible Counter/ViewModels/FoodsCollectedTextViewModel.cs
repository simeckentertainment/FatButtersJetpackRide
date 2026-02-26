public class FoodsCollectedTextViewModel : TextViewModel<CollectibleCounterModel>
{
    protected override string GetText()
    {
        return $"{ Model.FoodsCollected } / { Model.TotalFoods }";
    }
}
