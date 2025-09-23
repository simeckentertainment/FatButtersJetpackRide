using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    public static void Load(string levelName){
        SaveManager sm = Helper.NabSaveData().GetComponent<SaveManager>();
        sm.sceneLoadData.SceneToLoad = levelName;
        sm.sceneLoadData.LastLoadedLevelInt = 0;
        sm.sceneLoadData.LastLoadedLevel = SceneManager.GetActiveScene().name;
        sm.Save();
        SceneManager.LoadScene(Levels.SceneLoader);
    }
    public static void Load (int levelNumber){
        SaveManager sm = Helper.NabSaveData().GetComponent<SaveManager>();
        sm.sceneLoadData.SceneToLoad = LevelPrefix + "P18_Level" + levelNumber.ToString() + "_SCN_V001_RSS";
        sm.sceneLoadData.LastLoadedLevelInt = levelNumber;
        sm.sceneLoadData.LastLoadedLevel = "";
        sm.Save();
        SceneManager.LoadScene(SceneLoader);

    }
}
