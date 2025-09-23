using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CyborgBunnySuperState : CyborgBunnyMasterState{
    public CyborgBunnySuperState(CyborgBunny cyborgBunny, CyborgBunnyStateMachine cyborgBunnyStateMachine) : base(cyborgBunny, cyborgBunnyStateMachine){
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
