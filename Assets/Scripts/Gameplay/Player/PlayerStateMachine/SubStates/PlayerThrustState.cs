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
        if (!player.input.GoThrust)
        {
            player.stateMachine.changeState(player.playerFallState);
        }
        stateAge = 0;
        thrusterVolumeCounter = 0f;
        if (player.GroundTouch)
        {
            PlayAnim("launch");
        }
        else
        {
            PlayAnim("midAirLaunch");
        }
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
        
        if (stateAge == 3)
        {
            if (player.input.GoThrust)
            {
                player.vfx.StartPrimaryThrusters();
                thrust();
                UseFuel();
            }
        }
        if (stateAge > 3)
        {
            if (player.input.GoThrust)
            {
                thrust();
                UseFuel();
            }
        }
        if (!player.input.GoThrust)
        {
            player.stateMachine.changeState(player.playerFallState);
        }
        if (player.fuel <= 0.0f)
        {
            player.stateMachine.changeState(player.playerNoFuelState);
        }
        if (stateAge == 60)
        {
            PlayAnim("AirIdle");
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
        base.exit();
    }
    
}
