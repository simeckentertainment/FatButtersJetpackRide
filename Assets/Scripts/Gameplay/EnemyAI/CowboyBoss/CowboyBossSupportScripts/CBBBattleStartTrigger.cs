using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CBBBattleStartTrigger : MonoBehaviour
{
    [SerializeField] CowboyBoss cbb;
    // Start is called before the first frame update
    void Start(){
        cbb = FindFirstObjectByType<CowboyBoss>();
    }
    void OnTriggerEnter(Collider other){
        if(other.gameObject.tag == "Player"){
            cbb.BattleHasBegun = true;
        }
    }
}
