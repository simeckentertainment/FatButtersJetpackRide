public class InfoTextViewModel : TextViewModel<InfoModel>
{
    protected override string GetText()
    {
        return Model.InfoText;
    }
}
