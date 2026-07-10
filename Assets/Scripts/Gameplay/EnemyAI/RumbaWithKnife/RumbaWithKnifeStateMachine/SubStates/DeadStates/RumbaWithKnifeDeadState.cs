using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RumbaWithKnifeDeadState : RumbaWithKnifeDeathState{
    public RumbaWithKnifeDeadState(RumbaWithKnife rumba, RumbaWithKnifeStateMachine rumbaWithKnifeStateMachine) : base(rumba, rumbaWithKnifeStateMachine){
    }


    public override void enter(){
        Player.Instance.AddEnemiesDefeated();
        PlayAnim("StuckAnim");
        base.enter();
    }
    public override void Update(){
        rumba.transform.Rotate(Vector3.up * 5f);
        if(durationOfState == rumba.deathRageCountMax)
        {
            CreateExplosionSmoke(); //This just creates the smoke, hiding the rumba behind a sheet of particles.
            //Also creates a harmful explosion trigger.
        }
        if(durationOfState >= rumba.deathRageCountMax + 1f)
        {
            MonoBehaviour.Destroy(rumba.gameObject); //This destroys the rumba while the particles are there.
        }

        base.Update();
    }

    public override void FixedUpdate(){
        base.FixedUpdate();
    }

    void CreateExplosionSmoke()
    {
        foreach( GameObject obj in rumba.deathExplosionObjects)
        { 
            MonoBehaviour.Instantiate(obj,rumba.transform.position,Quaternion.identity);
        }
    }
}
