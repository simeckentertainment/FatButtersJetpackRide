using UnityEngine;

public class RequestGameplayStateAction : StoryActionBase
{
    [SerializeField] private StoryMode mode = StoryMode.Gameplay;

    public override void Execute(StoryStepContext context)
    {
        var activeGameplayBridge = context?.Controller?.GetGameplayBridge();
        if (activeGameplayBridge != null)
            activeGameplayBridge.ApplyMode(mode);
    }
}