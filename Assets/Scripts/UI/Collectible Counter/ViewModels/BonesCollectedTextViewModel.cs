public class BonesCollectedTextViewModel : TextViewModel<CollectibleCounterModel>
{
    protected override string GetText()
    {
        return $"{ Model.BonesCollected } / { Model.TotalBones }";
    }
}
