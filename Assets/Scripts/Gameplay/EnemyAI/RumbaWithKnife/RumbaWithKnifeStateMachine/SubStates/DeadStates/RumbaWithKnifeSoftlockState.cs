using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RumbaWithKnifeSoftlockState : RumbaWithKnifeDeathState{
    public RumbaWithKnifeSoftlockState(RumbaWithKnife rumba, RumbaWithKnifeStateMachine rumbaWithKnifeStateMachine) : base(rumba, rumbaWithKnifeStateMachine){
    }


    public override void enter(){
        PlayAnim("StuckAnim");
        base.enter();
    }
    public override void Update(){
        rumba.transform.Rotate(Vector3.up * 5f);
        base.Update();
    }

    public override void FixedUpdate(){
        base.FixedUpdate();
    }
    void SetWanderLocs()
    {
    }
}
