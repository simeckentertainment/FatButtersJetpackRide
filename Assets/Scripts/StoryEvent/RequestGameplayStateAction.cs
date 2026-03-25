using UnityEngine;

public class RequestGameplayStateAction : StoryActionBase
{
    [SerializeField] private StoryMode mode = StoryMode.Gameplay;
    [SerializeField] private StoryGameplayBridge gameplayBridge;

    public override void Execute(StoryStepContext context)
    {
        var b = gameplayBridge != null ? gameplayBridge : context?.Controller?.GetGameplayBridge();
        if (b != null)
            b.ApplyMode(mode);
    }
}