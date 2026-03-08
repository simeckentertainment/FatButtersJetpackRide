using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class DemoVideoControls : MonoBehaviour
{
[SerializeField] private VideoPlayer videoPlayer;
[SerializeField] private SceneLoadData sld;

    void Start()
    {
        videoPlayer.loopPointReached += OnVideoFinished;
        videoPlayer.Play();
    }

    void Update()
    {
        // Check for any touch beginning (avoids firing repeatedly while held)
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            LoadNextScene();
        }
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        LoadNextScene();
    }

    private void LoadNextScene()
    {
        // Unsubscribe before we leave to be safe
        videoPlayer.loopPointReached -= OnVideoFinished;
        if("Scenes/" + sld.LastLoadedLevel == Levels.GameOnDemoTitleScreen)
        {
            Levels.Load(Levels.GameOnDemoLevel);
        } else {
            Levels.Load(Levels.GameOnDemoTitleScreen);
        }
    }

    void OnDestroy()
    {
        videoPlayer.loopPointReached -= OnVideoFinished;
    }
}
