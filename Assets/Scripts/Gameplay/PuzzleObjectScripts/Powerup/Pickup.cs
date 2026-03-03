using UnityEngine;

public abstract class Pickup : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerCollisionReporter>(out var playerCollision))
        {
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
