using UnityEngine;

public abstract class Pickup : MonoBehaviour
{
    private bool isPickedUp = false; // fixes a case where collision reporters collide with the pickup simultaneously

    private void OnTriggerEnter(Collider other)
    {
        if (!isPickedUp && other.TryGetComponent<PlayerCollisionReporter>(out var playerCollision))
        {
            isPickedUp = true;
            var player = playerCollision.player;
            if (player.IsAlive)
            {
                OnPlayerTriggerEnter(player);
                Destroy(this.gameObject);
            }
        }
    }

    protected abstract void OnPlayerTriggerEnter(Player player);
}
