using UnityEngine;

public class FuelPickup : Pickup
{
    [SerializeField] private float fuelAmount = 25;

    private void Start()
    {
        fuelAmount *= transform.parent.localScale.x;
    }

    protected override void OnPlayerTriggerEnter(Player player)
    {
        player.PickUpFuel(fuelAmount);
    }
}
