using UnityEngine;

public class StoryEventTestRunner : MonoBehaviour
{
    private void Start()
    {
        GameplaySignal.Subscribe(GameplaySignal.ThrustUsedSignalId, OnThrustUsed);
    }

    private void OnDestroy()
    {
        GameplaySignal.Unsubscribe(GameplaySignal.ThrustUsedSignalId, OnThrustUsed);
    }

    private void OnThrustUsed()
    {
        Debug.Log("ThrustUsed received");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            GameplaySignal.Raise(GameplaySignal.ThrustUsedSignalId);
        }
    }
}