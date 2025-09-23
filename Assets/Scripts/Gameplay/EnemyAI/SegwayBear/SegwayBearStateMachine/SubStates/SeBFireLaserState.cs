using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeBFireLaserState : SeBProvokedState
{
    public SeBFireLaserState(SegwayBear segwayBear, SegwayBearStateMachine segwayBearStateMachine) : base(segwayBear, segwayBearStateMachine){

    }
    ParticleSystem Laser;
    ParticleSystem ExcessParticleRunoff;
    Rigidbody rb;

    int laserFireTimer;
    int laserFireTimerMax = 100;

    public override void enter(){
        laserFireTimer = 0;
        Laser = segwayBear.LaserParticles[0];
        ExcessParticleRunoff = segwayBear.LaserParticles[3];
        Laser.Play();
        ExcessParticleRunoff.Play();
        rb = segwayBear.rb;
        segwayBear.angleMachine.StartNewNoCompPeriod(40);
        
        PlayNewSound();
        base.enter();

        // Intial Jolt at the start of the state
        float initialRecoilForce = 500f;
        float forwardX = segwayBear.transform.forward.x;
        if(forwardX < 0){
            segwayBear.rb.AddForce(new Vector3(initialRecoilForce, 0, 0), ForceMode.Impulse);
            segwayBear.angleMachine.PushLeft();
        } else
        {
            segwayBear.rb.AddForce(new Vector3(-initialRecoilForce, 0, 0), ForceMode.Impulse);
            segwayBear.angleMachine.PushRight();
        }
    }
    public override void Update()
    {
        laserFireTimer++;

        float pushbackDurationFrames = laserFireTimerMax; // 100 frames at 60 FPS
        float halfway = pushbackDurationFrames / 2f;
        float forwardX = segwayBear.transform.forward.x;
        float constantForce = 500f;

        if (laserFireTimer < halfway)
        {
            // First half: constant force
            if (forwardX < 0)
            {
                segwayBear.rb.AddForce(new Vector3(constantForce, 0, 0), ForceMode.Force);
            }
            else
            {
                segwayBear.rb.AddForce(new Vector3(-constantForce, 0, 0), ForceMode.Force);
            }
        } else if (laserFireTimer < pushbackDurationFrames)
        {
            // Second half: gradually reduce force to 0
            float t = (laserFireTimer - halfway)/(pushbackDurationFrames - halfway); // Normalized time (0 to 1)
            float reducedForce = Mathf.Lerp(constantForce, 0f, t); // Linearly interpolate force from 500 to 0
            if (forwardX < 0)
            {
                segwayBear.rb.AddForce(new Vector3(reducedForce, 0, 0), ForceMode.Force);
            }
            else
            {
                segwayBear.rb.AddForce(new Vector3(-reducedForce, 0, 0), ForceMode.Force);
            }
        }


        //rb.AddForce(new Vector3(500,0,0),ForceMode.Force);
        if (laserFireTimer >= laserFireTimerMax)
        {
            Laser.Stop();
            ExcessParticleRunoff.Stop();

            // Unfreeze X rotation befpre recoil
            Debug.Log($"[FIRE] Before recoil: " +
                $"Constraints={segwayBear.rb.constraints}, " +
                $"Rotation={segwayBear.transform.rotation.eulerAngles}");

            Debug.Log($"[FIRE] After recoil: " +
                        $"Constraints={segwayBear.rb.constraints}, " +
                        $"Rotation={segwayBear.transform.rotation.eulerAngles}");
             
            segwayBear.stateMachine.changeState(segwayBear.seBIdleState);
        }
        base.Update();
    }



    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    void PlayNewSound(){
        segwayBear.bearAudio.clip = segwayBear.BearSounds[5];
        segwayBear.bearAudio.Play();
    }

    void SetRingColor(Material ringMat,float ColorNumber){
            Color ringColor = new Color(ColorNumber,0,0);
            ringMat.SetColor("_EmissionColor",ringColor);
    }
}
