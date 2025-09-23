using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlatformSensors : MonoBehaviour
{
    [SerializeField] PulleyManager pulleyManager;
    [SerializeField] WhichSensor whichSensor;

    private void OnTriggerEnter(Collider other){
        if(other.gameObject.tag == "Player"){
            if(whichSensor == WhichSensor.Left){
                pulleyManager.PlayerOnLeft = true;
            } else {
                pulleyManager.PlayerOnRight = true;
            }
        }
    }
    private void OnTriggerExit(Collider other){
        if(other.gameObject.tag == "Player"){
            if(whichSensor == WhichSensor.Left){
                pulleyManager.PlayerOnLeft = false;
            } else {
                pulleyManager.PlayerOnRight = false;
            }
        } 
    }
    enum WhichSensor {Left,Right};
}
