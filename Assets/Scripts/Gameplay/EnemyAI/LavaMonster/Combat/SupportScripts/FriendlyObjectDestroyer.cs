using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendlyObjectDestroyer : MonoBehaviour
{
    [SerializeField] LavaMonster lavaMonster;
    [SerializeField] Player player;
    // Start is called before the first frame update
    void Start()
    {
        player = Helper.NabPlayerObj().GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Friendly")
        {
            Destroy(other.gameObject);
        }

        if (other.gameObject.tag == "EventTrigger")
        {
            lavaMonster.dieNow = true;
        }
        if (other.gameObject.tag == "Player")
        {
            if (player.saveManager.collectibleData.HASBALL)
            {
                lavaMonster.ActivateHitWithBallPowerup();
            }
        }
    }
private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Friendly")
        {
            Destroy(other.gameObject);
        }
        if(other.gameObject.tag == "EventTrigger")
        {
            lavaMonster.dieNow = true;
        }
    }
}
