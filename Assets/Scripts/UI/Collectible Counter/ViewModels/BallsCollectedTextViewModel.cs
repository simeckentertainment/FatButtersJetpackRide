public class BallsCollectedTextViewModel : TextViewModel<CollectibleCounterModel>
{
    protected override string GetText()
    {
        return $"{ Model.BallsCollected } / { Model.TotalBalls }";
    }
}
