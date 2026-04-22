using Solo.MOST_IN_ONE;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHurtState : PlayerAliveState
{
    public PlayerHurtState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
    {
    }

    string[] hurtAnims = { "hurt1","hurt2" };
    int stateAge;

    public override void enter(){
        stateAge = 0;
        player.tummy -= player.HarmfulDamageAmount;

        MOST_HapticFeedback.Generate(MOST_HapticFeedback.HapticTypes.HeavyImpact);

        if (player.tummy <= 0.0f){
            player.stateMachine.changeState(player.playerTummyDeathState);
        }
        player.UI.ActivateHurt();

        PlayAnim(hurtAnims[Random.Range(0, 2)]);
        player.ResetRechargeCounter();
        base.enter();
    }

    public override void FixedUpdate()
    {
        player.rb.AddExplosionForce(100.0f,player.HarmfulTouchObjectPosition,10.0f,10.0f,ForceMode.Force);
        stateAge++;
        if(GetNormalizedTime()>= 0.85f){
            player.stateMachine.changeState(player.playerFallState);
        }
        
        base.FixedUpdate();
    }

    public override void exit(){
        player.HarmfulTouch = false;
    }
}
