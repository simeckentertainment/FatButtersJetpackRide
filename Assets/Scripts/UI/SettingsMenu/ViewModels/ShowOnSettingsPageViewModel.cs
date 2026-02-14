using UnityEngine;

public class ShowOnSettingsPageViewModel : HideableViewModel<SettingsMenuModel>
{
    [SerializeField] private SettingsPage page;
    [SerializeField] private bool visibilityInverted = false;

    protected override bool IsVisible()
    {
        return visibilityInverted ?
            Model.CurrentPage != page :
            Model.CurrentPage == page;
    }
}
