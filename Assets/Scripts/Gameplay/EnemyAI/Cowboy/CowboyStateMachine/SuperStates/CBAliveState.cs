using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CBAliveState : CowboyState
{
    public CBAliveState(Cowboy cowboy, CowboyStateMachine cowboyStateMachine) : base(cowboy, cowboyStateMachine){

    }
    Vector3 leftRot = Vector3.down*90f;
    Vector3 rightRot = Vector3.up*90f;
    public override void enter(){
        base.enter();
    }

    public override void Update(){
        if(cowboy.hitTop){
            cowboy.stateMachine.changeState(cowboy.cowboyUpDieState);
            cowboy.hitTop = false;
            cowboy.cowboyCollider.enabled = false;
        }
        if(cowboy.hitBot){
            cowboy.stateMachine.changeState(cowboy.cowboyDownDieState);
            cowboy.hitBot = false;
            cowboy.cowboyCollider.enabled = false;
        }
        cowboy.playerDirection = cowboy.player.transform.position.x < cowboy.transform.position.x ? Cowboy.PlayerDirection.Left : Cowboy.PlayerDirection.Right;
        if(cowboy.playerDirection == Cowboy.PlayerDirection.Left){
            cowboy.transform.rotation = Quaternion.Euler(leftRot);
        } else {
            cowboy.transform.rotation = Quaternion.Euler(rightRot);
        }
        cowboy.playerDist = Vector3.Distance(cowboy.transform.position,cowboy.player.transform.position);

        base.Update();
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
