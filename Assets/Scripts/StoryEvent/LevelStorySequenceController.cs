using System.Collections.Generic;
using UnityEngine;

public class LevelStorySequenceController : MonoBehaviour
{
    [SerializeField] private List<StoryStep> steps = new List<StoryStep>();
    [SerializeField] private int currentStepIndex = 0;
    [SerializeField] private Player player;
    [SerializeField] private UIManager uiManager;

    private StoryStepContext context;

    private void Awake()
    {
        context = new StoryStepContext
        {
            Player = player,
            UIManager = uiManager,
            Controller = this,
        };
    }

    private void Start()
    {
        currentStepIndex = 0;
    }

    public void NotifyTriggerFired(int stepIndex)
    {
        if (stepIndex != currentStepIndex)
        {
            Debug.Log("Trigger ignored (current step " + currentStepIndex + ", got " + stepIndex + ")");
            return;
        }

        if (currentStepIndex < 0 || currentStepIndex >= steps.Count)
        {
            Debug.LogWarning("NotifyTriggerFired: no StoryStep at index " + currentStepIndex);
            return;
        }

        Debug.Log("Trigger accepted for step " + stepIndex);
        RunCurrentStepActions();
        StartCompletionForCurrentStep();
    }

    private void RunCurrentStepActions()
    {
        var step = steps[currentStepIndex];
        if (step.actions == null || step.actions.Count == 0)
            return;

        foreach (var action in step.actions)
        {
            if (action != null)
                action.Execute(context);
        }
    }


    private void StartCompletionForCurrentStep()
    {
        var step = steps[currentStepIndex];

        if (step.completionType == CompletionType.Instant)
        {
            AdvanceStep();
        }
    }

    private void AdvanceStep()
    {
        currentStepIndex++;
        Debug.Log("Advanced to step " + currentStepIndex);
    }
}