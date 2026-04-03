using UnityEngine;

public class StoryGameplayBridge : MonoBehaviour
{
    [SerializeField] private Player player;

    public void ApplyMode(StoryMode mode)
    {
        if (player?.input == null) return;
        if (mode == StoryMode.Cutscene || mode == StoryMode.PromptMode)
        {
            player.input.DisableInput();
        }
        else
        {
            player.input.EnableInput();
        }
    }
}