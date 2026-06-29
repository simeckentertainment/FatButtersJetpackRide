using UnityEngine;

public class LockControlsAction : StoryActionBase
{
    public override void Execute(StoryStepContext context)
    {
        var player = context?.Player;
        if (player == null) return;

        if (player.input != null)
        {
            player.input.GoThrust = false;
            player.input.GoCw = false;
            player.input.GoCcw = false;
            player.input.GoBoost = false;
            player.input.aimAngle = 0f;
            player.input.DisableInput();
        }

        if (player.rb != null)
        {
            player.rb.linearVelocity = Vector3.zero;
            player.rb.angularVelocity = Vector3.zero;
        }

        if (player.stateMachine != null && player.playerIdleState != null)
            player.stateMachine.changeState(player.playerIdleState);
    }
}