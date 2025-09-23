using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using UnityEditor.Rendering;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
/*

[System.Serializable]
public class TextLevelTools
{

    [MenuItem("FatButters Tools/Level Tools/Compile Level")]
    private static void BuildLevelTextFile(){
        //set up my data.
        //Objects within the scene
        GameObject[] allGameObjectArr = GameObject.FindObjectsOfType<GameObject>();
        //objects within my asset structure.
        List<PlaceableObject> assetList = new List<PlaceableObject>();
        List<PlaceableObject> levelObjects = new List<PlaceableObject>();

        //Get all assets in the project.
        string[] assetPaths = AssetDatabase.GetAllAssetPaths();
        //Make sure we're only sussing out the PlaceableObjects.
        foreach (string path in assetPaths){
            if(CheckForPlaceableObject(path)){
                //assetFileList.Add(path);
                assetList.Add(AssetDatabase.LoadAssetAtPath<PlaceableObject>(path));
            }
        }
        //Now the  object list should only be filled with the PlaceableObjects in the Asset folder, including subdirs.
        //For every game object in the scene
        LevelData levelData = new LevelData();
        levelData.levelObjects = new List<LevelObject>();
        for(int i=0;i<allGameObjectArr.Length;i++){
            //If the gameobject is based on a prefab...
            if(PrefabUtility.IsPartOfPrefabInstance(allGameObjectArr[i])){
                //nab the prefab from the gameobject
                GameObject prefab = PrefabUtility.GetCorrespondingObjectFromSource(allGameObjectArr[i]) as GameObject;
                // find the corresponding asset in the list of PlaceableObjects...
                // NGL I really don't understand the syntax here but it works so 🤷‍♂️
                // Pretty sure it's ChatGPT code.
                // Dammit, I hurt myself thinking too hard about it.
                // Update: It's a lambda. I still can't use them effectively but GPT made it work so here we are.
                PlaceableObject matchingAsset = assetList.Find(asset => asset.model == prefab);
                //If there is one...
                if(matchingAsset != null){
                    //Add it to the list.
                    LevelObject obj = new LevelObject();
                    obj.name = matchingAsset.name;
                    obj.ID = matchingAsset.ID;
                    obj.position = allGameObjectArr[i].transform.position;
                    obj.rotation = allGameObjectArr[i].transform.rotation;
                    obj.scale = allGameObjectArr[i].transform.localScale;
                    obj.PrefabPath = AssetDatabase.GetAssetPath(matchingAsset.model);
                    obj.tag = allGameObjectArr[i].tag;
                    obj.ExtraData = AcquireExtraData(allGameObjectArr[i]);
                    levelData.levelObjects.Add(obj);
                }
            }
            levelData.Skybox = AssetDatabase.GetAssetPath(RenderSettings.skybox);
            levelData.PlayerPos = GameObject.Find("StateMachinePlayerPrefab").transform.position;
            levelData.song = GameObject.Find("AudioManager").GetComponent<MusicManager>().songSelector;
        }
        //further code goes here.

        BinaryFormatter binaryFormatter = new BinaryFormatter();

        if (levelObjects != null){
            string path = "Assets/Scenes/puzzleLevels/" + SceneManager.GetActiveScene().name +".butters";
            //var path = EditorUtility.SaveFilePanel("Save Buttery Level Data",
                //"Assets/Scenes/puzzleLevels/",SceneManager.GetActiveScene().name +".butters","butters");
            if (path.Length != 0){
                string json = JsonUtility.ToJson(levelData);
                StreamWriter writer = new StreamWriter(path);
                writer.WriteLine(json);
                writer.Close();
            }
        }    
        EditorUtility.DisplayDialog("Level Compiled!", "Level " + SceneManager.GetActiveScene().name + " compiled." , "Acknowledged.");
    }
    static string AcquireExtraData(GameObject obj){
        InstanceDataParser instanceDataParser = obj.GetComponent<InstanceDataParser>();
        if(instanceDataParser != null){
            return instanceDataParser.GetLevelCompileData();
        }
        return "";
    } 
    [MenuItem("FatButters Tools/Level Tools/Compile ALL Levels")]
    private static void CompileAllLevels(){
        string startingScene = SceneManager.GetActiveScene().path;
        string levelDir = "Assets/Scenes/puzzleLevels/";
        string[] levelScenes = Directory.GetFiles(levelDir, "*.unity");
        foreach (string sceneFile in levelScenes){
            EditorSceneManager.OpenScene(sceneFile);
            BuildLevelTextFile();
        }
        EditorSceneManager.OpenScene(startingScene);
    }


    [MenuItem("FatButters Tools/Level Tools/Compile Main Menu")]
    private static void BuildMMTextFile(){
        /*
        if(SceneManager.GetActiveScene().name != "MMDecorationScene"){
            if(EditorUtility.DisplayDialog("Oopsies!","You must be in the main menu Decoration Scene in order to use this tool.","Go there","Cancel")){
                EditorSceneManager.OpenScene("Assets/Scenes/MMDecorationScene.unity");
                return;
            }
        }*/ /*
        //set up my data.
        //Objects within the scene
        GameObject[] allGameObjectArr = GameObject.FindObjectsOfType<GameObject>();
        //objects within my asset structure.
        List<PlaceableObject> assetList = new List<PlaceableObject>();
        List<PlaceableObject> MMObjects = new List<PlaceableObject>();
        string[] assetPaths = AssetDatabase.GetAllAssetPaths();
        foreach (string path in assetPaths){
            if(CheckForPlaceableObject(path)){
                //assetFileList.Add(path);
                assetList.Add(AssetDatabase.LoadAssetAtPath<PlaceableObject>(path));
            }
        }
        //Now the  object list should only be filled with the PlaceableObjects in the Asset folder, including subdirs.
        //For every game object in the scene
        LevelData levelData = new LevelData();
        levelData.PlayerPos = Vector3.zero;
        levelData.Skybox = AssetDatabase.GetAssetPath(RenderSettings.skybox);
        levelData.song = 0;
        levelData.levelObjects = new List<LevelObject>();
        for(int i=0;i<allGameObjectArr.Length;i++){
            //If the gameobject is based on a prefab...
            if(PrefabUtility.IsPartOfPrefabInstance(allGameObjectArr[i])){
                //nab the prefab from the gameobject
                GameObject prefab = PrefabUtility.GetCorrespondingObjectFromSource(allGameObjectArr[i]) as GameObject;
                //find the corresponding asset in the list of PlaceableObjects...
                PlaceableObject matchingAsset = assetList.Find(asset => asset.model == prefab);
                //If there is one...
                if(matchingAsset != null){
                    //Add it to the list.
                    LevelObject obj = new LevelObject();
                    obj.name = matchingAsset.name;
                    obj.ID = matchingAsset.ID;
                    obj.position = allGameObjectArr[i].transform.position;
                    obj.rotation = allGameObjectArr[i].transform.rotation;
                    obj.scale = allGameObjectArr[i].transform.localScale;
                    obj.PrefabPath = AssetDatabase.GetAssetPath(matchingAsset.model);
                    obj.tag = allGameObjectArr[i].tag;
                    levelData.levelObjects.Add(obj);
                }
            }
        }
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        if (MMObjects != null){
            var path = EditorUtility.SaveFilePanel("Save Buttery Level Data",
                "","LevelName" +".butters","butters");
            if (path.Length != 0){
                string json = JsonUtility.ToJson(levelData);
                StreamWriter writer = new StreamWriter(path);
                writer.WriteLine(json);
                writer.Close();
            }
        }
    }


        [MenuItem("FatButters Tools/Level Tools/Compile Level Select Scene")]
    private static void BuildLSTextFile(){
        //set up my data.
        //Objects within the scene
        GameObject[] allGameObjectArr = GameObject.FindObjectsOfType<GameObject>();
        //objects within my asset structure.
        List<PlaceableObject> assetList = new List<PlaceableObject>();
        List<PlaceableObject> LSObjects = new List<PlaceableObject>();
        string[] assetPaths = AssetDatabase.GetAllAssetPaths();
        foreach (string path in assetPaths){
            if(CheckForPlaceableObject(path)){
                assetList.Add(AssetDatabase.LoadAssetAtPath<PlaceableObject>(path));
            } else {
                //EditorUtility.DisplayDialog("Error", "No PlaceableObject for " + path + ".", "OK");
            }
        }
        //Now the  object list should only be filled with the PlaceableObjects in the Asset folder, including subdirs.
        //For every game object in the scene
        LevelData levelData = new LevelData();
        levelData.PlayerPos = Vector3.zero;
        levelData.Skybox = null;
        levelData.song = 0;
        levelData.levelObjects = new List<LevelObject>();
        for(int i=0;i<allGameObjectArr.Length;i++){
            //If the gameobject is based on a prefab...
            if(PrefabUtility.IsPartOfPrefabInstance(allGameObjectArr[i])){
                //nab the prefab from the gameobject
                GameObject prefab = PrefabUtility.GetCorrespondingObjectFromSource(allGameObjectArr[i]) as GameObject;
                //find the corresponding asset in the list of PlaceableObjects...
                PlaceableObject matchingAsset = assetList.Find(asset => asset.model == prefab);
                //If there is one...
                if(matchingAsset != null){
                    //Add it to the list.
                    LevelObject obj = new LevelObject();
                    obj.name = matchingAsset.name;
                    obj.ID = matchingAsset.ID;
                    obj.position = allGameObjectArr[i].transform.position;
                    obj.rotation = allGameObjectArr[i].transform.rotation;
                    obj.scale = allGameObjectArr[i].transform.localScale;
                    obj.PrefabPath = AssetDatabase.GetAssetPath(matchingAsset.model);
                    obj.ExtraData = AcquireExtraData(allGameObjectArr[i]);
                    obj.tag = allGameObjectArr[i].tag;
                    levelData.levelObjects.Add(obj);
                }
            }
        }
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        if (LSObjects != null){
            var path = EditorUtility.SaveFilePanel("Save Buttery Level Data",
                "","LevelSelect" +".butters","butters");
            if (path.Length != 0){
                string json = JsonUtility.ToJson(levelData);
                StreamWriter writer = new StreamWriter(path);
                writer.WriteLine(json);
                writer.Close();
            }
        }
    }
    static bool CheckForPlaceableObject(string path){
        if(Path.GetExtension(path).Equals(".asset", System.StringComparison.OrdinalIgnoreCase)){
            // Load the asset
            UnityEngine.Object asset = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path);
            // Check if the asset is of type "placeableObject"
            if (asset != null && asset is PlaceableObject){
                return true;
            }
            return false;
        }
        return false;
    }



    [MenuItem("FatButters Tools/Create all new PlaceableObjects")]
    private static void CreatePlaceableObjects(){
        string prefabFolderPath = "Assets/Objects/PuzzlePieces"; // Adjust the path as needed

        string[] prefabGUIDs = AssetDatabase.FindAssets("t:Prefab", new[] { prefabFolderPath });

        foreach (var prefabGUID in prefabGUIDs){
            string prefabPath = AssetDatabase.GUIDToAssetPath(prefabGUID);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);

            if (prefab != null){
                CreatePlaceableObject(prefab, prefabPath);
            }
        }

        AssetDatabase.Refresh();
    }

    private static void CreatePlaceableObject(GameObject prefab, string prefabPath){
        string prefabName = Path.GetFileNameWithoutExtension(prefab.name);
        string placeableObjectID = prefabName;

        // Check if a PlaceableObject with the same ID already exists
        string existingPlaceableObjectPath = Path.Combine(Path.GetDirectoryName(prefabPath), prefabName + ".asset");
        PlaceableObject existingPlaceableObject = AssetDatabase.LoadAssetAtPath<PlaceableObject>(existingPlaceableObjectPath);

        if (existingPlaceableObject != null){
            return; // Skip creating a new PlaceableObject
        }
        Debug.Log("Placing PlaceableObject for: " + prefabName);
        PlaceableObject placeableObject = ScriptableObject.CreateInstance<PlaceableObject>();
        placeableObject.name = ""; // Leave the name property blank
        placeableObject.ID = placeableObjectID;
        placeableObject.position = Vector3.zero;
        placeableObject.rotation = Quaternion.identity;
        placeableObject.model = prefab;

        string savePath = Path.Combine(Path.GetDirectoryName(prefabPath), prefabName + ".asset");
        AssetDatabase.CreateAsset(placeableObject, savePath);
    }
}
*/

