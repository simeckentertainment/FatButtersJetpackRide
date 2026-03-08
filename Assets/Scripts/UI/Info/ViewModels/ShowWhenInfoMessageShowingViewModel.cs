public class ShowWhenInfoMessageShowingViewModel : HideableViewModel<InfoModel>
{
    protected override bool IsVisible()
    {
        return Model.ShowingInfo;
    }
}
