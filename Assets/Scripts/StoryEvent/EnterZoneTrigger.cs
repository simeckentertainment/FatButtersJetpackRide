using UnityEngine;

public class EnterZoneTrigger : StoryTriggerBase
{
    [SerializeField] private string requiredTag = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (other == null || string.IsNullOrEmpty(requiredTag) ) return;
        if (other.CompareTag(requiredTag))
            NotifyController();
    }
}