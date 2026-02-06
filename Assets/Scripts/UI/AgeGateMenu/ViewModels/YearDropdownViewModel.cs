using System;
using System.Collections.Generic;
using UnityEngine;

public class YearDropdownViewModel : DropdownViewModel<AgeGateMenuModel>
{
    [SerializeField] private int yearsToGoBack = 100;

    private void Start()
    {
        Dropdown.ClearOptions();
        var yearOptions = new List<string>();
        yearOptions.Add("Year");
        for (int i = 0; i < yearsToGoBack; i++)
        {
            yearOptions.Add((DateTime.Now.Year - i).ToString());
        }

        Dropdown.AddOptions(yearOptions);
    }

    protected override void OnDropdownChanged(int index)
    {
        Model.Year = int.Parse(Dropdown.options[index].text);
    }
}