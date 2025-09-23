using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ThatSeemsDirtyTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other){
        if(other.gameObject.tag == "Player"){
            //Social.ReportProgress("CgkI293vto8EEAIQCA", 100.0f, (bool success) => {Debug.Log("That seems dirty success!");});
        }
    }
}
