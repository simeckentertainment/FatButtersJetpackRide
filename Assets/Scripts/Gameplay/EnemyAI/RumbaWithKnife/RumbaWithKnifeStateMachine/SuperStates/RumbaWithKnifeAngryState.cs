using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RumbaWithKnifeAngryState : RumbaWithKnifeMasterState{
    public RumbaWithKnifeAngryState(RumbaWithKnife rumba, RumbaWithKnifeStateMachine rumbaWithKnifeStateMachine) : base(rumba, rumbaWithKnifeStateMachine){
    }


    public override void enter(){
        rumba.SetAngerSpouts(true);
        rumba.rumbaMesh.material.EnableKeyword("_EMISSION");
        rumba.rumbaMesh.material.SetColor("_EmissionColor", Color.red);
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
