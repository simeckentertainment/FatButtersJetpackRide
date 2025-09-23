using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallState : PlayerAliveState
{
    public PlayerFallState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine){

    }
    float stateAge;
    float VolumeReductionThreshold;
    public override void enter(){
        stateAge = 0;
        VolumeReductionThreshold = 10;
        PlayAnim("fall");
        DeActivateGravyBoat();
        base.enter();
    }

    public override void Update(){
        base.Update();
    }
    public override void FixedUpdate()
    {


        stateAge++;

        //Calm the sound the fuck down so we don't blow people's ears out.
        player.sfx.volume = Mathf.Clamp((VolumeReductionThreshold-stateAge)/VolumeReductionThreshold,0f,1f);
        if((stateAge > VolumeReductionThreshold) & player.sfx.isPlaying){player.sfx.Stop();}
        if(player.GroundTouch){
            PlayAnim("Land");
            player.stateMachine.changeState(player.playerIdleState);
        }
        if(player.input.GoThrust){
            player.stateMachine.changeState(player.playerThrustState);
        }
        if (stateAge == 120)
        {
            PlayAnim("fallIdle");
        }
        if (stateAge == 360)
        {
            player.stateMachine.changeState(player.playerEnterDangleState);
        }
        base.FixedUpdate();
    }

    public override void exit(){
        //if(player.anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1.0f){
            //player.animationPercentage = 1.0f;
        //} else {
            //player.animationPercentage = GetNormalizedTime();
        //}
    }
}
