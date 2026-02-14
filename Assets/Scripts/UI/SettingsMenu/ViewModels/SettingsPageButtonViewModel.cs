using UnityEngine;

public class SettingsPageButtonViewModel : ButtonViewModel<SettingsMenuModel>
{
    [SerializeField] private SettingsPage page;

    protected override void OnClick()
    {
        Model.CurrentPage = page;
    }
}