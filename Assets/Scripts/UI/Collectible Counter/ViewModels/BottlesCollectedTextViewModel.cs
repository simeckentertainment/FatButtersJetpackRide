public class BottlesCollectedTextViewModel : TextViewModel<CollectibleCounterModel>
{
    protected override string GetText()
    {
        return $"{ Model.FuelsCollected } / { Model.TotalFuels }";
    }
}
