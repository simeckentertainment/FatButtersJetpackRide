using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingRockTriggerScript : MonoBehaviour
{
    [SerializeField] Rigidbody rb;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            rb.useGravity = true;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.tag == "OneHitKill")
        {
            Destroy(gameObject);
        }
    }

}
