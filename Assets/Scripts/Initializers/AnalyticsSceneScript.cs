using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using Unity.Services.Core;
public class AnalyticsSceneScript : InitializationCommonFunctions
{
    [SerializeField] GameObject[] AnalyticsConsentUiObjects;
    [SerializeField] GameObject Darkener;
    [SerializeField] GameObject logo;
    bool ReadyToGo;
    int ReadyToGoThreshold = 30;
    int readyToGoCounter;
    // Start is called before the first frame update

    async void OnEnable(){
        await UnityServices.InitializeAsync();
    }
    void Start()
    {
        readyToGoCounter = 0;
        ReadyToGo = false;
        if(saveManager == null){
            saveManager = Helper.NabSaveData().GetComponent<SaveManager>();
        }
        saveManager.EnsureSaveFileExists();
        saveManager.Load();
        PrettyPictureToggle(saveManager.userInfo.analyticsConsentAnswered);
    }

    // Update is called once per frame
    void Update()
    {
        if(saveManager.userInfo.analyticsConsentAnswered){
           RunReadyToGo();
        }
    }

    public void AcceptDataCollection(){
        saveManager.userInfo.analyticsConsentAnswered = true;
        saveManager.userInfo.dataCollectionConsent = true;
        Debug.Log("Accept!");
        PrettyPictureToggle(true);
        saveManager.Save();
    }
    public void DenyDataCollection(){
        saveManager.userInfo.analyticsConsentAnswered = true;
        saveManager.userInfo.dataCollectionConsent = false;
        Debug.Log("Decline!");
        PrettyPictureToggle(true);
        saveManager.Save();
    }

    void RunReadyToGo(){
        readyToGoCounter++;
        if(readyToGoCounter >= ReadyToGoThreshold){
            Levels.Load(Levels.TitleScreen);
        }
    }

    void PrettyPictureToggle(bool whatDo){
        //if it's already done...
        if(whatDo){
            logo.SetActive(true);
            Darkener.SetActive(false);
            foreach(GameObject obj in AnalyticsConsentUiObjects){
                obj.SetActive(false);
            }
        } else {
            //if it's not done...
            logo.SetActive(false);
            Darkener.SetActive(true);
            foreach(GameObject obj in AnalyticsConsentUiObjects){
                obj.SetActive(true);
            } 
        }
    }


}
