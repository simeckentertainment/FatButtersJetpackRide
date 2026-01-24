using UnityEngine;

public class ShowOnSettingsPageViewModel : HideableViewModel<SettingsMenuModel>
{
    [SerializeField] private SettingsPage page;

    protected override bool IsVisible()
    {
        return Model.CurrentPage == page;
    }
}
