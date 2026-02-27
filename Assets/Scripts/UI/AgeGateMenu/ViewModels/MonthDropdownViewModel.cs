public class MonthDropdownViewModel : DropdownViewModel<AgeGateMenuModel>
{
    protected override void OnDropdownChanged(int index)
    {
        Model.Month = index;
    }
}
