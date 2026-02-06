using UnityEngine;

public class FuelGaugeViewModel : ImageViewModel<GameplayUIModel>
{
    [SerializeField] private Sprite[] FuelGaugeColors;
    [SerializeField] private float colorPingPongLength = 0.3f;

    protected override void OnModelChanged()
    {
        Image.fillAmount =  Model.FuelPercent;
        base.OnModelChanged();
    }

    protected override Sprite GetSprite()
    {
        if (Model.FuelPercent > 0.5f)
        {
            Image.color = Color.white;
            return FuelGaugeColors[0];
        }
        else if (Model.FuelPercent <= 0.5f & Model.FuelPercent > 0.25f)
        {
            Image.color = Color.white;
            return FuelGaugeColors[1];
        }
        else
        {
            return FuelGaugeColors[2];
        }
    }

    private void Update()
    {
        if (Model.FuelPercent < 0.25f)
        {
            Image.color = Color.Lerp(Color.white, Color.black, Mathf.PingPong(Time.time, colorPingPongLength)); // color PingPong
        }
    }
}
