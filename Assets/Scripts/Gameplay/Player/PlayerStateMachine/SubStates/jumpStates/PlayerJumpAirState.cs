using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerJumpAirState : PlayerJumpCommonState
{
    public PlayerJumpAirState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
    {

    }
    public override void enter()
    {
        PlayAnim("JumpAir");
        base.enter();
    }

    public override void Update()
    {
        if (player.IsGrounded && durationOfState >= 10)
        {
            player.crm.EnableWobble();
            player.stateMachine.changeState(player.playerJumpLandState);
        }
        base.Update();

        if (!player.JetpackActivationPossible)
        {
            return;
        }
        if (player.input.GoThrust || player.input.GoJump) //Double jumping is just the thruster.
        {
            player.stateMachine.changeState(player.playerThrustState);
        }

    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
    public override void exit()
    {

        base.exit();
    }
    
}
