using UnityEngine;

public class FoodPickup : Pickup
{
    [SerializeField] private float foodAmount = 25;

    private void Start()
    {
        if (transform.parent != null)
        {
            foodAmount *= transform.parent.localScale.x;
        }
    }

    protected override void OnPlayerTriggerEnter(Player player)
    {
        player.PickUpFoods(foodAmount);
    }
}
