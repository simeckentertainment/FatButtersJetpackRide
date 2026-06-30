using UnityEngine.UI;

public class LevelSelectSliderViewModel : SliderViewModel<LevelSelectUIModel>
{
    private bool needsRefreshNavigation = false;

    protected override void OnModelChanged()
    {
        base.OnModelChanged();
        Slider.value = Model.LevelSelectScrollValue;

        needsRefreshNavigation = true; // delay the refresh to ensure the level select buttons have been moved
    }

    protected override void OnSliderChanged(float value)
    {
        Model.LevelSelectScrollValue = value;
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

        newNavigation.selectOnRight = Slider.navigation.selectOnRight;
        newNavigation.selectOnLeft = Slider.navigation.selectOnLeft;
        newNavigation.selectOnUp = Model.GetMostCentralLevelSelectButton().GetButton();
        newNavigation.selectOnDown = Slider.navigation.selectOnUp;

        Slider.navigation = newNavigation;
    }
}
