using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTummyDeathState : PlayerLevelLoseState
{
    public PlayerTummyDeathState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine){

    }

    public override void enter(){
        player.UI.FailText.text = "Your tummy is empty!\nTry upgrading your tummy in the store!";
        base.enter();
    }

    public override void Update(){
        base.Update();
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
