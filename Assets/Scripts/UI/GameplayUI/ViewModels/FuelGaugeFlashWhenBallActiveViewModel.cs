using UnityEngine;
using UnityEngine.UI;

public class FuelGaugeFlashWhenBallActiveViewModel : ImageViewModel<GameplayUIModel>
{
    [SerializeField] private float colorPingPongLength = 0.3f;

    private void Update()
    {
        if (Model.PlayerHasBall)
        {
            Image.color = Color.Lerp(Color.white, Color.black, Mathf.PingPong(Time.unscaledTime, colorPingPongLength)); // color PingPong
        }
    }
}
