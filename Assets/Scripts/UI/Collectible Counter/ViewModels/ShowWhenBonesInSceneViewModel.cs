public class ShowWhenBonesInSceneViewModel : HideableViewModel<CollectibleCounterModel>
{
    protected override bool IsVisible()
    {
        return Model.TotalBones > 0;
    }
}
