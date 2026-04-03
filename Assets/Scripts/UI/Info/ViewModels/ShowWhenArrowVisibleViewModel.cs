public class ShowWhenArrowVisibleViewModel : HideableViewModel<InfoModel>
{
    protected override bool IsVisible()
    {
        return Model.ShowArrow;
    }
}
