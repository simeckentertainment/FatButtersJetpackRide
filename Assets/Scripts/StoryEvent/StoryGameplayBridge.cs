using UnityEngine;

/// <summary>
/// Hook for story-driven gameplay state. Player input is not driven by <see cref="StoryMode"/> here —
/// use <see cref="LockControlsAction"/> / <see cref="UnlockControlsAction"/> on each step’s action list.
/// </summary>
public class StoryGameplayBridge : MonoBehaviour
{
    /// <summary>
    /// Reserved for future non-input story state (e.g. UI or camera). Does not touch <see cref="Player.input"/>.
    /// </summary>
    public void ApplyMode(StoryMode mode)
    {
    }
}