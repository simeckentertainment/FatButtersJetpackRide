using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RumbaWithKnifeSpinState : RumbaWithKnifeCalmState{
    public RumbaWithKnifeSpinState(RumbaWithKnife rumba, RumbaWithKnifeStateMachine rumbaWithKnifeStateMachine) : base(rumba, rumbaWithKnifeStateMachine){
    }

     public override void enter()
    {
        PlayAnim("SpinAnim");
        base.enter();
    }

    public override void Update(){
        base.Update();
    }

    public override void FixedUpdate(){
        if(rumba.anim.GetCurrentAnimatorStateInfo(0).IsName("SpinAnim") && rumba.anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            rumba.stateMachine.changeState(rumba.rumbaTurnState);
        }
        base.FixedUpdate();
    }

}
