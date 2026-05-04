using UnityEngine;

public class BallPickup : Pickup
{
    [SerializeField] private GameObject pickupEffect;

    protected override void OnPlayerTriggerEnter(Player player)
    {
        Instantiate(pickupEffect, transform.position, Quaternion.identity);

        player.PickUpBalls();
    }
}
