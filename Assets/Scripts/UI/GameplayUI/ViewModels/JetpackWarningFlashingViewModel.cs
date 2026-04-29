using UnityEngine;

public class JetpackWarningFlashingViewModel : ImageViewModel<GameplayUIModel>
{
    [SerializeField] private float colorPingPongLength = 0.3f;

    private Color clearWhite = new Color(1, 1, 1, 0);

    private void Update()
    {
        if (Model.PlayerHasBeenUsingJetpack)
        {
            Image.color = Color.Lerp(Color.white, clearWhite, Mathf.PingPong(Time.unscaledTime, colorPingPongLength)); // color PingPong
        }
        else
        {
            Image.color = clearWhite;
        }
    }
}
