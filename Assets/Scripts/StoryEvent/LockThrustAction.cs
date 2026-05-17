public class LockThrustAction : StoryActionBase
{
    public override void Execute(StoryStepContext context)
    {
        if (context?.Player == null) return;
        context.Player.InJetpackKillZone = true;
    }
}
