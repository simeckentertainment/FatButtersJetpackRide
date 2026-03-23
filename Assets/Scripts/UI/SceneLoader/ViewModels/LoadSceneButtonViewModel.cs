using UnityEngine;

public class LoadSceneButtonViewModel : ButtonViewModel<SceneLoaderModel>
{
    [SerializeField] private string sceneName;

    protected override void OnClick()
    {
        Model.LoadScene(sceneName);
    }
}
