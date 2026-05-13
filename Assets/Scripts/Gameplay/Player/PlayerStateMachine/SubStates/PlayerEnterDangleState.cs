using UnityEngine;

public class PlayerEnterDangleState : PlayerAliveState
{
    private float stateAge;
    private float VolumeReductionThreshold;
    public PlayerEnterDangleState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
    {
    }

    public override void enter()
    {
        PlayAnim("enterFallDangle");
        base.enter();
    }

    public override void FixedUpdate()
    {
        stateAge++;
        player.ResetRechargeCounter();

        //Calm the sound the fuck down so we don't blow people's ears out.
        player.sfx.volume = Mathf.Clamp((VolumeReductionThreshold-stateAge)/VolumeReductionThreshold,0f,1f);
        if ((stateAge > VolumeReductionThreshold) & player.sfx.isPlaying){player.sfx.Stop();}
        if (player.IsGrounded)
        {
            PlayAnim("Land");
            player.stateMachine.changeState(player.playerIdleState);
        }
        if (player.input.GoThrust&& player.JetpackActivationPossible)
        {
            player.stateMachine.changeState(player.playerThrustState);
        }
        if (GetNormalizedTime() >= 0.95f)
        {
            player.stateMachine.changeState(player.playerDangleState);
        }
        base.FixedUpdate();
    }

    public override void exit()
    {
        base.exit();
    }
}
