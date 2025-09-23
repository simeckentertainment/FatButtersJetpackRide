using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CowboyChaseState : CBAliveState
{
    public CowboyChaseState(Cowboy cowboy, CowboyStateMachine cowboyStateMachine) : base(cowboy, cowboyStateMachine){

    }

    public override void enter(){
        cowboy.anim.Play("StartRun");
        base.enter();
    }
    public override void Update()
    {
        if(cowboy.playerDist < cowboy.aiTriggerDistance*0.5f){
            cowboy.stateMachine.changeState(cowboy.cowboyThrowRockState);
        } else if (cowboy.playerDist > cowboy.aiTriggerDistance*2.0f){
            cowboy.stateMachine.changeState(cowboy.cowboyIdleState);
        } else {
            //MoveToPlayerOnX();
        }
        base.Update();
    }


    private void RunAudio()
    {
    }



    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
    public void MoveToPlayerOnX(){
        if(cowboy.playerDirection == Cowboy.PlayerDirection.Left){
            cowboy.transform.localEulerAngles = new Vector3(0,-90,0);
        } else {
            cowboy.transform.localEulerAngles = new Vector3(0,90,0);
        }
        cowboy.transform.Translate(Vector3.forward*Time.deltaTime*cowboy.moveSpeed);
    }


}
