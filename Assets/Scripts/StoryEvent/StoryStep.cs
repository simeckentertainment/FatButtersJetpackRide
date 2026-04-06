using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StoryStep
{
    [Tooltip("Step index this trigger belongs to (e,g, 0=first step). Trigger fires when controller is on this step.")]
    public int triggerStepIndex;

    [Tooltip("Completion type for this step")]
    public CompletionType completionType = CompletionType.Instant;

    [Tooltip("For AfterTimer: duration in seconds.")]
    public float completionTimerDuration = 3f;

    [Tooltip("For AfterGameplaySignal: signal ID to wait for.")]
    public string completionSignalId = GameplaySignal.ThrustUsedSignalId;

    [Tooltip("Actions to run when step is entered (in order).")]
    public List<StoryActionBase> actions = new List<StoryActionBase>();

    [Tooltip("If true, this step doesn't require a trigger; run actions as soon as step is entered.")]
    public bool runImmediately;
}