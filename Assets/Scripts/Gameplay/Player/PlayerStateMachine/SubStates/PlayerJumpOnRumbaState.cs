using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerJumpOnRumbaState : PlayerAliveState
{
    public PlayerJumpOnRumbaState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine){

    }

    public override void enter()
    {
        PlayAnim("LaunchFromRumba");
        base.enter();
    }

    public override void Update(){
        if(player.anim.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1.0f){
            player.stateMachine.changeState(player.playerThrustState);
        }
        base.Update();
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
