using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CyborgBunnyChasePlayerState : CyborgBunnyAggroState {
    public CyborgBunnyChasePlayerState(CyborgBunny cyborgBunny, CyborgBunnyStateMachine cyborgBunnyStateMachine) : base(cyborgBunny, cyborgBunnyStateMachine) {
    }
    Vector3 GoalPos;
    public override void enter() {
        GoalPos = GetNextGoalPos();
        PlayAnim("BunnyRAGELoop");
        base.enter();
    }
    public override void Update() {

            base.Update();
    }

    public override void FixedUpdate() {

        //Lasers look at the player. This needs to happen seperately from the head.
        foreach (ParticleSystem laser in cyborgBunny.lasers)
        {
            laser.transform.LookAt(cyborgBunny.player.transform);
        }

        cyborgBunny.transform.LookAt(GetPlayerCoords()); //Bunny Looks at the player for laser-firing purposes.
        if (durationOfState % cyborgBunny.LaserFireFrequency == 0)
        {
            cyborgBunny.FireLasers();
        }

        if (Helper.isWithinMarginOfError(cyborgBunny.transform.position.x, GoalPos.x, 0.05f))
        {
            cyborgBunny.StopMomentum();
            GoalPos = GetNextGoalPos();
        }
        else
        {
            Vector3 movementVectorThisFrame = (cyborgBunny.transform.position - GoalPos).normalized;
            cyborgBunny.rb.AddForce(movementVectorThisFrame*cyborgBunny.speedMult * -1f);
        }


        base.FixedUpdate();
    }



    Vector3 GetNextGoalPos() {
        Vector3 goalPos = new Vector3(GetPlayerCoords().x, GetPlayerCoords().y+5.0f,0.0f);
        if (cyborgBunny.transform.position.x < cyborgBunny.player.transform.position.x) //If we are to the left of the player, aim for the right side
        {
            goalPos.x = goalPos.x + 5.0f;
        }
        else //If we are to the right of the player, aim for the left side
        {
            goalPos.x = goalPos.x - 5.0f;
        }
        return goalPos;
    }
}
