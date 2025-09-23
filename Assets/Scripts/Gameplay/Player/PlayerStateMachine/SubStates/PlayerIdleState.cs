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
    string[] idleAnnoyedAnims = {"idleAnnoyed1","idleAnnoyed2"};
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
        base.FixedUpdate();
        if (player.input.GoThrust)
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

        if (GetCurrentAnimName() == "idleAnnoyed1" & Helper.isWithinMarginOfError(GetNormalizedTime(), 0.5f, 0.025f)){
            PlayOneTimeAudio(player.borks[Random.Range(0,3)]); //play the bork
        }
        if (GetCurrentAnimName() == "idleAnnoyed1" & Helper.isWithinMarginOfError(GetNormalizedTime(), 0.75f, 0.025f)){
            PlayOneTimeAudio(player.borks[Random.Range(0,3)]); //play the bork
        }
    }
    public override void exit()
    {
        base.exit();
    }
}
