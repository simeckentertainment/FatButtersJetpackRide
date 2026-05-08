using UnityEngine;

public class PlayerWalkState : PlayerAliveState
{
    private float animNormalizedTime; //since we're switching between different animations dynamically, we should handle normalized time tracking here.
    private float absoluteZ;

    private string[] forwardMove = { "ForeWalkSlow", "ForeWalkMid", "ForeWalkFast" };

    WalkSpeed walkSpeedEnum;
    WalkSpeed previousWalkSpeedEnum;

    public PlayerWalkState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
    {
    }

    public override void enter()
    {
        previousWalkSpeedEnum = WalkSpeed.Stop; //set this now to avoid errors on frame 1.
        walkSpeedEnum = GetSpeedEnum();

        base.enter();
    }
    
    public override void FixedUpdate()
    {
        walkSpeedEnum = GetSpeedEnum();
        SetSpeed();
        if (walkSpeedEnum == WalkSpeed.Stop)
        {
            player.stateMachine.changeState(player.playerIdleState);
        }
        if (player.input.GoThrust)
        {
            player.stateMachine.changeState(player.playerThrustState);
        }

        if(durationOfState > 0)
        {
            animNormalizedTime = GetNormalizedTime(0); //for driving mid-animation changes
        }
        else
        {
            animNormalizedTime = 0.0f;
        }

        if (previousWalkSpeedEnum != walkSpeedEnum)
        {
            SetWalkAnimation();
            previousWalkSpeedEnum = walkSpeedEnum; //reset for "remembering" for next frame.
        }

        var targetAngle = targetRotation;
        if (Mathf.Abs(player.input.aimAngle) - 5 > 0) // TODO: arbitrary sensitivity of 5 degrees, make configurable
        {
            targetAngle = player.input.aimAngle < 0 ? 0 : 180;
        }
        SetTargetRotation(targetAngle, 720);
        
        if (player.IsFalling())
        {
            player.stateMachine.changeState(player.playerFallState);
        }

        base.FixedUpdate();
    }

    WalkSpeed GetSpeedEnum()
    {
        // Get signed Z rotation (-180 to 180)
        absoluteZ = Mathf.Abs(player.input.aimAngle);
        if (absoluteZ < player.SlowWalkMinAngle)
        {
            return WalkSpeed.Stop;
        }
        else if (absoluteZ >= player.SlowWalkMinAngle & absoluteZ < player.MediumWalkMinAngle)
        { //Slow walk
            return WalkSpeed.Slow;
        }
        else if (absoluteZ > player.MediumWalkMinAngle & absoluteZ < player.FastWalkMinAngle)
        { //Medium walk
            return WalkSpeed.Medium;
        }
        else
        {
            return WalkSpeed.Fast;
        }

    }

    private void SetSpeed()
    {
        //Determine target direction. -1 = right, 1 = left
        player.walkDirection = player.input.aimAngle < 0.0f ? 1.0f : -1.0f;

        // Determine target speed based on rotation
        switch (walkSpeedEnum)
        {
            case WalkSpeed.Stop:
                player.walkCurrentSpeed = 0.0f;
                break;
            case WalkSpeed.Slow:
                player.walkCurrentSpeed = Helper.RemapArbitraryValues(15.0f, 25.0f, player.slowWalkSpeed, player.mediumWalkSpeed, absoluteZ); //Remap makes the actual speed smooth between different speed thresholds.
                break;
            case WalkSpeed.Medium:
                player.walkCurrentSpeed = Helper.RemapArbitraryValues(25.0f, 35.0f, player.mediumWalkSpeed, player.fastWalkSpeed, absoluteZ);
                break;
            case WalkSpeed.Fast:
                player.walkCurrentSpeed = player.fastWalkSpeed;
                player.ResetRechargeCounter();
                break;
        }

        player.rb.linearVelocity = new Vector3(player.walkDirection * player.walkCurrentSpeed, player.rb.linearVelocity.y, 0f);
    }

    void SetWalkAnimation()
    {
        switch (walkSpeedEnum)
        {
            case WalkSpeed.Slow:
                //Debug.Log("Set Slow!");
                PlayAnim(forwardMove[0], animNormalizedTime);
                break;
            case WalkSpeed.Medium:
                //Debug.Log("Set Medium!");
                PlayAnim(forwardMove[1], animNormalizedTime);
                break;
            case WalkSpeed.Fast:
                //Debug.Log("Set Fast!");
                PlayAnim(forwardMove[2], animNormalizedTime);
                break;
            default: //no need to do anything because we're going to idle state.
                break;
        }
    }

    private enum WalkSpeed { Stop, Slow, Medium, Fast }
}
