using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CyborgBunnySpawnState : CyborgBunnyCalmState
{
    public CyborgBunnySpawnState(CyborgBunny cyborgBunny, CyborgBunnyStateMachine cyborgBunnyStateMachine) : base(cyborgBunny, cyborgBunnyStateMachine)
    {
    }

    //Kicks in upon first spawn for the character instance. Grows the bunny from nothing to full size while giving it a little spin.
    //There are 4 frames in the lightning animation.

    float turnAmount = 2.0f * 360f; // turn twice.
    Vector3 OriginalParticleSpawnSize;
    public override void enter()
    {
        cyborgBunny.transform.localScale = Vector3.zero;
        OriginalParticleSpawnSize = cyborgBunny.floatationParticles.transform.localScale;
        //Play spawn anim
        PlayAnim("BunnySpawn");
        base.enter();
    }
    public override void Update()
    {
        base.Update();
    }

    public override void FixedUpdate()
    {
        float normalizedStateTime = durationOfState / cyborgBunny.spawnAnimLength;
        cyborgBunny.floatationParticles.transform.localScale = OriginalParticleSpawnSize * normalizedStateTime;
        cyborgBunny.transform.localRotation = Quaternion.Euler(new Vector3(0.0f, Helper.RemapArbitraryValues(0f, 1f, 0f, turnAmount, normalizedStateTime), 0.0f));
        cyborgBunny.transform.localScale = new Vector3(normalizedStateTime, normalizedStateTime, normalizedStateTime);
        RunLightning();
        if (durationOfState == cyborgBunny.spawnAnimLength)
        {
            DeactivateAllPlates();
            cyborgBunny.stateMachine.changeState(cyborgBunny.cyborgBunnyIdleState);
        }
        cyborgBunny.rb.angularVelocity = Vector3.zero;

        base.FixedUpdate();
    }
    public override void exit()
    {

        base.exit();
    }



}
