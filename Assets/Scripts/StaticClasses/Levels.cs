using UnityEngine.SceneManagement;

public class Levels
{
    public const string AnalyticsChecker = "Scenes/P18_AnalyticsChecker_SCN_V001_RSS";
    public const string AgeGate = "Scenes/P18_AgeGate_SCN_V002_RSS";
    public const string TitleScreen = "Scenes/P18_TitleScreen_SCN_V001_RSS";
    public const string SceneLoader = "Scenes/P18_SceneLoader_SCN_V001_RSS";
    public const string LevelSelect = "Scenes/P18_LevelSelect_SCN_V002_RSS";
    public const string SaveDataInitializer = "Scenes/P18_SaveDataInitializer_SCN_V001_RSS";
    public const string LevelPrefix = "Scenes/puzzleLevels/";
    public const string GameOnDemoTitleScreen = "Scenes/P18-GameOnDemoTitleScreen";
    public const string GameOnDemoLevel = "Scenes/puzzleLevels/GameOnExpoDemoLevel";

    private static SceneLoadData sceneLoadData => SaveManager.Instance.sceneLoadData;

    public static void Load(string levelName)
    {
        sceneLoadData.SceneToLoad = levelName;
        sceneLoadData.LastLoadedLevelInt = 0;
        sceneLoadData.LastLoadedLevel = SceneManager.GetActiveScene().name;
        SaveManager.Instance.Save();
        SceneManager.LoadScene(Levels.SceneLoader);
    }

    public static void Load(int levelNumber)
    {
        sceneLoadData.SceneToLoad = LevelPrefix + "P18_Level" + levelNumber.ToString() + "_SCN_V001_RSS";
        sceneLoadData.LastLoadedLevelInt = levelNumber;
        sceneLoadData.LastLoadedLevel = "";
        SaveManager.Instance.Save();
        SceneManager.LoadScene(SceneLoader);
    }
}
