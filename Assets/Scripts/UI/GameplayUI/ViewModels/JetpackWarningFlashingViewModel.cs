using UnityEngine;

public class JetpackWarningFlashingViewModel : ImageViewModel<GameplayUIModel>
{
    [SerializeField] private float colorPingPongLength = 0.3f;

    private void FixedUpdate()
    {
        var clearWhite = new Color(1, 1, 1, 0);
        if (Model.PlayerIsUsingJetpack)
        {
            Image.color = Color.Lerp(Color.white, clearWhite, Mathf.PingPong(Time.time, colorPingPongLength)); // color PingPong
        }
        else
        {
            Image.color = clearWhite;
        }
    }
}
