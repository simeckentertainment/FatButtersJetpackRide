using System.Collections;
using UnityEngine;

public class TimerTrigger : StoryTriggerBase
{
    [SerializeField] private float delaySeconds = 1f;
    [SerializeField] private bool startOnEnable = true;

    private void OnEnable()
    {
        if(startOnEnable)
            StartCoroutine(RunTimer());
    }

    public void StartTimer()
    {
        StartCoroutine(RunTimer());
    }

    private IEnumerator RunTimer(){
        yield return new WaitForSeconds(delaySeconds);
        NotifyController();
    }
}