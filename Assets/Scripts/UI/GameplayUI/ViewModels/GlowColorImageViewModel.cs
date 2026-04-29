using UnityEngine;

public class GlowColorImageViewModel : ImageViewModel<GameplayUIModel>
{
    [SerializeField] private Color defaultColor;
    [SerializeField] private float glowColorAlpha = 1;

    protected override void OnModelChanged()
    {
        base.OnModelChanged();
        if (Model.BallActive)
        {
            var color = Model.GlowColor;
            color.a = glowColorAlpha;
            Image.color = color;
        }
        else
        {
            Image.color = defaultColor;
        }
    }
}
