using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NosePokeTrigger : MonoBehaviour
{
    [SerializeField] Furret furret;
    // Start is called before the first frame update

    void OnTriggerEnter(Collider other){
        if(other.gameObject.tag == "Player"){
            furret.nosePokeTriggered = true;
        }
    }
}
