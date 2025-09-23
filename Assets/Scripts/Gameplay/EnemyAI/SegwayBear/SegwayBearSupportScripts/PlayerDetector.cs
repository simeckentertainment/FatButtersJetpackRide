using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    Transform PitchCube;
    [SerializeField] SegwayBear segwayBear;

    List<PlayerCollisionReporter> playerCollisionReporters;


    void Start()
    {
        playerCollisionReporters = new List<PlayerCollisionReporter>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (segwayBear.beaten) return; //If beaten, we do nothing.
        //PlayerCollisionReporter pcr = other.GetComponent<PlayerCollisionReporter>();
        //if (pcr == null)
        //{
        //    return;
        //}
        //else
        //{
        //    foreach (PlayerCollisionReporter upcr in playerCollisionReporters)
        //    {
        //        if (pcr == upcr)
        //        { //If it's already in the list, don't do anything. If it's not, move forward.
        //            return;
        //        }

        //    }
        //    playerCollisionReporters.Add(pcr);
        //}


        //if (segwayBear.target == null)
        //{ //Don't forget to aggro if necessary.
        //    Debug.Log("PlayerDetector: OnTriggerEnter called.");
        //    Debug.Log("Tag of other collider: " + other.tag);
        //    segwayBear.target = pcr.player.transform;
        //    segwayBear.detectedPlayer = true;
        //}

        if (other.tag == "PlayerDamageTrigger")
        { //Don't forget to aggro if necessary.
            Debug.Log("PlayerDetector: OnTriggerEnter called.");
            Debug.Log("Tag of other collider: " + other.tag);
            segwayBear.target = other.transform;
            segwayBear.detectedPlayer = true;
        }
    }



    private void OnTriggerExit(Collider other)
    {
        if (segwayBear.beaten) return; //If beaten, we do nothing.
        //PlayerCollisionReporter pcr = other.GetComponent<PlayerCollisionReporter>();
        ////First remove the collision reporter in question
        //foreach (PlayerCollisionReporter upcr in playerCollisionReporters)
        //{
        //    if (pcr == upcr)
        //    {
        //        playerCollisionReporters.Remove(upcr);
        //    }
        //}

        ////Then make sure that we're still needing aggro. Otherwise turn agro off.
        //if (playerCollisionReporters.Count == 0)
        //{
        //    Debug.Log("PlayerDetector: OnTriggerExit called."); 
        //    segwayBear.detectedPlayer = false;
        //    segwayBear.target = null;
        //}

        if (other.tag == "PlayerDamageTrigger")
        {
            Debug.Log("PlayerDetector: OnTriggerExit called.");
            segwayBear.detectedPlayer = false;
            segwayBear.target = null;
        }
    }
}
