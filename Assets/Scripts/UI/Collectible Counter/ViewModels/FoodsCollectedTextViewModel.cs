public class FoodsCollectedTextViewModel : TextViewModel<CollectibleCounterModel>
{
    protected override string GetText()
    {
        return $"0 / { Model.TotalFoods }";
    }
}
