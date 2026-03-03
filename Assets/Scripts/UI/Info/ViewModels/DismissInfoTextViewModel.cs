public class DismissInfoTextViewModel : ButtonViewModel<InfoModel>
{
    protected override void OnClick()
    {
        Model.DismissInfoText();
    }
}
