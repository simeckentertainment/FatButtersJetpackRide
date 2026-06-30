public class MainMenuButtonViewModel : LevelSelectTopButtonViewModel
{
    protected override void OnClick()
    {
        Model.GoToMainMenu();
    }
}