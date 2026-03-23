using System.Collections.Generic;
using UnityEngine;

public class NearGroundChecker : MonoBehaviour
{
    [SerializeField] private Player player;

    private HashSet<int> currentGroundColliders = new HashSet<int>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Untagged")
        {
            var otherId = other.GetInstanceID();

            if (!currentGroundColliders.Contains(otherId))
            {
                currentGroundColliders.Add(otherId);
            }

            if (currentGroundColliders.Count > 0)
            {
                player.GroundNear = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Untagged")
        {
            var otherId = other.GetInstanceID();

            if (currentGroundColliders.Contains(otherId))
            {
                currentGroundColliders.Remove(otherId);
            }

            if (currentGroundColliders.Count == 0)
            {
                player.GroundNear = false;
            }
        }
    }
}
