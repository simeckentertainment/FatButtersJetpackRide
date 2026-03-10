using UnityEngine;


public class InheritWalkState : PlayerAliveState
{
    float animNormalizedTime; //since we're switching between different animations dynamically, we should handle normalized time tracking here.
    float absoluteZ;
    bool switchThisFrame;

    string[] forwardMove = { "ForeWalkSlow", "ForeWalkMid", "ForeWalkFast" };
    string[] backwardMove = { "BackWalkSlow", "BackWalkFast" };

    WalkSpeed walkSpeedEnum;
    WalkSpeed previousWalkSpeedEnum;

    int fallDelayThreshold = 20; //number of frames to wait before switching to fall state
    int fallDelayCounter;
    public InheritWalkState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
    {

    }

    public override void enter()
    {
        switchThisFrame = true;
        previousWalkSpeedEnum = WalkSpeed.Stop; //set this now to avoid errors on frame 1.
        walkSpeedEnum = GetSpeedEnum();
        fallDelayCounter = 0;
        // Initialize walk speed trackin
        if (player.animationPercentage == 0.0f)
        { //It should only ever be 0.0 on start.
          //   PlayAnim(forwardMove[Random.Range(0, 2)]);
        }
        //DeActivateGravyBoat();
        base.enter();
    }

    public override void Update()
    {
        base.Update();
    }
    
    public override void FixedUpdate()
    {

        //The player will leave the ground for a few frames while running so we need to build in a short delay before
        //switching to the fall state.
        if(!player.GroundTouch & !player.OtherObjectTouch)
        {
            fallDelayCounter++;
            TrackWalkToFallTransition();
        } else
        {
            fallDelayCounter = 0;
        }
        
        walkSpeedEnum = GetSpeedEnum();
        SetSpeed();
        if (walkSpeedEnum == WalkSpeed.Stop)
        {
            player.stateMachine.changeState(player.playerIdleState);
        }
        if (player.input.GoThrust & player.JetpackActivationPossible)
        {
            player.stateMachine.changeState(player.playerThrustState);
        }

        if(durationOfState > 0)
        {
            animNormalizedTime = GetNormalizedTime(0); //for driving mid-animation changes
        } else
        {
            animNormalizedTime = 0.0f;
        }

        if(previousWalkSpeedEnum != walkSpeedEnum){
            SetWalkAnimation();
            previousWalkSpeedEnum = walkSpeedEnum; //reset for "remembering" for next frame.
        }

        base.FixedUpdate();
    }

    public override void exit()
    {
        base.exit();
    }

    WalkSpeed GetSpeedEnum()
    {
        // Get signed Z rotation (-180 to 180)
        absoluteZ = Mathf.Abs(player.input.aimAngle);
        if (absoluteZ < 15f)
        {
            return WalkSpeed.Stop;
        }
        else if (absoluteZ >= 15f & absoluteZ < 25f)
        { //Slow walk
            return WalkSpeed.Slow;
        }
        else if (absoluteZ > 25f & absoluteZ < 35f)
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
        //Backwards walk speed is half of forward walk speed.
        if(player.walkDirection == -1.0f)
        {
            player.walkCurrentSpeed *= 0.5f;
        }
        player.rb.linearVelocity = new Vector3(player.walkDirection * player.walkCurrentSpeed, player.rb.linearVelocity.y, 0f);
    }

    void SetWalkAnimation()
    {
       if (player.walkDirection > 0.0f)
            {
                SetForwardWalkAnimWithTime();
            }
            else
            {
                SetBackwardWalkAnimWithTime();
            } 
    }

    private void SetForwardWalkAnimWithTime()
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
    
       private void SetBackwardWalkAnimWithTime()
    {
        switch (walkSpeedEnum)
        {
            case WalkSpeed.Slow:
                PlayAnim(backwardMove[0], animNormalizedTime);
                break;
            case WalkSpeed.Medium:
                PlayAnim(backwardMove[1], animNormalizedTime);
                break;
            case WalkSpeed.Fast:
                PlayAnim(backwardMove[1], animNormalizedTime);
                break;
            default: //no need to do anything because we're going to idle state.
                break;
        }
    }


        private void TrackWalkToFallTransition()
    {
        if(fallDelayCounter >= fallDelayThreshold)
        {
            player.stateMachine.changeState(player.playerFallState);
        }
    }

    private enum WalkSpeed { Stop, Slow, Medium, Fast }
}
