using UnityEngine;

public enum StoryMode
{
    Gameplay,
    GuidedGameplay,
    Cutscene,
    PromptMode,
    CorgiSense
}

public enum CompletionType
{
    Instant,
    AfterDialogueDuration,
    AfterTimer,
    AfterGameplaySignal,
    Manual
}