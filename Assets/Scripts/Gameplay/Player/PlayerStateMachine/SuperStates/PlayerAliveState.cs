using UnityEngine;

public class PlayerAliveState : PlayerState
{
    AudioSource[] thrusterSoundHolders;
    public float thrusterVolumeCounter = 0f;

    public PlayerAliveState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
    {

    }
    int ballTimer;
    public override void enter()
    {
        thrusterSoundHolders = new AudioSource[2];
        thrusterSoundHolders[0] = player.vfx.ThrusterSoundHolders[0].GetComponent<AudioSource>();
        thrusterSoundHolders[1] = player.vfx.ThrusterSoundHolders[1].GetComponent<AudioSource>();
        player.vfx.StartRocketSounds();
        base.enter();
    }
    public override void Update()
    {
        base.Update();
    }
    public override void FixedUpdate()
    {
        if (!player.corgiTurned)
        {
            DidThePlayerTurnChecker();
        }
        //These are the collision runners.
        AdjustRotationAngle();
        HarmfulInteractionRunner();
        BoneRunner();
        PowerupRunner();
        BallRunner();
        LowGravModeRunner();
        thrusterVolumeRunner();
        if (player.FinishTouch) { player.stateMachine.changeState(player.playerWinState); }
        base.FixedUpdate();
    }
    #region CollisionRunners
    void DidThePlayerTurnChecker()
    {
        if (player.input.aimAngle != 0.0f)
        {
            player.corgiTurned = true;
            return;
        }
        if (player.input.GoCcw)
        {
            player.corgiTurned = true;
            return;
        }
        if (player.input.GoCw)
        {
            player.corgiTurned = true;
            return;
        }
    }
    private void HarmfulInteractionRunner()
    {
#if UNITY_EDITOR
        if (collectibleData.GameplayTestingMode)
        {
            return;
        }
#endif
        if (!BallCheck())
        {
            if (player.HarmfulTouch)
            {
                player.stateMachine.changeState(player.playerHurtState);
            }
            if (player.OHKTouch)
            {
                player.stateMachine.changeState(player.playerOHKState);
            }
        }
    }
    private void BoneRunner()
    {
        if (player.BoneTouch)
        {
            player.AddBones(1);
            player.BoneTouch = false;
        }
    }
    private void PowerupRunner()
    {
        if (player.JerryCanTouch)
        {
            player.Fuel += player.FuelAdditionAmount;
            player.JerryCanTouch = false;
        }
        if (player.FoodTouch)
        {
            if (player.tummy + player.FoodAdditionAmount > player.maxTummy)
            {
                player.tummy = player.maxTummy;
            }
            else
            {
                player.tummy += player.FoodAdditionAmount;
            }
            player.FoodTouch = false;
        }
    }
    private void BallRunner()
    {
        if (player.BallTouch)
        {
            if (!player.hasPermaBall)
            {
                player.hasTemporaryBall = true;
            }
            player.BallTouch = false;
        }
        if (player.hasTemporaryBall)
        {
            ballTimer++;
            if (ballTimer >= player.ballTimerMax)
            {
                player.hasTemporaryBall = false;
                ballTimer = 0;
            }
        }
        if (player.hasPermaBall | player.hasTemporaryBall)
        {
            player.vfx.BallEffectRunner();
        }
        if (!player.hasPermaBall & !player.hasTemporaryBall)
        {
            player.vfx.BallEffectCanceler();
        }
    }
    void LowGravModeRunner()
    {
        if (player.LowGravMode)
        {
            player.rb.AddForce(Physics.gravity * -0.5f);
        }
    }
    #endregion

    #region CoreMechanicStuff
    private void AdjustRotationAngle()  //Moving into input driver
    {
        if (player.input.GoCw & player.input.GoCcw) { return; }
        player.vfx.StopAllRotParticles();
        if (player.input.GoCcw)
        {
            player.KeyboardRollOffset += 0.25f * player.KeyboardSensitivity;
            player.vfx.StartMinusRotParticles();
        }
        if (player.input.GoCw)
        {
            player.KeyboardRollOffset -= 0.25f * player.KeyboardSensitivity;
            player.vfx.StartPlusRotParticles();
        }
        player.GravityRoll = player.input.aimAngle;
        player.transform.rotation = Quaternion.Euler(Vector3.forward * (player.GravityRoll + player.KeyboardRollOffset));
    }

    public void thrust()
    {
        player.rb.AddRelativeForce(0, player.thrust, 0);
    }
    public void UseFuel()
    {
        UseFuel(false);
    }
    
    public void UseFuel(bool isBoosting)
    {

#if UNITY_EDITOR
        if (collectibleData.GameplayTestingMode)
        {
            return;
        }
#endif
        if (!BallCheck())
        {
            // Boost consumes 1.0 fuel per frame, normal thrust consumes 0.5
            player.Fuel -= isBoosting ? 1.0f : 0.5f;
        }
    }
    bool BallCheck()
    {
        return player.hasPermaBall | player.hasTemporaryBall ? true : false;
    }


    void thrusterVolumeRunner()
    {
        if (player.input.GoThrust)
        {
            thrusterVolumeCounter++;
            if (thrusterVolumeCounter > 30f) { thrusterVolumeCounter = 30f; }
        }
        else
        {
            thrusterVolumeCounter--;
            if (thrusterVolumeCounter < 0f) { thrusterVolumeCounter = 0f; }
        }

        float volVal = thrusterVolumeCounter / 30f * collectibleData.SFXVolumeLevel;
        player.vfx.SetThrusterVolume(volVal);
    }
    #endregion

    public void PlayAnim(string animName)
    {
        player.anim.Play("gameplayBaseLayer." + animName, 0, 0.0f);
        if (player.secondaryAnim != null)
        {
            player.secondaryAnim.Play("ThrusterLayer." + animName, 0, 0.0f);
        }
    }

    public void PlayAnim(string animName, float normalizedTime)
    {
        player.anim.Play("gameplayBaseLayer." + animName, 0, normalizedTime);
        if (player.secondaryAnim != null)
        {
            player.secondaryAnim.Play("ThrusterLayer." + animName, 0, normalizedTime);
        }
    }
    public void ActivateGravyBoat()
    {
        if (player.gbr1 == null) { return; }
        player.gbr1.ActivateBoat();
        player.gbr2.ActivateBoat();
    }

    public void DeActivateGravyBoat()
    {
        if (player.gbr1 == null) { return; }
        player.gbr1.DeactivateBoat();
        player.gbr2.DeactivateBoat();
    }

}
