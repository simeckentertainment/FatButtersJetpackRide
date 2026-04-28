using UnityEngine;

public class UnlockControlsAction : StoryActionBase
{
    public override void Execute(StoryStepContext context)
    {
        if (context?.Player?.input == null) return;
        context.Player.input.EnableInput();
    }
}