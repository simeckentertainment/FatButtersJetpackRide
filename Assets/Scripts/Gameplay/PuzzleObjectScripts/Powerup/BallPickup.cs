public class BallPickup : Pickup
{
    protected override void OnPlayerTriggerEnter(Player player)
    {
        player.PickUpBalls();
    }
}
