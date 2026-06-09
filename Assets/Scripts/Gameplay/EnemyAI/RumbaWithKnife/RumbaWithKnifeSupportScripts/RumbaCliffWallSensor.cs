using UnityEngine;

public class RumbaCliffWallSensor : MonoBehaviour
{
    [SerializeField] RumbaWithKnife rumba;
    [SerializeField] Transform otherSensor;
    [SerializeField] public RumbaWithKnife.Direction side;
    [SerializeField] float rotationCheckBlock = 30.0f;
    RaycastHit wallCheck;
    RaycastHit downCast;
    [SerializeField] LayerMask wallLayerMask = -1; // default to Everything


    void Start()
    {
        CoordinateSide();
    }
    // Update is called once per frame
    void Update()
    {
        CoordinateSide();
        if (VerifyRumbaRotations())
        {
            ReportAsNeeded(CheckForGround(),CheckForWall());
        }
    }
    bool CheckForGround()
    {
        if(Physics.Raycast(transform.position, -transform.up, out downCast, 3.0f, wallLayerMask,QueryTriggerInteraction.Ignore)) //Long drops it stops and turns around. Short drops it just goes for it.
        {
            return true;
        } else
        {
            return false;
        }
    }
    bool CheckForWall()
    {
        if(Physics.Raycast(transform.position, transform.forward, out wallCheck, rumba.WanderDistance, wallLayerMask,QueryTriggerInteraction.Ignore)) //cast forward to look for wall.
        {

            if(side == RumbaWithKnife.Direction.Left)
            {
                rumba.wanderLeftMax = new Vector3(wallCheck.point.x, rumba.transform.position.y, 0.0f);
            } else
            {
                rumba.wanderRightMax = new Vector3(wallCheck.point.x, rumba.transform.position.y, 0.0f);
            }
            return Vector3.Distance(rumba.transform.position,wallCheck.point) <= rumba.wallDistanceTrigger; //only return true if we're super close to a wall.

        } else {
            if(side == RumbaWithKnife.Direction.Left)
            {
                rumba.wanderLeftMax = new Vector3(rumba.transform.position.x - rumba.WanderDistance, rumba.transform.position.y, 0.0f);
            } else
            {
                rumba.wanderRightMax = new Vector3(rumba.transform.position.x + rumba.WanderDistance, rumba.transform.position.y, 0.0f);
            }
            return false;
        }
    }
    bool VerifyRumbaRotations()
    {
        if(Helper.isWithinMarginOfError(rumba.transform.rotation.eulerAngles.y,rumba.leftFacingRot,rotationCheckBlock)){return true;}
        if(Helper.isWithinMarginOfError(rumba.transform.rotation.eulerAngles.y,rumba.rightFacingRot,rotationCheckBlock)){return true;}
        return false;
    }
    void CoordinateSide()
    {
        if(transform.position.x > otherSensor.position.x)
        {
            side = RumbaWithKnife.Direction.Right;
        } else
        {
            side = RumbaWithKnife.Direction.Left;
        }
    }
    void ReportAsNeeded(bool groundCheck, bool wallCheck)
    {
        if(side == RumbaWithKnife.Direction.Left)
        {
            rumba.ignoreLeft = wallCheck || !groundCheck;

        } else if( side == RumbaWithKnife.Direction.Right)
        {
            rumba.ignoreRight = wallCheck || !groundCheck;
        }

        

    }
    public enum Side {left, right}
}
