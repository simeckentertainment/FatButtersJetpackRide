using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerThrustState : PlayerAliveState
{
    public PlayerThrustState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
    {

    }
    int stateAge;
    public override void enter()
    {
        if (!player.input.GoThrust | !player.JetpackActivationPossible)
        {
            player.stateMachine.changeState(player.playerFallState);
        }
        stateAge = 0;
        thrusterVolumeCounter = 0f;
        if (player.IsGrounded)
        {
            PlayAnim("launch");
        }
        else
        {
            PlayAnim("midAirLaunch");
        }
        StartNewGrr();
        ActivateGravyBoat();
        base.enter();
    }

    public override void Update()
    {
        base.Update();
    }
    public override void FixedUpdate()
    {
        stateAge++;
        player.ResetRechargeCounter();
        // Handle boost logic within the state machine
        bool isBoosting = player.input.GoBoost && player.Fuel > 0f;
        
        // Apply boost thrust modifier (preserves upgrades)
        if (isBoosting)
        {
            player.thrust = player.baseThrustWithUpgrades + 12.5f;
        }
        else
        {
            player.thrust = player.baseThrustWithUpgrades;
        }
        
        if (stateAge == 3)
        {
            if (player.input.GoThrust)
            {
                player.vfx.StartPrimaryThrusters();
                thrust();
                UseFuel(isBoosting);
            }
        }
        if (stateAge > 3)
        {
            if (player.input.GoThrust)
            {
                thrust();
                UseFuel(isBoosting);
            }
        }

        player.SetFootCollisionEnabled(!player.IsGrounded);

        if (!player.input.GoThrust || !player.JetpackActivationPossible || player.Fuel < 0.0f)
        {
            player.stateMachine.changeState(player.playerFallState);
        }
        if (stateAge == 60)
        {
            PlayAnim("AirIdle");
        }
        if(GetGrrProgress() == 0.0f | GetGrrProgress() >= 1.0f)
        {
            StartNewGrr();
        }

        base.FixedUpdate();
    }
    public override void exit()
    {
        thrusterVolumeCounter = Mathf.Clamp(stateAge,0,30);
        if(player.anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1.0f){
        player.animationPercentage = 1.0f;
        } else {
        player.animationPercentage = GetNormalizedTime();
        }
        player.vfx.StopPrimaryThrusters();
        player.SetFootCollisionEnabled(true);
        base.exit();
    }
    
}
