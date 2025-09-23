using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeBChargeLaserState : SeBProvokedState
{
    public SeBChargeLaserState(SegwayBear segwayBear, SegwayBearStateMachine segwayBearStateMachine) : base(segwayBear, segwayBearStateMachine){

    }

    Material ring1;
    Material ring2;
    Material ring3;
    int intakeTimer;
    ParticleSystem intakeParticles;
    ParticleSystem excessParticleRunoff;
    List<Quaternion> RotList;
    int chargeSegmentLength;
    int currentChargeSegment;

    public override void enter(){
        PlayNewSound();
        RotList = new List<Quaternion>();
        FigureRotList();
        chargeSegmentLength = RotList.Count;
        currentChargeSegment = 1;
        ring1 = segwayBear.LaserRings[0].material;
        ring2 = segwayBear.LaserRings[1].material;
        ring3 = segwayBear.LaserRings[2].material;
        intakeParticles = segwayBear.LaserParticles[2];
        excessParticleRunoff = segwayBear.LaserParticles[3];
        intakeParticles.Play();
        base.enter();
    }
    public override void Update()
    {
        RunRings();
        //RunLaserWobble();
        segwayBear.PitchCubes[1].LookAt(segwayBear.target);
        RunAudio();
        base.Update();
    }

    void FigureRotList(){ //kindly provided by chatGPT
        Vector3 forwardDir = segwayBear.PitchCubes[1].forward;
        // Calculate the angle in degrees using Atan2
        float targetAngle = Mathf.Atan2(forwardDir.y, forwardDir.x) * Mathf.Rad2Deg;
        // Calculate the angles 45 degrees more and 45 degrees less than the target angle
        float angle45More = targetAngle + 45f;
        float angle45Less = targetAngle - 45f;
        // Wrap the angles within the range [0, 360] to avoid exceeding full rotations
        angle45More = WrapAngle(angle45More);
        angle45Less = WrapAngle(angle45Less);
        // Convert the angles to Quaternion rotations
        Quaternion rotationTarget = Quaternion.Euler(targetAngle,0f, 0f);
        Quaternion rotation45More = Quaternion.Euler(angle45More,0f, 0f);
        Quaternion rotation45Less = Quaternion.Euler(angle45Less,0f, 0f);
        RotList.Add(rotationTarget);
        RotList.Add(rotation45Less);
        RotList.Add(rotation45More);
        RotList.Add(Quaternion.Lerp(rotation45More,rotation45Less,0.9f));
        RotList.Add(Quaternion.Lerp(rotation45Less,rotation45More,0.8f));
        RotList.Add(Quaternion.Lerp(rotation45More,rotation45Less,0.7f));
        RotList.Add(Quaternion.Lerp(rotation45Less,rotation45More,0.6f));
        RotList.Add(rotationTarget);
    }
    void RunLaserWobble(){
        if(segwayBear.GetAudioPercentage() >= (segwayBear.bearAudio.clip.length/chargeSegmentLength)*currentChargeSegment ){
            currentChargeSegment++;
        }
        Quaternion firstQuat = RotList[currentChargeSegment-1];
        Quaternion SecondQuat = RotList[currentChargeSegment];
        float currentPlace = Helper.RemapToBetweenZeroAndOne((segwayBear.bearAudio.clip.length/chargeSegmentLength)*(currentChargeSegment-1),(segwayBear.bearAudio.clip.length/chargeSegmentLength)*(currentChargeSegment-1),segwayBear.bearAudio.clip.length/chargeSegmentLength);
        segwayBear.PitchCubes[1].transform.rotation = Quaternion.Lerp(firstQuat,SecondQuat,currentPlace);
    }
        float WrapAngle(float angle)
    {
        angle %= 360f;
        if (angle < 0)
            angle += 360f;
        return angle;
    }
    private void RunAudio()
    {
        if (segwayBear.GetAudioPercentage() > 0.4f)
        {
            excessParticleRunoff.Play();
        }
        if (segwayBear.GetAudioPercentage() >= 0.90f)
        {
            intakeParticles.Stop();
            
            segwayBear.stateMachine.changeState(segwayBear.seBFireLaserState);
        }
    }

    private void RunRings(){
        if (segwayBear.GetAudioPercentage() > 0.26f && segwayBear.GetAudioPercentage() < 0.5f){
            SetRingColor(ring1, Helper.RemapArbitraryValues(0.26f, 0.5f, 0, 255, segwayBear.GetAudioPercentage()));
        }
        if (segwayBear.GetAudioPercentage() > 0.45f && segwayBear.GetAudioPercentage() < 0.7f){
            SetRingColor(ring2, Helper.RemapArbitraryValues(0.45f, 0.7f, 0, 255, segwayBear.GetAudioPercentage()));
        }
        if (segwayBear.GetAudioPercentage() > 0.6f && segwayBear.GetAudioPercentage() < 0.8f){
            SetRingColor(ring3, Helper.RemapArbitraryValues(0.6f, 0.8f, 0, 255, segwayBear.GetAudioPercentage()));
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    void PlayNewSound(){
        segwayBear.bearAudio.clip = segwayBear.BearSounds[3];
        segwayBear.bearAudio.Play();
    }


    void SetRingColor(Material ringMat,float ColorNumber){
            Color ringColor = new Color(ColorNumber,0,0);
            ringMat.SetColor("_EmissionColor",ringColor);
    }
}
