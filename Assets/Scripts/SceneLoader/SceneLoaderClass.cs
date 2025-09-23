using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class SceneLoaderClass : MonoBehaviour
{

    [System.NonSerialized] SaveManager saveManager;
    [SerializeField] private TMP_Text percentText;
    [SerializeField] Slider progressBone;
    private float currentValue;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnEnable()
    {
        if(saveManager == null){
            saveManager = Helper.NabSaveData().GetComponent<SaveManager>();
        }
        saveManager.Load();
        StartCoroutine(LoadLevelContainer());
    }

    // Update is called once per frame
    void Update()
    {
        percentText.text = System.Math.Truncate((currentValue)*100.0f).ToString() + "%";
    }

    IEnumerator LoadLevelContainer(){
        yield return LoadLevel();
        yield return null;
    }
    IEnumerator LoadLevel(){
    AsyncOperation operation = SceneManager.LoadSceneAsync(saveManager.sceneLoadData.SceneToLoad);
    operation.allowSceneActivation = false;
        while (!operation.isDone){
            currentValue = operation.progress/.9f;
            progressBone.value = currentValue;
            percentText.text = System.Math.Truncate((operation.progress)*100.0f).ToString() + "%";
            if(operation.progress >= 0.9f) {
                currentValue = 1f;
                operation.allowSceneActivation = true;
            }
            yield return null;
        }
    }
}
