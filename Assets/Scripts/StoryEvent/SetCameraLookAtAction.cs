using Unity.Cinemachine;
using UnityEngine;

public class SetCameraLookAtAction : StoryActionBase
{
    [SerializeField] private CinemachineCamera camera;
    [SerializeField] private Transform target;

    public override void Execute(StoryStepContext context)
    {
        if (camera == null) return;
        camera.LookAt = target;
    }
}
