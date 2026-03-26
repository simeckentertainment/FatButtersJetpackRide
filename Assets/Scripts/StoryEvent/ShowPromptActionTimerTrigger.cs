using UnityEngine;

public class ShowPromptActionTimerTrigger : StoryActionBase
{
    [SerializeField] private string title = "Timer Trigger";
    [SerializeField] private string text = "Timer fired this step.";
    [SerializeField] private bool useArrowTransform;
    [SerializeField] private EditorLocalTransform arrowTransform;

    public override void Execute(StoryStepContext context)
    {
        if (context?.UIManager == null) return;
        var transformToUse = useArrowTransform ? arrowTransform : EditorLocalTransform.Identity;
        context.UIManager.ShowInfoText(title, text, transformToUse);
    }
}