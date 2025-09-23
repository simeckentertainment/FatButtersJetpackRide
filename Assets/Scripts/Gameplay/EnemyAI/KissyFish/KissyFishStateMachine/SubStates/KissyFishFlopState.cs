using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KissyFishFlopState : KissyFishState
{
    public KissyFishFlopState(KissyFish kissyFish, KissyFishStateMachine kissyFishStateMachine) : base(kissyFish, kissyFishStateMachine){

    }

    Vector3 FlopDir;

    public override void enter(){
        FlopDir = new Vector3(Random.Range(0.0f,0.25f),1.0f,Random.Range(0.0f,0.25f));
        //FlopDir = Vector3.up*1000f;
        kissyFish.fishAudio.clip = kissyFish.FlopSound;
        kissyFish.fishAudio.Play();
        kissyFish.rb.AddForce(FlopDir*10,ForceMode.VelocityChange);
        base.enter();
    }
    public override void Update()
    {
        base.Update();
    }


    private void RunAudio()
    {
    }



    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }


}
