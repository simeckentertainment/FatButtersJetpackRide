using UnityEngine;
using UnityEngine.Playables;

public class PlayTimelineAction : StoryActionBase
{
    [SerializeField] private PlayableDirector director;

    [Tooltip("Signal raised when the timeline finishes. Set to None to skip.")]
    [SerializeField] private SignalId signalOnFinish = SignalId.None;

    public override void Execute(StoryStepContext context)
    {
        if (director == null) return;

        director.stopped -= OnDirectorStopped;
        director.stopped += OnDirectorStopped;

        director.time = 0;
        director.Play();
    }

    private void OnDirectorStopped(PlayableDirector stoppedDirector)
    {
        if (stoppedDirector != null)
            stoppedDirector.stopped -= OnDirectorStopped;

        if (signalOnFinish != SignalId.None)
            GameplaySignal.Raise(signalOnFinish);
             Debug.Log("Timeline complete");
    }

    private void OnDisable()
    {
        if (director != null)
            director.stopped -= OnDirectorStopped;
    }
}
