using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CyborgBunnyIdleState : CyborgBunnyCalmState
{

    Vector3 currentIdleTargetPos;
    public CyborgBunnyIdleState(CyborgBunny cyborgBunny, CyborgBunnyStateMachine cyborgBunnyStateMachine) : base(cyborgBunny, cyborgBunnyStateMachine)
    {
    }

    public override void enter()
    {
        for (int i = 0; i < cyborgBunny.lightningPlates.Length; i++)
        {
            cyborgBunny.lightningPlates[i].SetActive(false);
        }
        currentIdleTargetPos = SetNewTargetPos();
        PlayAnim("BunnyIdle1");
        base.enter();
    }
    public override void Update()
    {
        base.Update();
    }

    public override void FixedUpdate()
    {
        if (CastLineOfSightToPlayer())
        {
            cyborgBunny.stateMachine.changeState(cyborgBunny.cyborgBunnyNoticePlayerState);
        }


        //If the player's not a valid target, just do patrol.
        if (Helper.isWithinMarginOfError(cyborgBunny.transform.position, currentIdleTargetPos, 0.05f))
        {
            cyborgBunny.StopMomentum();
            currentIdleTargetPos = SetNewTargetPos();
        } else {
            cyborgBunny.transform.LookAt(currentIdleTargetPos); //Looks towards the goal.
            Vector3 movementVectorThisFrame = (cyborgBunny.transform.position - currentIdleTargetPos).normalized;

            cyborgBunny.rb.AddForce(movementVectorThisFrame*cyborgBunny.speedMult * -1f);
        }
        //Bunny spawner is run in the Master state.
        base.FixedUpdate();
    }



    Vector3 SetNewTargetPos()
    {
        Vector3 currentPos = cyborgBunny.transform.position;
        return new Vector3(currentPos.x + Random.Range(-5f, 5f),currentPos.y,currentPos.z);
    }
}
