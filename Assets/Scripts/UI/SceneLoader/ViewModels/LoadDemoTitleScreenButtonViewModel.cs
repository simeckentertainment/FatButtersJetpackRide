public class LoadDemoTitleScreenButtonViewModel : ButtonViewModel<SceneLoaderModel>
{
    protected override void OnClick()
    {
        Model.LoadDemoTitleScene();
    }
}
