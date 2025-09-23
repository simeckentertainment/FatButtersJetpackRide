using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerOHKState : PlayerLevelLoseState
{
    public PlayerOHKState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine){

    }

    public override void enter(){
        
        //Old skin-setting code.
        //player.vfx.Skins[skindex].SetActive(false);
        player.UI.FailText.text = "You are no more.\nMaybe don't touch that again!";
        MonoBehaviour.Destroy(player.GetComponent<Rigidbody>());
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
