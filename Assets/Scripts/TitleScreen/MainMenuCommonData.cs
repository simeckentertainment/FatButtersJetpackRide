using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuCommonData : MonoBehaviour
{
    [Header("Common Main Menu Data")]
    [SerializeField] public MainMenuCameraMover mainMenuCameraMover;
    [System.NonSerialized] public SaveManager saveManager;
    //[SerializeField] public CollectibleData collectibleData;
    //[SerializeField] public SceneLoadData sceneLoadData;

    // Start is called before the first frame update
    void OnEnable()
    {
        saveManager = Helper.NabSaveData().GetComponent<SaveManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void BackButton(){
        mainMenuCameraMover.MoveTo(mainMenuCameraMover.MainMenu);
    }
}
