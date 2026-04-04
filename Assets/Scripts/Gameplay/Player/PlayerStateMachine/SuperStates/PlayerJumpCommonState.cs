using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerJumpCommonState : PlayerAliveState
{
    public PlayerJumpCommonState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
    {

    }

    public override void enter()
    {
        base.enter();
    }

    public override void Update()
    {
        base.Update();
        if (player.HarmfulTouch)
        {
            player.stateMachine.changeState(player.playerHurtState);
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
