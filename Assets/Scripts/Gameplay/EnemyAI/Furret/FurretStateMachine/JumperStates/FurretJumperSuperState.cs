using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurretJumperSuperState : FurretState
{
    public FurretJumperSuperState(Furret furret, FurretStateMachine furretStateMachine) : base(furret, furretStateMachine){

    }
    public override void enter(){
        base.enter();
    }

    public override void Update(){
        base.Update();
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
