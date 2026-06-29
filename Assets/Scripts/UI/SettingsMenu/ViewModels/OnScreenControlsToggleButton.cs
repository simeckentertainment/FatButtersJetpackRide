public class OnScreenControlsToggleButtonViewModel : ToggleButtonViewModel<SettingsMenuModel>
{
    public InputDriver input;
    protected override void OnModelChanged()
    {
        base.OnModelChanged();
        ToggleButton.isOn = Model.OnScreenControlsEnabled;
        input.ToggleOnScreenControls(ToggleButton.isOn);
    }

    protected override void OnToggleChanged(bool value)
    {
        Model.OnScreenControlsEnabled = value;
    }
}
