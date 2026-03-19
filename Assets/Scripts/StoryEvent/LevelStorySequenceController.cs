using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStorySequenceController : MonoBehaviour
{
    [SerializeField] private List<StoryStep> steps = new List<StoryStep>();
    [SerializeField] private Player player;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private StoryGameplayBridge gameplayBridge;

    private int _currentStepIndex;
    private bool _currentStepTriggerFired;
    private bool _currentStepCompletionStarted;
    private string _subscribedCompletionSignalId;
    private StoryStepContext _context;

    private void Awake()
    {
        _context = new StoryStepContext{
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
        TryEnterCurrentStep();
    }

    public void NotifyTriggerFired(int stepIndex)
    {
        if (stepIndex != _currentStepIndex) return;
        if (_currentStepIndex < 0 || _currentStepIndex >= steps.Count) return;

        _currentStepTriggerFired = true;
        RunCurrentStepActions();
    }

    private void TryEnterCurrentStep()
    {
        if (_currentStepIndex < 0 || _currentStepIndex >= steps.Count) return;

        var step = steps[_currentStepIndex];
        if (step.runImmediately)
        {
            _currentStepTriggerFired = true;
            RunCurrentStepActions();
        }
    }

    private void RunCurrentStepActions()
    {
        if (_currentStepIndex < 0 || _currentStepIndex >= steps.Count) return;
        var step = steps[_currentStepIndex];

        if (gameplayBridge != null)
            gameplayBridge.ApplyMode(step.requestedMode);

        if (step.actions != null)
        {
            foreach (var action in step.actions)
            {
                if (action != null)
                    action.Excute(_context);
            }
        }

        StartCompletionForCurrentStep();
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
            case CompletionType.AfterTime:
            case CompletionType.AfterDialogueDuration:
                StartCoroutine(CompleteAfterTimer(step.completionTimerDuration));
                break;
            case CompletionType.AfterGameplaySignal:
                _subscribedCompletionSignalId = step.completionSignalId;
                GameplaySignal.Subscribe(_subscribedCompletionSignalId, OnCompletionSignalRaised);
                break;
            case CompletionType.Manual:
                _subscribedCompletionSignalId = GameplaySignal.StepCompleteSignalId;
                GameplaySignal.Subscribe(_subscribedCompletionSignalId, OnCompletionSignalRaised);
                break;
        }
    }

    private void OnCompletionSignalRaised()
    {
        if (!string.IsNullOrEmpty(_subscribedCompletionSignalId))
        {
            GameplaySignal.Unsubscribe(_subscribedCompletionSignalId, OnCompletionSignalRaised);
            _subscribedCompletionSignalId = null;
        }
        AdvanceStep();
    }

    private IEnumerator CompleteAfterTimer(float duration)
    {
        yield return new WaitForSeconds(duration);
        AdvanceStep();
    }

    private void AdvanceStep()
    {
        _currentStepCompletionStarted = false;
        _currentStepTriggerFired = false;
        _currentStepIndex++;

        if (_currentStepIndex >= steps.count)                                                                                                                                                           )
    }

    public StoryGameplayBridge GetGameplayBridge() => gameplayBridge;
}
