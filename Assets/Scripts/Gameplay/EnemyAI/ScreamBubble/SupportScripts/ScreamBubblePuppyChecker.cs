using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreamBubblePuppyChecker : MonoBehaviour
{
    [SerializeField] ScreamBubble bubble;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            bubble.PlayerInSightDistance = true;
            bubble.target = other.gameObject;
        }
    }
    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Player")
        {
            bubble.PlayerInSightDistance = false;
        }
    }
}
