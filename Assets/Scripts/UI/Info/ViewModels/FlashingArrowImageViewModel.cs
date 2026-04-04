using UnityEngine;

public class FlashingArrowImageViewModel : ImageViewModel<InfoModel>
{
    [SerializeField] private float colorPingPongLength = 0.3f;

    protected override void OnModelChanged()
    {
        base.OnModelChanged();
        this.transform.UpdateFromEditorLocalTransform(Model.ArrowTransform);
    }

    private void Update()
    {
        Image.color = Color.Lerp(Color.white, Color.black, Mathf.PingPong(Time.unscaledTime, colorPingPongLength)); // color PingPong
    }
}
