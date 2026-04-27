using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotatockSuperState : PotatockMasterState{
    public PotatockSuperState(Potatock potatock, PotatockStateMachine potatockStateMachine) : base(potatock, potatockStateMachine){
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
