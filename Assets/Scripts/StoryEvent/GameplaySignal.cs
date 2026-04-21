using System;
using System.Collections.Generic;

public enum SignalId
{
    None = 0,
    ThrustUsed = 1,
    ObjectiveComplete = 2,
    StepComplete = 3,
}

public static class GameplaySignal
{
    private static readonly Dictionary<SignalId, Action> Handlers = new Dictionary<SignalId, Action>();

    public static void Subscribe(SignalId signalId, Action callback)
    {
        if (signalId == SignalId.None || callback == null) return;
        if (!Handlers.ContainsKey(signalId))
            Handlers[signalId] = null;
        Handlers[signalId] += callback;
    }

    public static void Unsubscribe(SignalId signalId, Action callback)
    {
        if (signalId == SignalId.None || !Handlers.ContainsKey(signalId)) return;
        Handlers[signalId] -= callback;
    }

    public static void Raise(SignalId signalId)
    {
        if (signalId == SignalId.None || !Handlers.ContainsKey(signalId)) return;
        Handlers[signalId]?.Invoke();
    }
}
