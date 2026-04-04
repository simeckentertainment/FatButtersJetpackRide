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

        if (player.input.GoThrust)
        {
            player.stateMachine.changeState(player.playerThrustState);
        }
        base.Update();
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
