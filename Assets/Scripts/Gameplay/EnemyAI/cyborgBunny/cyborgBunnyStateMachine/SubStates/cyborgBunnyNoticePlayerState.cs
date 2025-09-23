using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CyborgBunnyNoticePlayerState : CyborgBunnyCalmState{
    public CyborgBunnyNoticePlayerState(CyborgBunny cyborgBunny, CyborgBunnyStateMachine cyborgBunnyStateMachine) : base(cyborgBunny, cyborgBunnyStateMachine){
    }

    public override void enter(){
        cyborgBunny.playerSeen = true;
        PlayAnim("BunnyRAGEEnter");
        base.enter();
    }
    public override void Update(){
        base.Update();
    }

    public override void FixedUpdate()
    {
        if (durationOfState >= 5)
        {
            cyborgBunny.stateMachine.changeState(cyborgBunny.cyborgBunnyChasePlayerState);
        }
        base.FixedUpdate();
        
    }
}
