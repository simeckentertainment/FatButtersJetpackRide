using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLevelLoseState : PlayerState
{
    public PlayerLevelLoseState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine){

    }
    public string UIText;
    public override void enter(){
        //player.UI.PauseGame();
        player.input.DisableInput();
        player.saveManager.collectibleData.HASBALL = false;
        PlayOneTimeAudio(player.vfx.deathSound);
        player.sfx.Play();
        player.vfx.StopPrimaryThrusters();
        player.vfx.StopRocketSounds();
        player.UI.ActivateFailMenu();
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
