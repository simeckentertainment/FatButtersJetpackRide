public class BorderGlowColorImageViewModel : ImageViewModel<GameplayUIModel>
{
    protected override void OnModelChanged()
    {
        base.OnModelChanged();
        Image.color = Model.GlowColor;
    }
}
