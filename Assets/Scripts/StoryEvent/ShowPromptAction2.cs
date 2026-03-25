using UnityEngine;

public class ShowPromptAction2 : StoryActionBase
{
    [SerializeField] private string title = "Tutorial";
    [SerializeField] private string text = "Press thrust or tilt the phone to fly";
    [SerializeField] private bool useArrowTransform;
    [SerializeField] private EditorLocalTransform arrowTransform;

    public override void Execute(StoryStepContext context)
    {
        if (context?.UIManager == null) return;
        var transformToUse = useArrowTransform ? arrowTransform : EditorLocalTransform.Identity;
        context.UIManager.ShowInfoText(title, text, transformToUse);
    }
}