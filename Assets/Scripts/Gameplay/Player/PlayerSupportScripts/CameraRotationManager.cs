using System;
using Unity.VisualScripting;
using UnityEngine;

public class CameraRotationManager : MonoBehaviour
{
    [SerializeField] Player player;
    float PlayerMaxSpeed;
    [SerializeField] Transform virtualCam;
    [SerializeField] DeviceType deviceType;
    enum DeviceType {HandheldGyro, Stationary};

    bool runningWobble;
    int wobbleCounter;
    [SerializeField] int wobbleMaxCount;
    float[] wobblePoints;
    [SerializeField] int[] FivePointTiming;
    [SerializeField] int[] SevenPointTiming;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayerMaxSpeed = calculatePlayerTerminalVelocity(); //We're calculating all wobble intensities based on terminal velocity. It's more consistent that way.
    }

    // Update is called once per frame
    void Update()
    {
        
        if(deviceType == DeviceType.Stationary){ return;} //Don't need to handle rotation for stationary devices!
        //We're manually taking over camera rotations because letting Cinemachine do it was leading to unpredictable results.
        if(!runningWobble){
        virtualCam.localRotation = Quaternion.Euler(0.0f,0.0f,player.input.aimAngle*2); 
        }

        //always be watching for the max speed a player can achieve. //TODO: Refactor this based on physics calculations times thrust stat.
        
        if(!runningWobble && (player.OtherObjectTouch || player.GroundTouch))
        {
            Debug.Log(player.rb.linearVelocity.magnitude);
            InitiateWobble();
        }

        if (runningWobble)
        {
            RunWobble();
        }
    }

    void InitiateWobble()
    {
        float TerminalVelocityPercentage = Mathf.Clamp01(player.rb.angularVelocity.magnitude / PlayerMaxSpeed); //For our purposes, we don't need values beyond 100%.
        if(TerminalVelocityPercentage < 0.05f){return;} //Don't wobble for light impacts.

        float maxDeviation = 90.0f * TerminalVelocityPercentage; // force-sensitive.
        float DeviceRotationAtImpact = player.input.aimAngle*2.0f;
        if(TerminalVelocityPercentage < 0.4f) // Below 0.75 is medium impact. We get 5-point wobble. Above 0.75 is heavy impact. We use 7 poiunt wobble.
        {
            wobblePoints = new float[5]{
                DeviceRotationAtImpact,
                DeviceRotationAtImpact - maxDeviation*0.8f,
                DeviceRotationAtImpact + maxDeviation*0.5f,
                DeviceRotationAtImpact - maxDeviation*0.2f,
                DeviceRotationAtImpact
            };

        } else {
            wobblePoints = new float[7]
            {
                DeviceRotationAtImpact,
                DeviceRotationAtImpact + maxDeviation*0.8f,
                DeviceRotationAtImpact - maxDeviation*0.7f,
                DeviceRotationAtImpact + maxDeviation*0.45f,
                DeviceRotationAtImpact - maxDeviation*0.2f,
                DeviceRotationAtImpact + maxDeviation*0.05f,
                DeviceRotationAtImpact
            };
        }
        runningWobble = true;
    }
    
    
    void RunWobble()
    {
        wobbleCounter ++;
        if(wobbleCounter > wobbleMaxCount) //if we're done.
        {
            runningWobble = false;
            virtualCam.localRotation = Quaternion.Euler(0.0f,0.0f,player.input.aimAngle*2); //reset to active angle tracking.
            wobblePoints = new float[1]; //clear that puppy out.
            return; //kicks us out of wobble madness if we're done.
        }
        Debug.Log("Running Wobble!");
        if(wobblePoints.Length == 5){virtualCam.localRotation = Quaternion.Euler(run5Wobble());}
        if(wobblePoints.Length == 7){virtualCam.localRotation = Quaternion.Euler(run7Wobble());}
        //I'm sure there's a better way to do this than writing 2 methods that do the same thing slightly differently, but I just wanna get it done. Maybe I can come back to this and rethink it later.
    }

    Vector3 run5Wobble()
    { // threshold points are .25, .5, .75, and 1
        Vector3 outRot;
        float percentComplete = wobbleCounter/wobbleMaxCount;
        if(percentComplete < 0.25f) //Wobble 1. This code is ugly as sin but it works a treat.
        {
            outRot = Vector3.Lerp(
                    new Vector3(0.0f,0.0f,wobblePoints[0]),
                    new Vector3(0.0f,0.0f,wobblePoints[1]),
                    Helper.RemapToBetweenZeroAndOne(0.0f,0.25f,percentComplete
                    )
                );
        }else if (percentComplete > 0.25f && percentComplete < 0.5f) //Wobble 2
        {
            outRot = Vector3.Lerp(
                new Vector3(0.0f,0.0f,wobblePoints[1]),
                new Vector3(0.0f,0.0f,wobblePoints[2]),
                Helper.RemapToBetweenZeroAndOne(0.25f,0.5f,percentComplete)
            );


        }else if (percentComplete > 0.5f && percentComplete < 0.75f) //Wobble 3
        {
            outRot = Vector3.Lerp(
                new Vector3(0.0f,0.0f,wobblePoints[2]),
                new Vector3(0.0f,0.0f,wobblePoints[3]),
                Helper.RemapToBetweenZeroAndOne(0.5f,0.75f,percentComplete)
            );            
        } else //Wobble 4
        {
            outRot = Vector3.Lerp(
                new Vector3(0.0f,0.0f,wobblePoints[3]),
                new Vector3(0.0f,0.0f,wobblePoints[4]),
                Helper.RemapToBetweenZeroAndOne(0.75f,1.0f,percentComplete)
            );       
        }
        return outRot;
    } 

    Vector3 run7Wobble()
    { //threshold points are .16, .33, .45, .65, .82, and 1
        Vector3 outRot;
        float percentComplete = wobbleCounter/wobbleMaxCount;
        if(percentComplete < 0.16f) // Wobble 1. This code is ugly as sin but it works a treat.
        {
            outRot = Vector3.Lerp(
                    new Vector3(0.0f,0.0f,wobblePoints[0]),
                    new Vector3(0.0f,0.0f,wobblePoints[1]),
                    Helper.RemapToBetweenZeroAndOne(0.0f,0.16f,percentComplete
                    )
                );
        }else if (percentComplete > 0.16f && percentComplete < 0.33f) //Wobble 2
        {
            outRot = Vector3.Lerp(
                new Vector3(0.0f,0.0f,wobblePoints[1]),
                new Vector3(0.0f,0.0f,wobblePoints[2]),
                Helper.RemapToBetweenZeroAndOne(0.33f,0.45f,percentComplete)
            );


        }else if (percentComplete > 0.33f && percentComplete < 0.45f) //Wobble 3
        {
            outRot = Vector3.Lerp(
                new Vector3(0.0f,0.0f,wobblePoints[2]),
                new Vector3(0.0f,0.0f,wobblePoints[3]),
                Helper.RemapToBetweenZeroAndOne(0.5f,0.75f,percentComplete)
            );            
        }else if (percentComplete > 0.45f && percentComplete < 0.65f) //Wobble 4
        {
            outRot = Vector3.Lerp(
                new Vector3(0.0f,0.0f,wobblePoints[3]),
                new Vector3(0.0f,0.0f,wobblePoints[4]),
                Helper.RemapToBetweenZeroAndOne(0.75f,1.0f,percentComplete)
            );      
        } else if (percentComplete > 0.65f && percentComplete < 0.82f) //Wobble 5
        {
            outRot = Vector3.Lerp(
                new Vector3(0.0f,0.0f,wobblePoints[4]),
                new Vector3(0.0f,0.0f,wobblePoints[5]),
                Helper.RemapToBetweenZeroAndOne(0.65f,0.82f,percentComplete)
            );       
        } else //Wobble 6
        {
            outRot = Vector3.Lerp(
                new Vector3(0.0f,0.0f,wobblePoints[5]),
                new Vector3(0.0f,0.0f,wobblePoints[6]),
                Helper.RemapToBetweenZeroAndOne(0.82f,1.0f,percentComplete)
            );      
        }  
        return outRot;         
    }


    float calculatePlayerTerminalVelocity()
    {
        float effectiveDrag = player.rb.linearDamping / ( 1.0f + player.rb.linearDamping * Time.fixedDeltaTime);
        //float effectiveDrag = 1.0f;
        return (player.thrust - player.rb.mass * Mathf.Abs(Physics.gravity.y)) / (player.rb.mass * effectiveDrag);
    }

}
