using UnityEngine;

public class EnableCorgiSenseAction : StoryActionBase
{
    public override void Execute(StoryStepContext context)
    {
        if (SaveManager.Instance?.collectibleData == null) return;
        SaveManager.Instance.collectibleData.CorgiSenseEnabled = true;
        Debug.Log("StoryEvent: CorgiSenseEnabled = true");
    }
}