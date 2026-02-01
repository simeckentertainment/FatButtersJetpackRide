using UnityEngine;

public class FramerateManager : Singleton<FramerateManager>
{
    public const int TargetFramerate = 60;

    protected override void Awake()
    {
        base.Awake();

        if (!markedToDestroy)
        {
            Application.targetFrameRate = TargetFramerate;
            Debug.Log($"Frame rate set to {TargetFramerate} FPS");
        }
    }
}
