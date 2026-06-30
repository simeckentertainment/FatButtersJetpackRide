using UnityEngine.UI;

public abstract class LevelSelectTopButtonViewModel : ButtonViewModel<LevelSelectUIModel>
{
    private bool needsRefreshNavigation = false;

    protected override void OnModelChanged()
    {
        base.OnModelChanged();

        needsRefreshNavigation = true; // delay the refresh to ensure the level select buttons have been moved
    }

    private void LateUpdate()
    {
        if (needsRefreshNavigation)
        {
            needsRefreshNavigation = false;
            RefreshNavigation();
        }
    }

    private void RefreshNavigation()
    {
        var newNavigation = new Navigation();
        newNavigation.mode = Navigation.Mode.Explicit;

        newNavigation.selectOnRight = Button.navigation.selectOnRight;
        newNavigation.selectOnLeft = Button.navigation.selectOnLeft;
        newNavigation.selectOnUp = Button.navigation.selectOnUp;
        newNavigation.selectOnDown = Model.GetMostCentralLevelSelectButton().GetButton();

        Button.navigation = newNavigation;
    }
}
