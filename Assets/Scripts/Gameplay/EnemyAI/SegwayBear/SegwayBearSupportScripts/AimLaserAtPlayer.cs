using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimLaserAtPlayer : MonoBehaviour
{
    [SerializeField] SegwayBear segwayBear;


    // Update is called once per frame
    void Update()
    {
        
        if(segwayBear.AdjustingLaser){
            transform.LookAt(segwayBear.target,Vector3.up);
        }
    }
}
