using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerJumpState : PlayerAliveState
{
    public PlayerJumpState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
    {

    }
    public override void enter()
    {
        PlayAnim("JumpPrep");
        base.enter();
    }

    public override void Update()
    {
        Debug.Log(GetCurrentAnimName());
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
