using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostBehavior : MonoBehaviour
{
    RaycastHit LookRay;
    [SerializeField] Vector3 oldPos;
    [SerializeField] Vector3 targetPos;
    [SerializeField] Vector3 targetDir;
    [SerializeField] float range = 20.0f;
    [SerializeField] float timerMax = 180f;
    float timer;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0.0f;
        CheckDirection();
    }

    // Update is called once per frame
    void Update()
    {
        timer+=1;
        transform.position = Vector3.Lerp(oldPos,targetPos,timer/timerMax);
        if(Helper.isWithinMarginOfError(transform.position,targetPos,1.0f)){
            CheckDirection();
        }
    }


    void CheckDirection(){
        timer = 0.0f;
        oldPos = transform.position;
        float x = Random.Range(transform.position.x-range,transform.position.x+range);
        float y = Random.Range(transform.position.y-range,transform.position.y+range);
        Vector3 maxTargetPos = new Vector3(x,y,transform.position.z);
        targetDir = (maxTargetPos-transform.position).normalized;
        if(Physics.Raycast(transform.position,targetDir,out LookRay,range)){
            float newDist = Vector3.Distance(transform.position,LookRay.point);
            newDist = newDist*.8f;
            targetPos = oldPos+(targetDir*newDist);
        } else {
            targetPos = maxTargetPos;
        }
    }
}
