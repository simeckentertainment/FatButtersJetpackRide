using System;
using System.Collections.Generic;
using UnityEngine;

public static class GameplaySignal
{
    public const string ThrustUsedSignalId = "ThrustUsed";
    public const string ObjectiveCompleteSignalId = "ObjectiveComplete";
    public const string StepCompleteSignalId = "StepComplete";

    private static readonly Dictionary<string, Action> Handlers = new Dictionary<string, Action>();

    public static void Subscribe(string signalId, Action callback)
    {
        if (string.IsNullOrEmpty(signalId) || callback == null) return;
        if (!Handlers.ContainsKey(signalId))
            Handlers[signalId] = null;
        Handlers[signalId] += callback;
    }

    public static void Unsubscribe(string signalId, Action callback)
    {
        if (string.IsNullOrEmpty(signalId) || !Handlers.ContainsKey(signalId)) return;
        Handlers[signalId] -= callback;
    }

    public static void Raise(string signalId)
    {
        if (string.IsNullOrEmpty(signalId) || !Handlers.ContainsKey(signalId)) return;
        Handlers[signalId]?.Invoke();
    }
}