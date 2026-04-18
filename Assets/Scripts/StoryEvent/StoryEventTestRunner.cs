using UnityEngine;

public class StoryEventTestRunner : MonoBehaviour
{
    private void Start()
    {
        GameplaySignal.Subscribe(SignalId.ThrustUsed, OnThrustUsed);
    }

    private void OnDestroy()
    {
        GameplaySignal.Unsubscribe(SignalId.ThrustUsed, OnThrustUsed);
    }

    private void OnThrustUsed()
    {
        Debug.Log("ThrustUsed received");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            GameplaySignal.Raise(SignalId.ThrustUsed);
        }
    }
}