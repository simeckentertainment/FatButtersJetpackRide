using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWinState : PlayerLevelWinState
{
    public PlayerWinState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
    {

    }
    float rollStartingValue;
    float KeyboardOffsetStartingValue;
    int FixRollTimer;
    int FixRollTimerMax = 60;
    public override void enter()
    {
        player.saveManager.collectibleData.LevelBeaten[player.saveManager.sceneLoadData.LastLoadedLevelInt] = true;
        player.saveManager.collectibleData.HASBALL = false;
        FixRollTimer = 0;
        PlayOneTimeAudio(player.vfx.successSound);
        player.vfx.StopPrimaryThrusters();
        rollStartingValue = player.input.roll;
        KeyboardOffsetStartingValue = player.KeyboardRollOffset;
        player.UI.SetEndLevelStats(player.tempBones);
        player.saveManager.Save();
        player.input.DisableInput();
        player.UI.ActivateWinMenu();
        base.enter();
    }

    public override void Update()
    {
        base.Update();
    }
    public override void FixedUpdate()
    {
        if (FixRollTimer < FixRollTimerMax)
        {
            FixRollTimer++;
            player.input.roll = Mathf.Lerp(rollStartingValue, 0.0f, FixRollTimer / FixRollTimerMax);
            player.KeyboardRollOffset = Mathf.Lerp(KeyboardOffsetStartingValue, 0.0f, FixRollTimer / FixRollTimerMax);
        }
        base.FixedUpdate();
    }
}
