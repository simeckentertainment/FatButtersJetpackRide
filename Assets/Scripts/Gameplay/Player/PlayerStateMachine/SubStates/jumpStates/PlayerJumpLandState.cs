using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerJumpLandState : PlayerJumpCommonState
{
    public PlayerJumpLandState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
    {

    }

    public override void enter()
    {
        PlayAnim("JumpLand");
        base.enter();
    }

    public override void Update()
    {

        if(GetNormalizedTime() >= 0.95f && player.input.aimAngle < 15.0f)
        {
            player.stateMachine.changeState(player.playerIdleState);
        } else
        {
            player.stateMachine.changeState(player.playerWalkState);
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
