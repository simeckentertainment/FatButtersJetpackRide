using TMPro;

public class DayDropdownViewModel : DropdownViewModel<AgeGateMenuModel>
{
    protected override void OnModelChanged()
    {
        base.OnModelChanged();

        var daysThisMonth = Model.GetDaysThisMonth(); 

        if (daysThisMonth < Dropdown.options.Count - 1)
        {
            var nextDayIndex = daysThisMonth + 1;
            Dropdown.options.RemoveRange(nextDayIndex, Dropdown.options.Count - nextDayIndex);
        }

        for (int i = 0; i <= daysThisMonth; i++)
        {
            if (Dropdown.options.Count == 0)
            {
                Dropdown.options.Add(new TMP_Dropdown.OptionData("Day"));
            }
            else if (Dropdown.options.Count <= i)
            {
                Dropdown.options.Add(new TMP_Dropdown.OptionData(i.ToString()));
            }
        }

        if (Dropdown.value > daysThisMonth)
        {
            Dropdown.value = daysThisMonth;
        }
    }

    protected override void OnDropdownChanged(int index)
    {
        Model.Day = index;
    }
}
