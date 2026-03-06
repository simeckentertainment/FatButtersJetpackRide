using Unity.VisualScripting;
using UnityEngine;

public class CameraRotationManager : MonoBehaviour
{
    [SerializeField] Player player;
    float maxSpeed;
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
        maxSpeed = WatchForMaxSpeed();

        if(!runningWobble && (player.OtherObjectTouch || player.GroundTouch) && player.rb.linearVelocity.magnitude > maxSpeed * 0.35f)
        {
            InitiateWobble();
        }

        if (runningWobble)
        {
            RunWobble();
        }
    }
    float WatchForMaxSpeed()
    {
        if(player.rb.linearVelocity.magnitude > maxSpeed){ 
            return player.rb.linearVelocity.magnitude;
        } else { 
            return maxSpeed;
        }

    }

    void InitiateWobble()
    {
        float MaxSpeedPercentage = player.rb.angularVelocity.magnitude / maxSpeed;
        float maxDeviation = 15.0f * MaxSpeedPercentage; // force-sensitive.
        float DeviceRotationAtImpact = player.input.aimAngle*2.0f;
        if(MaxSpeedPercentage > 0.8f)
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
        if(wobbleCounter > wobbleMaxCount)
        {
            runningWobble = false;
            virtualCam.localRotation = Quaternion.Euler(0.0f,0.0f,player.input.aimAngle*2); //reset to active angle tracking.
            wobblePoints = new float[1]; //clear that puppy out.
            return; //kicks us out of wobble madness if we're done.
        }

        if(wobblePoints.Length == 5){virtualCam.localRotation = Quaternion.Euler(run5Wobble());}
        if(wobblePoints.Length == 7){virtualCam.localRotation = Quaternion.Euler(run7Wobble());}
        //I'm sure there's a better way to do this than writing 2 methods that do the same thing slightly differently, but I just wanna get it done. Maybe I can come back to this and rethink it later.
    }

    Vector3 run5Wobble()
    { // threshold points are .25, .5, .75, and 1
        Vector3 outRot;
        float percentComplete = wobbleCounter/wobbleMaxCount;
        if(percentComplete < 0.25f) //This code is ugly as sin but it works a treat.
        {
            outRot = Vector3.Lerp(
                    new Vector3(0.0f,0.0f,wobblePoints[0]),
                    new Vector3(0.0f,0.0f,wobblePoints[1]),
                    Helper.RemapToBetweenZeroAndOne(0.0f,0.25f,percentComplete
                    )
                );
        }else if (percentComplete > 0.25f && percentComplete < 0.5f)
        {
            outRot = Vector3.Lerp(
                new Vector3(0.0f,0.0f,wobblePoints[1]),
                new Vector3(0.0f,0.0f,wobblePoints[2]),
                Helper.RemapToBetweenZeroAndOne(0.25f,0.5f,percentComplete)
            );


        }else if (percentComplete > 0.5f && percentComplete < 0.75f)
        {
            outRot = Vector3.Lerp(
                new Vector3(0.0f,0.0f,wobblePoints[2]),
                new Vector3(0.0f,0.0f,wobblePoints[3]),
                Helper.RemapToBetweenZeroAndOne(0.5f,0.75f,percentComplete)
            );            
        } else
        {
            outRot = Vector3.Lerp(
                new Vector3(0.0f,0.0f,wobblePoints[3]),
                new Vector3(0.0f,0.0f,wobblePoints[4]),
                Helper.RemapToBetweenZeroAndOne(0.75f,1.0f,percentComplete)
            ); //this line's ugly as sin.        
        }
        return outRot;
    } 

    Vector3 run7Wobble()
    { //threshold points are .16, .33, .45, .65, .82, and 1
        Vector3 outRot;
        float percentComplete = wobbleCounter/wobbleMaxCount;
        if(percentComplete < 0.16f) //This code is ugly as sin but it works a treat.
        {
            outRot = Vector3.Lerp(
                    new Vector3(0.0f,0.0f,wobblePoints[0]),
                    new Vector3(0.0f,0.0f,wobblePoints[1]),
                    Helper.RemapToBetweenZeroAndOne(0.0f,0.16f,percentComplete
                    )
                );
        }else if (percentComplete > 0.16f && percentComplete < 0.33f)
        {
            outRot = Vector3.Lerp(
                new Vector3(0.0f,0.0f,wobblePoints[1]),
                new Vector3(0.0f,0.0f,wobblePoints[2]),
                Helper.RemapToBetweenZeroAndOne(0.33f,0.45f,percentComplete)
            );


        }else if (percentComplete > 0.33f && percentComplete < 0.45f)
        {
            outRot = Vector3.Lerp(
                new Vector3(0.0f,0.0f,wobblePoints[2]),
                new Vector3(0.0f,0.0f,wobblePoints[3]),
                Helper.RemapToBetweenZeroAndOne(0.5f,0.75f,percentComplete)
            );            
        }else if (percentComplete > 0.45f && percentComplete < 0.65f) 
        {
            outRot = Vector3.Lerp(
                new Vector3(0.0f,0.0f,wobblePoints[3]),
                new Vector3(0.0f,0.0f,wobblePoints[4]),
                Helper.RemapToBetweenZeroAndOne(0.75f,1.0f,percentComplete)
            ); //this line's ugly as sin.        
        } else if (percentComplete > 0.65f && percentComplete < 0.82f) 
        {
            outRot = Vector3.Lerp(
                new Vector3(0.0f,0.0f,wobblePoints[4]),
                new Vector3(0.0f,0.0f,wobblePoints[5]),
                Helper.RemapToBetweenZeroAndOne(0.65f,0.82f,percentComplete)
            ); //this line's ugly as sin.        
        } else
        {
            outRot = Vector3.Lerp(
                new Vector3(0.0f,0.0f,wobblePoints[5]),
                new Vector3(0.0f,0.0f,wobblePoints[6]),
                Helper.RemapToBetweenZeroAndOne(0.82f,1.0f,percentComplete)
            ); //this line's ugly as sin.        
        }  
        return outRot;         
    }

}
