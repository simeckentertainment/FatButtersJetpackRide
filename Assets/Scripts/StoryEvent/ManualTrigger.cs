using UnityEngine;
using UnityEngine.Events;

public class ManualTrigger : StoryTriggerBase
{
    [SerializeField] private UnityEvent onTriggerButton;

    public void Trigger()
    {
        NotifyController();
    }

    private void OnEnable()
    {
        if (onTriggerButton != null)
            onTriggerButton.AddListener(Trigger);
    }

    private void OnDisable()
    {
        if (onTriggerButton != null)
            onTriggerButton.RemoveListener(Trigger);
    }
}