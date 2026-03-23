public class BonePickup : Pickup
{
    protected override void OnPlayerTriggerEnter(Player player)
    {
        player.PickUpBones();
    }
}
