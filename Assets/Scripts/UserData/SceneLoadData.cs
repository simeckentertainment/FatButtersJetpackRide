using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName ="newSceneLoadData", menuName ="Data/SceneLoadData")]
public class SceneLoadData : ScriptableObject
{
    public string SceneToLoad;
    public string LastLoadedLevel;
    public int LastLoadedLevelInt;
    public int adHistoryCounter;
}
