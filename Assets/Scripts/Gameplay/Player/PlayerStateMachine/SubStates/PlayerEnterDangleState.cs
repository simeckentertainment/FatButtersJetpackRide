using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnterDangleState : PlayerAliveState
{
    public PlayerEnterDangleState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine){

    }
    float stateAge;
    float VolumeReductionThreshold;
    public override void enter(){
        PlayAnim("enterFallDangle");
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
        if (GetNormalizedTime() >= 0.95f)
        {
            player.stateMachine.changeState(player.playerDangleState);
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
