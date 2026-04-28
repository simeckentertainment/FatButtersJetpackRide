using UnityEngine;

public abstract class StoryActionBase : MonoBehaviour
{
    public abstract void Execute(StoryStepContext context);
}
