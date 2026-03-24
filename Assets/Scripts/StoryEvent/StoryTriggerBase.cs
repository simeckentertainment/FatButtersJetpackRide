using UnityEngine;

public abstract class StoryTriggerBase : MonoBehaviour
{
    [SerializeField] protected LevelStorySequenceController controller;
    [SerializeField] protected int stepIndex;
    
    protected void NotifyController()
    {
        if (controller != null)
            controller.NotifyTriggerFired(stepIndex);
    }

    protected virtual void OnValidate()
    {
        if (controller == null)
            controller = FindFirstObjectByType<LevelStorySequenceController>();
    }

}