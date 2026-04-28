using UnityEngine;

public class DisableCorgiSenseAction : StoryActionBase
{
    public override void Execute(StoryStepContext context)
    {
        if (SaveManager.Instance?.collectibleData == null) return;
        SaveManager.Instance.collectibleData.CorgiSenseEnabled = false;
        Debug.Log("StoryEvent: CorgiSenseEnabled = false");
    }
}