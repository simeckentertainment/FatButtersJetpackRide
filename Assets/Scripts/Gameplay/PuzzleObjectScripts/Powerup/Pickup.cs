using Solo.MOST_IN_ONE;
using UnityEngine;

public abstract class Pickup : MonoBehaviour
{
    private bool isPickedUp = false; // fixes a case where collision reporters collide with the pickup simultaneously

    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponentInParent<Player>();
        if (!isPickedUp && player != null)
        {
            isPickedUp = true;
            if (player.IsAlive)
            {
                OnPlayerTriggerEnter(player);
                MOST_HapticFeedback.Generate(MOST_HapticFeedback.HapticTypes.SoftImpact);
                Destroy(this.gameObject);
            }
        }
    }

    protected abstract void OnPlayerTriggerEnter(Player player);
}
