using UnityEngine;

public enum StoryMode
{
    Gameplay,
    GuideGameplay,
    Cutscene,
    PromptMode,
    CorgiSense
}

public enum CompletionType
{
    instant,
    AfterDialogueDuration,
    AfterTimer,
    AfterGameplaySignal,
    Manual
}