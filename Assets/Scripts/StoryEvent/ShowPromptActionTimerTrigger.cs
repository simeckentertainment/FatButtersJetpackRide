using UnityEngine;

public class ShowPromptActionTimerTrigger : StoryActionBase
{
    [SerializeField] private string title;
    [SerializeField] private string text = "Timer fired this step.";
    [SerializeField] private bool useArrowTransform;
    [SerializeField] private EditorLocalTransform arrowTransform;

    public override void Execute(StoryStepContext context)
    {
        if (context?.UIManager == null) return;
        var transformToUse = useArrowTransform ? arrowTransform : EditorLocalTransform.Identity;
        var message = string.IsNullOrEmpty(title) ? text : $"{title}\n{text}";
        context.UIManager.ShowInfoText(message, transformToUse, useArrowTransform);
    }
}