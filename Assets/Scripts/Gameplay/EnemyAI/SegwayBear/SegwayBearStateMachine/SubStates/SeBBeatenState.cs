using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SeBBeatenState : SeBProvokedState
{

    public SeBBeatenState(SegwayBear segwayBear, SegwayBearStateMachine segwayBearStateMachine) : base(segwayBear, segwayBearStateMachine){

    }
    AudioClip currentClip;

    public override void enter(){
        PlayNewSound();

        foreach (ParticleSystem ps in segwayBear.LaserParticles)
        {
            ps.Stop();
        }
        PhysicsSetup();
        //Social.ReportProgress("CgkI293vto8EEAIQBA", 100.0f, (bool success) => {Debug.Log("Bear Beaten!");});
        base.enter();
    }
    public override void Update(){
        base.Update();
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    void PlayNewSound(){
        segwayBear.bearAudio.clip = segwayBear.BearSounds[4];
        segwayBear.bearAudio.Play();
    }

    void PhysicsSetup()
    {
        //get rid of the current system.
        segwayBear.angleMachine.enabled = false;
        segwayBear.axel.gameObject.GetComponent<MeshRenderer>().enabled = false;
        segwayBear.axel.enabled = false; //axel refers to the wheelMachine.
        foreach (GameObject wheel in segwayBear.wheels)
        {
            wheel.GetComponent<WheelCollider>().enabled = false;
            wheel.GetComponent<MeshCollider>().enabled = true;
            //wheel.AddComponent<Rigidbody>();
        }
        foreach (GameObject piece in segwayBear.DeathPhysicsPieces)
        {
            piece.GetComponent<Collider>().enabled = true;
            piece.AddComponent<Rigidbody>();
            piece.GetComponent<Rigidbody>().AddExplosionForce(1.0f, piece.transform.position, 2.0f, 2.0f);
        }

        segwayBear.booBearModel.AddComponent<Rigidbody>();
        SphereCollider boobear = segwayBear.booBearModel.GetComponent<SphereCollider>();
        boobear.isTrigger = false;
        boobear.radius = 0.7f;
        boobear.center = Vector3.zero;


        //Make sure that all of the pieces can no longer harm the player.
        foreach (Transform t in segwayBear.GetComponentsInChildren<Transform>())
        {
            t.gameObject.tag = "Friendly";
        }
    }
}
