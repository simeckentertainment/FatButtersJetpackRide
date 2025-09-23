using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CyborgBunnyDeathState : CyborgBunnyMasterState
{
    public CyborgBunnyDeathState(CyborgBunny cyborgBunny, CyborgBunnyStateMachine cyborgBunnyStateMachine) : base(cyborgBunny, cyborgBunnyStateMachine)
    {
    }
    float turnAmount = 2.0f * 360f; // turn twice.
    Vector3 BlueRingSizeOffset;
    public override void enter()
    {
        PlayAnim("BunnyDie");
        BlueRingSizeOffset = cyborgBunny.floatationParticles.transform.localScale;
        base.enter();
    }
    public override void Update()
    {
        base.Update();
    }

    public override void FixedUpdate()
    {
        RunLightning();
        float stateTime = cyborgBunny.spawnAnimLength - durationOfState;
        float normalizedStateTime = stateTime / cyborgBunny.spawnAnimLength;
        cyborgBunny.floatationParticles.transform.localScale = BlueRingSizeOffset * normalizedStateTime;
        cyborgBunny.transform.localScale = Vector3.one * normalizedStateTime;
        cyborgBunny.transform.localRotation = Quaternion.Euler(new Vector3(0.0f, Helper.RemapArbitraryValues(0f, 1f, 0f, turnAmount, normalizedStateTime), 0.0f));
        if(cyborgBunny.transform.localScale.x < 0.0f) { KillMe(); }
        base.FixedUpdate();
    }


    void KillMe()
    {
        MonoBehaviour.Destroy(cyborgBunny.transform.parent.gameObject);
    }
}
