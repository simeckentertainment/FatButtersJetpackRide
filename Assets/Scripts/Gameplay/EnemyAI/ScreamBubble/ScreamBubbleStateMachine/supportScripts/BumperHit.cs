using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BumperHit : MonoBehaviour
{
    [SerializeField] ScreamBubble bubble;

    private void OnTriggerEnter(Collider other) {
        bubble.hitWall = true;
    }
}
