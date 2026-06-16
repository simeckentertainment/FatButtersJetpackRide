public class UnlockThrustAction : StoryActionBase
{
    public override void Execute(StoryStepContext context)
    {
        if (context?.Player == null) return;
        context.Player.InJetpackKillZone = false;
    }
}
