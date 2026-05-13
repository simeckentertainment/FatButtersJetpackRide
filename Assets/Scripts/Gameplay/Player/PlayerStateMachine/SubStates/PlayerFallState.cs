using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallState : PlayerAliveState
{
    private float stateAge;
    private float VolumeReductionThreshold;

    public PlayerFallState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
    {
    }

    public override void enter()
    {
        stateAge = 0;
        VolumeReductionThreshold = 10;
        if (!player.IsJumping)
        {
            PlayAnim("fall");
            // if jumping, it will automatically transition to the JumpAir animation
        }
        DeActivateGravyBoat();
        base.enter();
    }

    public override void FixedUpdate()
    {
        stateAge++;
        player.ResetRechargeCounter();

        //Calm the sound the fuck down so we don't blow people's ears out.
        player.sfx.volume = Mathf.Clamp((VolumeReductionThreshold-stateAge)/VolumeReductionThreshold,0f,1f);
        if ((stateAge > VolumeReductionThreshold) & player.sfx.isPlaying)
        {
            player.sfx.Stop();
        }
        if (stateAge > 2 && player.TouchingGround)
        {
            if (player.IsJumping)
            {
                PlayAnim("JumpLand");
                player.IsJumping = false;
            }
            else
            {
                PlayAnim("Land");
            }
            
            player.stateMachine.changeState(player.playerIdleState);
        }
        if(player.input.GoThrust && player.JetpackActivationPossible)
        {
            player.stateMachine.changeState(player.playerThrustState);
        }
        if (stateAge == 120)
        {
            PlayAnim("fallIdle");
            player.IsJumping = false;
        }
        if (stateAge == 360)
        {
            player.stateMachine.changeState(player.playerEnterDangleState);
        }

        base.FixedUpdate();
    }

    public override void exit()
    {
        base.exit();
    }
}
