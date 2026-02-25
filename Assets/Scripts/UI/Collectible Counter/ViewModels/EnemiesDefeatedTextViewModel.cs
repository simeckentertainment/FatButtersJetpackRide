public class EnemiesDefeatedTextViewModel : TextViewModel<CollectibleCounterModel>
{
    protected override string GetText()
    {
        return $"0 / { Model.TotalEnemies }";
    }
}
