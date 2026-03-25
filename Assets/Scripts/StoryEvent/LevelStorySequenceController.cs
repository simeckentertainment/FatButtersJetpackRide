using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStorySequenceController : MonoBehaviour
{
    [SerializeField] private List<StoryStep> steps = new List<StoryStep>();
    [SerializeField] private int _currentStepIndex = 0;
    [SerializeField] private Player player;
    [SerializeField] private UIManager uiManager;

    private StoryStepContext _context;
    private bool _currentStepCompletionStarted;
    private bool _currentStepTriggerFired;

    private void Awake()
    {
        _context = new StoryStepContext
        {
            Player = player,
            UIManager = uiManager,
            Controller = this,
        };
    }

    private void Start()
    {
        _currentStepIndex = 0;
        _currentStepTriggerFired = false;
        _currentStepCompletionStarted = false;
    }

    public void NotifyTriggerFired(int stepIndex)
    {
        if (stepIndex != _currentStepIndex)
        {
            Debug.Log("Trigger ignored (current step " + _currentStepIndex + ", got " + stepIndex + ")");
            return;
        }

        if (_currentStepIndex < 0 || _currentStepIndex >= steps.Count)
        {
            Debug.LogWarning("NotifyTriggerFired: no StoryStep at index " + _currentStepIndex);
            return;
        }

        if (_currentStepTriggerFired) return;

        Debug.Log("Trigger accepted for step " + stepIndex);
        _currentStepTriggerFired = true;
        RunCurrentStepActions();
        StartCompletionForCurrentStep();
    }

    private void RunCurrentStepActions()
    {
        var step = steps[_currentStepIndex];
        if (step.actions == null || step.actions.Count == 0)
            return;

        foreach (var action in step.actions)
        {
            if (action != null)
                action.Execute(_context);
        }
    }


    private void StartCompletionForCurrentStep()
        {
            if (_currentStepCompletionStarted) return;
            if (_currentStepIndex < 0 || _currentStepIndex >= steps.Count) return;

            var step = steps[_currentStepIndex];
            _currentStepCompletionStarted = true;

            switch (step.completionType)
            {
                case CompletionType.Instant:
                    AdvanceStep();
                    break;

                case CompletionType.AfterTimer:
                case CompletionType.AfterDialogueDuration:
                    StartCoroutine(CompleteAfterTimer(step.completionTimerDuration));
                    break;

                // Step 8+ (signal/manual) can be added later
                default:
                    _currentStepCompletionStarted = false; // optional safety for unhandled types
                    break;
            }
        }

    private void TryEnterCurrentStep()
    {
        if (_currentStepIndex <0 || _currentStepIndex >= steps.Count) return;
        
        var step = steps[_currentStepIndex];
        if (step.runImmediately)
        {
            _currentStepTriggerFired = true;
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
        _currentStepIndex++;
        _currentStepCompletionStarted = false;
        _currentStepTriggerFired = false;
        Debug.Log("Advanced to step " + _currentStepIndex);

        TryEnterCurrentStep();
    }
}