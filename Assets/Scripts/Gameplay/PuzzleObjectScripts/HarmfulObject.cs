using UnityEngine;

public class HarmfulObject : MonoBehaviour
{
    [SerializeField] private float damage;

    [SerializeField] private bool damageOnCollision = true;
    [SerializeField] private bool damageOnTrigger = true;

    private void OnCollisionEnter(Collision collision)
    {
        if (damageOnCollision)
        {
            HandleDamage(collision.collider.GetComponentInParent<Player>());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (damageOnTrigger)
        {
            HandleDamage(other.GetComponentInParent<Player>());
        }
    }

    private void HandleDamage(Player player)
    {
        if (player != null && player.IsAlive)
        {
            player.HarmfulTouch = true;
            player.HarmfulDamageAmount = damage;
            player.HarmfulTouchObjectPosition = this.transform.position;

            OnPlayerTouched(player);
        }
    }

    protected virtual void OnPlayerTouched(Player player)
    {
        // no-op
    }
}
