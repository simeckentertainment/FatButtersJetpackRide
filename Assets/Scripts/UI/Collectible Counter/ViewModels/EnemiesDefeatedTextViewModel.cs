public class EnemiesDefeatedTextViewModel : TextViewModel<CollectibleCounterModel>
{
    protected override string GetText()
    {
        return $"{ Model.EnemiesDefeated } / { Model.TotalEnemies }";
    }
}
