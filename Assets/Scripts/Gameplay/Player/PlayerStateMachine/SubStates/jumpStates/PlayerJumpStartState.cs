using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerJumpStartState : PlayerJumpCommonState
{
    public PlayerJumpStartState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
    {

    }

    public override void enter()
    {
        PlayAnim("JumpStart");
        player.rb.AddExplosionForce(750.0f, player.transform.position + new Vector3(-1.0f, -1.0f, 0.0f), 2.0f);
        base.enter();
    }

    public override void Update()
    {
        if (GetNormalizedTime() >= 0.95f)
        {
            player.stateMachine.changeState(player.playerJumpAirState);
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
