using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngleMachine : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] float currentAngle;
    [SerializeField] float goalAngle;
    float realGoalAngle;
    [SerializeField] WheelMachine axel;
    [SerializeField] SegwayBear segwayBear;

    [SerializeField]public bool noCompPeriod;
    [SerializeField] int noCompTimer;
    int noCompTimerMax = 50;
    void Update()
    {
        if(goalAngle < 0.0f){
            realGoalAngle = 360.0f-goalAngle;
        } else {
            realGoalAngle = goalAngle;
        }
        if(noCompPeriod){
            noCompTimer ++;
            rb.centerOfMass = Vector3.zero;
            if(noCompTimer >= noCompTimerMax){
                noCompTimer = 0;
                noCompPeriod = false;
            }
        } else {
            CompensateAngle();
        }
    }

    private void CompensateAngle()
    {    //TODO: Manual rotation for comedy.
        currentAngle = segwayBear.transform.rotation.eulerAngles.x;
        if (currentAngle != 0) //simpler, working code for comparison.
        {
            if (currentAngle > 180){
                PushRight();
            } else if (currentAngle < 180){
                PushLeft();
            }
        }
    }

    public void StartNewNoCompPeriod(int timerMax){
        noCompTimerMax = timerMax;
        noCompPeriod = true;
    }

    public void PushLeft(){
        rb.centerOfMass = new Vector3(0, 0, -5);
        axel.compensationRotation = (currentAngle) * -10.0f;
    }
    public void PushRight(){
        rb.centerOfMass = new Vector3(0, 0, 5);
        axel.compensationRotation = (360 - currentAngle) * 10.0f;
    }
}

