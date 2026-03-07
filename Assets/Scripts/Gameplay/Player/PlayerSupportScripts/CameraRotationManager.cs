using System;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class CameraRotationManager : MonoBehaviour
{
    [SerializeField] Player player;
    float PlayerMaxSpeed;
    [SerializeField] Transform virtualCam;
    [SerializeField] DeviceType deviceType;
    enum DeviceType {HandheldGyro, Stationary};
    [SerializeField] float maxDeviation;
    [SerializeField] bool runningWobble;
    [SerializeField] float wobbleCounter;
    [SerializeField] float wobbleMaxCount;
    float[] wobblePoints;
    [SerializeField] float camAngleOffset;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayerMaxSpeed = calculatePlayerTerminalVelocity(); //We're calculating all wobble intensities based on terminal velocity. It's more consistent that way.
    }

    // Update is called once per frame
    void Update()
    {
        //We're manually taking over camera rotations because letting Cinemachine do it was leading to unpredictable results.
        if(!runningWobble && (player.OtherObjectTouch || player.GroundTouch))
        {
            InitiateWobble();
        }
        camAngleOffset = runningWobble ? GetWobbleOffset() : 0.0f;
        virtualCam.localRotation = deviceType == DeviceType.HandheldGyro ?
           Quaternion.Euler(0.0f, 0.0f, player.input.aimAngle * 2 + camAngleOffset):
           virtualCam.localRotation = Quaternion.Euler(0.0f, 0.0f, camAngleOffset);
    }

    void InitiateWobble()
    {
        wobbleCounter = 0;
        float TerminalVelocityPercentage = Mathf.Clamp01(player.rb.angularVelocity.magnitude / PlayerMaxSpeed); //For our purposes, we don't need values beyond 100%. So, clamp.
        if(TerminalVelocityPercentage < 0.25f){return;} //Don't wobble for light impacts.
        runningWobble = true;
        float deviation = maxDeviation * TerminalVelocityPercentage; // force-sensitive.
        if(TerminalVelocityPercentage < 0.5f) // Below 0.5 is medium impact. We get 5-point wobble. Above 0.5 is heavy impact. We use 7 point wobble.
        {
            wobblePoints = new float[5]{
                0.0f,
                deviation*-0.8f,
                deviation*0.5f,
                deviation*-0.2f,
                0.0f
            };

        } else {
            wobblePoints = new float[7]
            {
                0.0f,
                deviation*0.8f,
                deviation*-0.7f,
                deviation*0.45f,
                deviation*-0.2f,
                deviation*0.05f,
                0.0f
            };
        }
    }
    
    float GetWobbleOffset()
    {
        float output = 0.0f;
        wobbleCounter ++;
        if (wobbleCounter > wobbleMaxCount) //if we're done.
        {
            runningWobble = false;
            return 0.0f; //kicks us out of wobble madness if we're done.
        }
        if(wobblePoints.Length == 5){output = run5Wobble();}
        if (wobblePoints.Length == 7) { output = run7Wobble(); }
        return output;

    }

    float run5Wobble()
    { // threshold points are .25, .5, .75, and 1. These are hardcoded because they're even amounts. My animation experience tells me even amounts are the right way to go.
        float outRot;
        float percentComplete = wobbleCounter / wobbleMaxCount;
        if (percentComplete < 0.25f) //Wobble 1. This code is ugly as sin but it works a treat.
        {
            outRot = Mathf.Lerp(wobblePoints[0], wobblePoints[1],Helper.RemapToBetweenZeroAndOne(0.0f, 0.25f, percentComplete));
        }
        else if (percentComplete > 0.25f && percentComplete < 0.5f) //Wobble 2
        {
            outRot = Mathf.Lerp(wobblePoints[1], wobblePoints[2], Helper.RemapToBetweenZeroAndOne(0.25f, 0.5f, percentComplete));
        }
        else if (percentComplete > 0.5f && percentComplete < 0.75f) //Wobble 3
        {
            outRot = Mathf.Lerp(wobblePoints[2],wobblePoints[3],Helper.RemapToBetweenZeroAndOne(0.5f, 0.75f, percentComplete));
        }
        else //Wobble 4
        {
            outRot = Mathf.Lerp(wobblePoints[3],wobblePoints[4],Helper.RemapToBetweenZeroAndOne(0.75f, 1.0f, percentComplete));
        }
        return outRot;
    } 
    float run7Wobble()
    { //threshold points are .16, .33, .45, .65, .82, and 1
        float outRot;
        float percentComplete = wobbleCounter/wobbleMaxCount;
        if (percentComplete < 0.16f) // Wobble 1. This code is ugly as sin but it works a treat.
        {
            outRot = Mathf.Lerp(wobblePoints[0], wobblePoints[1], Helper.RemapToBetweenZeroAndOne(0.0f, 0.16f, percentComplete));
        }
        else if (percentComplete > 0.16f && percentComplete < 0.33f) //Wobble 2
        {
            outRot = Mathf.Lerp(wobblePoints[1], wobblePoints[2], Helper.RemapToBetweenZeroAndOne(0.33f, 0.45f, percentComplete));
        }
        else if (percentComplete > 0.33f && percentComplete < 0.45f) //Wobble 3
        {
            outRot = Mathf.Lerp(wobblePoints[2], wobblePoints[3], Helper.RemapToBetweenZeroAndOne(0.5f, 0.75f, percentComplete));
        }
        else if (percentComplete > 0.45f && percentComplete < 0.65f) //Wobble 4
        {
            outRot = Mathf.Lerp(wobblePoints[3], wobblePoints[4], Helper.RemapToBetweenZeroAndOne(0.75f, 1.0f, percentComplete));
        }
        else if (percentComplete > 0.65f && percentComplete < 0.82f) //Wobble 5
        {
            outRot = Mathf.Lerp(wobblePoints[4], wobblePoints[5], Helper.RemapToBetweenZeroAndOne(0.65f, 0.82f, percentComplete));
        }
        else //Wobble 6
        {
            outRot = Mathf.Lerp(wobblePoints[5], wobblePoints[6], Helper.RemapToBetweenZeroAndOne(0.82f, 1.0f, percentComplete));
        }  
        return outRot;         
    }


    float calculatePlayerTerminalVelocity()
    { //Leave this here so that this thing automatically fixes itself in case we decide to change the physics.
        float effectiveDrag = player.rb.linearDamping / ( 1.0f + player.rb.linearDamping * Time.fixedDeltaTime);
        return (player.thrust - player.rb.mass * Mathf.Abs(Physics.gravity.y)) / (player.rb.mass * effectiveDrag);
    }

}
