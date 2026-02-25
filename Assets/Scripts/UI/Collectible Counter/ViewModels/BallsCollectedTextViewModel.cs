public class BallsCollectedTextViewModel : TextViewModel<CollectibleCounterModel>
{
    protected override string GetText()
    {
        return $"0 / { Model.TotalBalls }";
    }
}
