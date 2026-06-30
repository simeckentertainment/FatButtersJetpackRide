using UnityEngine;

public class HarmfulObject : MonoBehaviour
{
    [SerializeField] private float damage;

    [SerializeField] private bool damageOnCollision = true;
    [SerializeField] private bool damageOnTrigger = true;
    [SerializeField] public bool PlayerCollisionDetected;

    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (damageOnCollision)
        {
            HandleCollision(collision.collider.GetComponentInParent<Player>());
        }
    }
    protected virtual void OnCollisionExit(Collision collision)
    {
        if(collision.collider.GetComponentInParent<Player>()){
            PlayerCollisionDetected = false;
        }
    }
    protected virtual void OnTriggerExit(Collider other)
    {
        if(other.GetComponentInParent<Player>()){
            PlayerCollisionDetected = false;
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (damageOnTrigger)
        {
            HandleCollision(other.GetComponentInParent<Player>());
        }
    }

    private void HandleCollision(Player player)
    {
        if (player != null && player.IsAlive)
        {
            OnPlayerTouched(player);
        }
    }

    protected virtual void OnPlayerTouched(Player player)
    {
        PlayerCollisionDetected = true;
        player.HarmfulTouch = true;
        player.HarmfulDamageAmount = damage;
        player.HarmfulTouchObjectPosition = this.transform.position;
    }
}
