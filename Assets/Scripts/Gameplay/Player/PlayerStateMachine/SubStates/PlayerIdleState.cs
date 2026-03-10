using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerAliveState
{
    int stateAge;
    public PlayerIdleState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
    {

    }
    string[] idleAnims = {"idle1","idle2"};
    string[] idleAnnoyedAnims = { "idleAnnoyed1", "idleAnnoyed2" };
    
    int fallDelayThreshold = 20; //number of frames to wait before switching to fall state
    int fallDelayCounter;
    public override void enter()
    {
        stateAge = 0;
        if (player.animationPercentage == 0.0f)
        { //It should only ever be 0.0 on start.
            PlayAnim(idleAnims[Random.Range(0, 2)]);
        }
        DeActivateGravyBoat();
        base.enter();
    }

    public override void Update(){
        base.Update();
    }
    public override void FixedUpdate()
    {
        stateAge++;
        player.rb.linearVelocity = Vector3.zero;
        base.FixedUpdate();
        if (player.input.GoThrust & player.JetpackActivationPossible)
        {
            player.stateMachine.changeState(player.playerThrustState);
        }
        if (GetNormalizedTime() >= 0.99f)
        {
            PlayAnim(idleAnims[Random.Range(0, 2)]);
        }
        if (stateAge > 0 & stateAge % 1200 == 0)
        {
            PlayAnim(idleAnnoyedAnims[Random.Range(0, 2)]);
        }

        // Calculate walk detection (absZ) for transition check
        if (player.GroundTouch)
        {
            if (Mathf.Abs(player.input.aimAngle) > 15)
            {
                player.stateMachine.changeState(player.playerWalkState);
            }
        }
        if (GetCurrentAnimName() == "idleAnnoyed1" & Helper.isWithinMarginOfError(GetNormalizedTime(), 0.5f, 0.025f))
        {
            PlayOneTimeAudio(player.borks[Random.Range(0, 3)]); //play the bork
        }
        if (GetCurrentAnimName() == "idleAnnoyed1" & Helper.isWithinMarginOfError(GetNormalizedTime(), 0.75f, 0.025f))
        {
            PlayOneTimeAudio(player.borks[Random.Range(0, 3)]); //play the bork
        }

        //If the ground gets pulled out from under our feet, we need to fall.
        //This reproduces the code in the walk state, but eh.
        if (!player.GroundTouch & !player.OtherObjectTouch)
        {
            fallDelayCounter++;
            TrackStandToFallTransition();
        }
        else
        {
            fallDelayCounter = 0;
        }
    }
    
    private void TrackStandToFallTransition()
    {
        if(fallDelayCounter >= fallDelayThreshold)
        {
            player.stateMachine.changeState(player.playerFallState);
        }
    }
    public override void exit()
    {
        base.exit();
    }
}
