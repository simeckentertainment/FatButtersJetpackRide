using UnityEngine;

public class FoodPickup : Pickup
{
    [SerializeField] private float foodAmount = 25;

    private void Start()
    {
        foodAmount *= transform.parent.localScale.x;
    }

    protected override void OnPlayerTriggerEnter(Player player)
    {
        player.PickUpFoods(foodAmount);
    }
}
