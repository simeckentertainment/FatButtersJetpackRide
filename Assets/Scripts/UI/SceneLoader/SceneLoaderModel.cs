public class SceneLoaderModel : Model
{
    public void LoadScene(string name)
    {
        PauseUtility.Resume();

        Levels.Load(name);
    }

    public void LoadDemoTitleScene()
    {
        LoadScene(Levels.GameOnDemoTitleScreen);
    }
}
