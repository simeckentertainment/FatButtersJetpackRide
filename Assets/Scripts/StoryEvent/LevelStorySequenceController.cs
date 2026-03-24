using UnityEngine;

public class LevelStorySequenceController : MonoBehaviour
{
    [SerializeField] private int currentStepIndex = 0;
    [SerializeField] private Player player;
    [SerializeField] private UIManager uiManager;

    public void NotifyTriggerFired(int stepIndex)
    {
        if (stepIndex == currentStepIndex)
        {
            Debug.Log("Trigger accepted for step " + stepIndex);
        }
        else
        {
            Debug.Log("Trigger ignored (current step " + currentStepIndex + ", got " + stepIndex + ")");
        }
    }
}