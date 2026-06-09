public class LevelSelectSliderViewModel : SliderViewModel<LevelSelectUIModel>
{
    protected override void OnModelChanged()
    {
        base.OnModelChanged();
        Slider.value = Model.LevelSelectScrollValue;
    }

    protected override void OnSliderChanged(float value)
    {
        Model.LevelSelectScrollValue = value;
    }
}
