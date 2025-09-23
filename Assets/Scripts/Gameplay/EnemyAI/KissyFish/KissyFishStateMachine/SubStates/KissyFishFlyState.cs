using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KissyFishFlyState : KissyFishState
{
    public KissyFishFlyState(KissyFish kissyFish, KissyFishStateMachine kissyFishStateMachine) : base(kissyFish, kissyFishStateMachine){

    }



    public override void enter(){
        kissyFish.fishAudio.clip = kissyFish.JumpSounds[Random.Range(0,1)];
        kissyFish.fishAudio.Play();
        base.enter();
    }
    public override void Update()
    {
        RunAudio();
        base.Update();
    }


    private void RunAudio()
    {
    }



    public override void FixedUpdate()
    {
        kissyFish.transform.LookAt(kissyFish.rb.linearVelocity + kissyFish.transform.position);
        base.FixedUpdate();
    }


}
