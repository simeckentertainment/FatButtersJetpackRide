using UnityEngine;

/// <summary>
/// Hook for story-driven gameplay state. Player input is not driven by <see cref="StoryMode"/> here —
/// use <see cref="LockControlsAction"/> / <see cref="UnlockControlsAction"/> on each step’s action list.
/// </summary>
public class StoryGameplayBridge : MonoBehaviour
{
    /// <summary>
    /// Reserved for future non-input story state (UI/camera). Currently logs the requested mode so
    /// callers (e.g. RequestGameplayStateAction) are provably wired end-to-end; extend per-mode behavior
    /// here as UI/camera story needs are defined. Does not touch <see cref="Player.input"/>.
    /// </summary>
    public void ApplyMode(StoryMode mode)
    {
        Debug.Log($"[StoryGameplayBridge] ApplyMode requested: {mode}");
    }
}