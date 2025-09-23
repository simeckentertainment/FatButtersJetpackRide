using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class outpaceMagmarTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other){
        if(other.gameObject.tag == "Player"){
            //Social.ReportProgress(GPGSIds.achievement_outpace_magmar, 100.0f, (bool success) => {Debug.Log("Magmar outpaced!");});
        }
    }
}
