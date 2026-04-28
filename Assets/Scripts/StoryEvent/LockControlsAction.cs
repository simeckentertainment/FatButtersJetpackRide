using UnityEngine;

public class LockControlsAction : StoryActionBase
{
    public override void Execute(StoryStepContext context)
    {
        if (context?.Player?.input == null) return;
        context.Player.input.DisableInput();
    }
}