using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNoFuelState : PlayerLevelLoseState
{
    public PlayerNoFuelState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine){

    }

    public override void enter(){
        //Social.ReportProgress(GPGSIds.achievement_oh_my_tummy, 100.0f, (bool success) => {Debug.Log("Tummy's empty!");});
        if(player.corgiTurned){
            player.UI.FailText.text = "\nYour fuel tank is empty!\n Try upgrading your fuel tank in the store!";
        } else {
            player.UI.FailText.text = "\nDid you know that you can \n enable on-screen controls?\n Try it in the pause menu!";
            player.UI.savedBonesText.SetActive(false);
        }
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
