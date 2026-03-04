public class InfoTitleTextViewModel : TextViewModel<InfoModel>
{
    protected override string GetText()
    {
        return Model.InfoTitle;
    }
}
