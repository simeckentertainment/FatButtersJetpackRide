using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class ForceFieldRelease : MonoBehaviour
{
    public bool Triggered;
    public float MoveCounter;
    public float MoveThreshold;
    [SerializeField] Transform GoalTrans;
    Vector3 goalPos;
    Vector3 initialPos;
    // Start is called before the first frame update
    void Start()
    {
        Triggered = false;
        initialPos = transform.position;
        goalPos = GoalTrans.position;
    }

    // Update is called once per frame
    void Update()
    {
        
        if(Triggered){
            MoveCounter++;
            transform.position = Vector3.Lerp(initialPos, goalPos, MoveCounter/MoveThreshold);
            Triggered = (MoveCounter < MoveThreshold) ? true : false;

        }
    }
}
