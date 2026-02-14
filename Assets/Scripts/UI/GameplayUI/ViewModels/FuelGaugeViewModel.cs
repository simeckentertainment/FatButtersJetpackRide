using UnityEngine;
using UnityEngine.UI;

public class FuelGaugeViewModel : ImageViewModel<GameplayUIModel>
{
    [SerializeField] private Sprite[] FuelGaugeColors;
    [SerializeField] private float colorPingPongLength = 0.3f;
   //[SerializeField] Image barImage;

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
        // visual indicator on the fuel gauge when player trigger ball
        // just comment out the below code if you want to see the magic.
        
        
        // if (player.hasTemporaryBall || player.hasPermaBall)
        // {
        //     Image.color = Color.Lerp(Color.white, Color.black, Mathf.PingPong(Time.time, colorPingPongLength)); // color PingPong the inner bar
        //     barImage.color = Color.Lerp(Color.white, Color.black, Mathf.PingPong(Time.time, colorPingPongLength)); // color PingPong the outer bar 
             /* Comment out both line of code to ping pong both inner and outer bar. */
        // }  
       
        }
    }
}
