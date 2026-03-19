using UnityEngine;

public abstract class StoryTriggerBase : MonoBehaviour
{
    [SerializeField] protected LevelStorySequenceController Controller;
    [SerializeField] protected int stepIndex;
    
    protected void NotifyController()
    {
        if (Controller != null)
            controller.NotifyTriggerFired(stepIndex);
    }

    protected virtual void OnValidate()
    {
        if (controller == null)
            controller = FindFirstObjectByType<LevelStorySequenceController>();
    }

}