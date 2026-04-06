using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStorySequenceController : MonoBehaviour
{
    [SerializeField] private List<StoryStep> steps = new List<StoryStep>();
    [SerializeField] private int currentStepIndex = 0;
    [SerializeField] private Player player;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private StoryGameplayBridge gameplayBridge;

    private StoryStepContext context;
    private bool currentStepCompletionStarted;
    private bool currentStepTriggerFired;
    private string subscribedCompletionSignalId;

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
        currentStepTriggerFired = false;
        currentStepCompletionStarted = false;
    }

    public void TryFireTrigger(int stepIndex)
    {
        if (stepIndex != currentStepIndex)
        {
            Debug.Log($"Trigger ignored (current step {currentStepIndex}, got {stepIndex})");
            return;
        }

        if (currentStepIndex < 0 || currentStepIndex >= steps.Count)
        {
            Debug.LogWarning("TryFireTrigger: no StoryStep at index " + currentStepIndex);
            return;
        }

        if (currentStepTriggerFired) return;

        Debug.Log("Trigger accepted for step " + stepIndex);
        currentStepTriggerFired = true;
        RunCurrentStepActions();
        StartCompletionForCurrentStep();
    }

    private void RunCurrentStepActions()
    {
        if (currentStepIndex < 0 || currentStepIndex >= steps.Count) return;
        var step = steps[currentStepIndex];

        if (step.actions != null)
        {
            foreach (var action in step.actions)
            {
                if (action != null)
                    action.Execute(context);
            }
        }

        StartCompletionForCurrentStep();
    }


    private void StartCompletionForCurrentStep()
    {    
        if (currentStepCompletionStarted) return;
        if (currentStepIndex < 0 || currentStepIndex >= steps.Count) return;

        var step = steps[currentStepIndex];
        currentStepCompletionStarted = true;

        switch (step.completionType)
        {
            case CompletionType.Instant:
                AdvanceStep();
                break;

            case CompletionType.AfterTimer:
            case CompletionType.AfterDialogueDuration:
                StartCoroutine(CompleteAfterTimer(step.completionTimerDuration));
                break;
            case CompletionType.AfterGameplaySignal:
                subscribedCompletionSignalId = step.completionSignalId;
                GameplaySignal.Subscribe(subscribedCompletionSignalId, OnCompletionSignalRaised);
                break;
        }    
    }

    private void OnCompletionSignalRaised()
    {
        if (!string.IsNullOrEmpty(subscribedCompletionSignalId))
        {
            GameplaySignal.Unsubscribe(subscribedCompletionSignalId, OnCompletionSignalRaised);
            subscribedCompletionSignalId = null;
        }
        AdvanceStep();
    }

    private void TryEnterCurrentStep()
    {
        if (currentStepIndex <0 || currentStepIndex >= steps.Count) return;
        
        var step = steps[currentStepIndex];
        if (step.runImmediately)
        {
            currentStepTriggerFired = true;
            RunCurrentStepActions();
            StartCompletionForCurrentStep();
        }
    }

    private IEnumerator CompleteAfterTimer(float duration)
    {
        yield return new WaitForSeconds(duration);
        AdvanceStep();
    }

    private void AdvanceStep()
    {
        currentStepIndex++;
        currentStepCompletionStarted = false;
        currentStepTriggerFired = false;
        Debug.Log("Advanced to step " + currentStepIndex);

        TryEnterCurrentStep();
    }

    public StoryGameplayBridge GetGameplayBridge() => gameplayBridge;
}