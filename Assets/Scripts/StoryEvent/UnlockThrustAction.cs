using UnityEngine;

public class UnlockThrustAction : StoryActionBase
{
    private const string TokenName = "[StoryThrustLock]";

    public override void Execute(StoryStepContext context)
    {
        if (context?.Player == null) return;

        for (int i = context.Player.CollidersInJetpackKillZone.Count - 1; i >= 0; i--)
        {
            var c = context.Player.CollidersInJetpackKillZone[i];
            if (c == null || c.gameObject.name == TokenName)
                context.Player.CollidersInJetpackKillZone.RemoveAt(i);
        }
    }
}
