using UnityEngine;
using UnityEngine.UI;

public class FuelGaugeViewModel : GlowColorImageViewModel
{
    [SerializeField] private Color fullColor;
    [SerializeField] private Color halfColor;
    [SerializeField] private Color quarterColor;
    [SerializeField] Image shineImage;
    [SerializeField] private float colorPingPongLength = 0.3f;

    protected override void OnModelChanged()
    {
        Image.fillAmount =  Model.FuelPercent;
        shineImage.fillAmount =  Model.FuelPercent;

        if (Model.FuelPercent > 0.5f)
        {
            defaultColor = fullColor;
        }
        else if (Model.FuelPercent <= 0.5f & Model.FuelPercent > 0.25f)
        {
            defaultColor = halfColor;
        }
        else
        {
            defaultColor = quarterColor;
        }

        base.OnModelChanged();
    }

    private void Update()
    {
        if (Model.FuelPercent < 0.25f && ! Model.BallActive)
        {
            Image.color = Color.Lerp(defaultColor, Color.black, Mathf.PingPong(Time.unscaledTime, colorPingPongLength)); // color PingPong
        }
    }
}
