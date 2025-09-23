using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FurretNullState : FurretState //This state is unused.
{
    public FurretNullState(Furret furret, FurretStateMachine furretStateMachine) : base(furret, furretStateMachine){

    }



    public override void enter(){
        MonoBehaviour.Destroy(furret);
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
