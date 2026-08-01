using UnityEngine;

public class PlayerThrustState : PlayerAliveState
{
    protected override float TurnDelay => 0.5f;

    public PlayerThrustState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
    {

    }
    float jumpTimerMax = 3f;
    int stateAge;
    public override void enter()
    {
        if (player.CanJump)
        {
            StartJumpSequence();
            player.input.DisableInput(); //The player can't stop a jump when winding up for a jump.
            PlayAnim("JumpStart");
        }

        if (!player.input.GoThrust || !player.JetpackActivationPossible)
        {
            player.stateMachine.changeState(player.playerFallState);
        }

        stateAge = 0;
        thrusterVolumeCounter = 0f;
        if (!player.IsGrounded)
        {
            PlayAnim("midAirLaunch");
            player.IsJumping = false;
        }
        StartNewGrr();
        ActivateGravyBoat(); //For the thanksgiving skin
        base.enter();
    }

    public override void Update()
    {
        base.Update();
    }
    public override void FixedUpdate()
    {
        stateAge++;
        if (player.IsJumping && stateAge == 10) //10 frames is the jump delay. If you want to change this number,
                                                //Be sure to adjust the animation accordingly.
        {
            player.input.EnableInput(); //re-enable the input after the jump takes off.
            RunJump();
        }
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
            }
        }
        if (stateAge >= 3)
        {
            if (player.input.GoThrust)
            {
                player.Thrust();
                UseFuel(isBoosting);
                GameplaySignal.Raise(SignalId.ThrustUsed);
            }
        }

        UpdateTargetRotation(360);

        if (!player.input.GoThrust || !player.JetpackActivationPossible || player.Fuel < 0.0f)
        {
            player.stateMachine.changeState(player.playerFallState);
        }
        if (stateAge == 19)
        {
            // If you hold the thrust input longer than this, it's no longer considered a jump.
            // NOTE: If this duration is longer than the JumpStart animation,
            // then we'll automatically transition to the JumpAir animation before we make this check
            // Currently the duration of this animation is about 20 fixed updates.
            player.IsJumping = false;
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
        player.input.EnableInput();
        thrusterVolumeCounter = Mathf.Clamp(stateAge,0,30);
        player.vfx.StopPrimaryThrusters();
        player.IgnoreIdleAnimationReset = true;

        base.exit();
    }
    void StartJumpSequence()
    {
        player.IsJumping = true;
    }
    public void RunJump()
    {
        player.rb.linearVelocity = new Vector3(player.rb.linearVelocity.x, player.GetJumpForce(), player.rb.linearVelocity.z);

    }
}
