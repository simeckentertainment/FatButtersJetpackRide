using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerJumpPrepState : PlayerJumpCommonState
{
    public PlayerJumpPrepState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
    {

    }
    public override void enter()
    {
        player.crm.DisableWobble();
        PlayAnim("JumpPrep");
        base.enter();
    }

    public override void Update()
    {
        if(GetNormalizedTime() >= 0.95f)
        {
            player.stateMachine.changeState(player.playerJumpStartState);
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
