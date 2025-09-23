using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurretJumperIdleState : FurretJumperSuperState
{
    public FurretJumperIdleState(Furret furret, FurretStateMachine furretStateMachine) : base(furret, furretStateMachine){

    }



    public override void enter(){
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
