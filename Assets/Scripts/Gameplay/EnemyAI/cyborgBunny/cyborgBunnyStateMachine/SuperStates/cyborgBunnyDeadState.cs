using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CyborgBunnyDeadState : CyborgBunnyMasterState{
    public CyborgBunnyDeadState(CyborgBunny cyborgBunny, CyborgBunnyStateMachine cyborgBunnyStateMachine) : base(cyborgBunny, cyborgBunnyStateMachine){
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
