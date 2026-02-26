using UnityEngine;

public class SubmitButtonViewModel : ButtonViewModel<AgeGateMenuModel>
{
    protected override bool IsEnabled()
    {
        return Model.Year > 0 && Model.Month > 0 && Model.Day > 0;
    }

    protected override void OnClick()
    {
        Model.Submit();
    }
}
