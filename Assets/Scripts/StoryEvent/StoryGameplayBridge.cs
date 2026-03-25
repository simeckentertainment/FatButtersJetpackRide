using UnityEngine;

public class StoryGameplayBridge : MonoBehaviour
{
    [SerializeField] private Player player;

    public void ApplyMode(StoryMode mode)
    {
        if(player?.input != null) return;
        switch(mode)
        {
            case StoryMode.Gameplay:
            case StoryMode.GuidedGameplay:
            case StoryMode.CorgiSense:
                player.input.EnableInput();
                break;
            case StoryMode.Cutscene:
            case StoryMode.PromptMode:
                player.input.DisableInput();
                break;
            default:
                player.input.EnableInput();
                break;
        }
    }
}