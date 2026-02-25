public class BonesCollectedTextViewModel : TextViewModel<CollectibleCounterModel>
{
    protected override string GetText()
    {
        return $"{ Model.CurrentBones } / { Model.TotalBones }";
    }
}
