using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectAssetRealizer : MonoBehaviour
{/*
    [SerializeField] string path;
    List<GameObject> levelObjects;
    [SerializeField] SaveManager saveManager;
    ButtersFileLibrary buttersFileLibrary;

    // Start is called before the first frame update
    void Awake()
    {
        buttersFileLibrary = FindAnyObjectByType<ButtersFileLibrary>();
        saveManager.Load(); //gotta load the player data :)
        levelObjects = new List<GameObject>();
        LoadData();
        
    }
    void LoadData(){
        LevelData levelData = buttersFileLibrary.GetLevelSelectLevelData();
        foreach(LevelObject obj in levelData.levelObjects){
            try{
            GameObject instantiatedObj = ActiveAssetBundles.ActiveBundles[obj.bundle].LoadAsset<GameObject>(obj.PrefabPath);
            Debug.Log(instantiatedObj.name);
            instantiatedObj.tag = obj.tag;
            instantiatedObj.transform.position = obj.position;
            instantiatedObj.transform.rotation = obj.rotation;
            instantiatedObj.transform.localScale = obj.scale;
            InstanceDataParser idp = instantiatedObj.GetComponent<InstanceDataParser>();
            if(idp != null){
                idp.ExtraDataString = obj.ExtraData;
            }
            levelObjects.Add(Instantiate(instantiatedObj));
            } catch {
                Debug.Log("Can't spawn " + obj.name);
            }
        }
        LevelSelectLightManager lm = FindObjectOfType<LevelSelectLightManager>();
        lm.e1Skybox = ActiveAssetBundles.ActiveBundles["skyboxes"].LoadAsset<Material>("Assets/Objects/PuzzlePieces/AssetModels/Skyboxes/Epic_GloriousPink.mat");
        lm.e2Skybox = ActiveAssetBundles.ActiveBundles["skyboxes"].LoadAsset<Material>("Assets/Objects/PuzzlePieces/AssetModels/Skyboxes/AllSky_Overcast4_Low.mat");
        lm.e3Skybox = ActiveAssetBundles.ActiveBundles["skyboxes"].LoadAsset<Material>("Assets/Objects/PuzzlePieces/AssetModels/Skyboxes/Cold Night.mat");
        lm.e4Skybox = ActiveAssetBundles.ActiveBundles["skyboxes"].LoadAsset<Material>("Assets/Objects/PuzzlePieces/AssetModels/Skyboxes/Deep Dusk.mat");
        lm.e5Skybox = ActiveAssetBundles.ActiveBundles["skyboxes"].LoadAsset<Material>("Assets/Objects/PuzzlePieces/AssetModels/Skyboxes/Epic_BlueSunset.mat");
        lm.LockedEpisodeSkybox = ActiveAssetBundles.ActiveBundles["skyboxes"].LoadAsset<Material>("Assets/Objects/PuzzlePieces/AssetModels/Skyboxes/Cold Night.mat");
    }
    public void ClearPlayingField(){
        foreach(GameObject obj in levelObjects){
            Destroy(obj);
        }
    }
    */
}
