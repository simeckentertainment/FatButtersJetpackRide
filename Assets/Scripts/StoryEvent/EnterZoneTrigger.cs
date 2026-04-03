using UnityEngine;

public class EnterZoneTrigger : StoryTriggerBase
{
    private void OnTriggerEnter(Collider other)
    {
        if (other == null) return;
        var player = other.GetComponentInParent<Player>();
        if (player != null && player.IsAlive)
            NotifyController();
    }
}