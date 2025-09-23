/*using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class LevelSelectAddressablesRealizer : MonoBehaviour
{

    [SerializeField] string path;
    List<GameObject> levelObjects;
    private AsyncOperationHandle<GameObject> handle;
    LevelData levelData;
    [SerializeField] SaveManager saveManager;
    void Awake(){
        saveManager.Load(); //gotta load the player data :)
        levelObjects = new List<GameObject>();
        LoadData();
    }
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        
    }

    void LoadData(){
        string[] lines = System.IO.File.ReadAllLines(path);




        string data = lines[0]; 
        levelData = JsonUtility.FromJson<LevelData>(data);
            foreach(LevelObject obj in levelData.levelObjects){
            handle = Addressables.LoadAssetAsync<GameObject>(obj.PrefabPath);
            handle.Completed += (operation) => Handle_Completed(obj, operation);
        }
    }
    private void Handle_Completed(LevelObject obj, AsyncOperationHandle<GameObject> operation){
        if (operation.Status == AsyncOperationStatus.Succeeded){
            //set all stuff before we instantiate...
            operation.Result.tag = obj.tag;
            operation.Result.transform.position = obj.position;
            operation.Result.transform.rotation = obj.rotation;
            operation.Result.transform.localScale = obj.scale;
            if(obj.ExtraData != ""){
                operation.Result.GetComponent<InstanceDataParser>().ExtraDataString = obj.ExtraData;
            }
            GameObject intantiatedObj = Instantiate(operation.Result,obj.position,obj.rotation);
            intantiatedObj.transform.localScale = obj.scale;
            levelObjects.Add(intantiatedObj);

        }else{
            Debug.LogError($"Asset data failed to load.");
        }
    }
    public void ClearPlayingField(){
        foreach(GameObject obj in levelObjects){
            Destroy(obj);
        }
    }
}
*/