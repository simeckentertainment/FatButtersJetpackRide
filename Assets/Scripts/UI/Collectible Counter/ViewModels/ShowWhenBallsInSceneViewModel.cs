public class ShowWhenBallsInSceneViewModel : HideableViewModel<CollectibleCounterModel>
{
    protected override bool IsVisible()
    {
        return Model.TotalBalls > 0;
    }
}
