using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CBBPlayerHitTracker : MonoBehaviour
{
    [SerializeField] CowboyBoss cowboyBoss;

    void OnCollisionEnter(Collision collision){
        cowboyBoss.hitThisFrame = collision.gameObject.tag == "Player" ? true : false;
    }
}
