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
        virtualCam.localRotation = Quaternion.Euler(0.0f,0.0f,player.input.aimAngle*2); 

        //always be watching for the max speed a player can achieve. //TODO: Refactor this based on physics calculations times thrust stat.
        maxSpeed = WatchForMaxSpeed();

        if(!runningWobble && (player.OtherObjectTouch || player.GroundTouch) && player.rb.linearVelocity.magnitude > maxSpeed * 0.6f)
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
            return;
        }
        //TODO: Finish this. Use the ball arc from Zoodia's boss as a template.
    }

}
