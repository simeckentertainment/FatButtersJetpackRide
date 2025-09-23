using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Networking;
using System;
/*
#if UNITY_ANDROID
using GooglePlayGames;
using GooglePlayGames.BasicApi;
#endif
*/
public class MainMenuButtonManager : MainMenuCommonData
{

    [SerializeField] GameObject[] environments;
    [Header("Stuff for the credits menu")]
    [SerializeField] creditsScroller CreditsScroller;
    [Header("Stuff for MoTD")]
    [SerializeField] GameObject MoTDCanvas;
    [SerializeField] TMP_Text updateText;
    [SerializeField] UserInfo userInfo;

    // Start is called before the first frame update
    void Start()
    {
        //environments[UnityEngine.Random.Range(0, environments.Length)].SetActive(true);
        if(saveManager == null){
            saveManager = Helper.NabSaveData().GetComponent<SaveManager>();
        }
        saveManager.EnsureSaveFileExists();
        saveManager.Load();
        StartCoroutine(CoroutineHome());

 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void CreditsClick(){
        CreditsScroller.Reset();
        mainMenuCameraMover.MoveTo(mainMenuCameraMover.CreditsMenu);
    }

    public IEnumerator CoroutineHome()
    {
        yield return StartCoroutine(FetchMOTD());
    }
    public IEnumerator FetchMOTD()
    {
        string remoteDir = "https://fatbutters.simeck.com/AssetBundles/";
        UnityWebRequest motdFile = new UnityWebRequest(remoteDir + "motd.txt");
        var cert = new ForceAcceptAll();
        motdFile.certificateHandler = cert;
        motdFile.downloadHandler = new DownloadHandlerBuffer();
        yield return motdFile.SendWebRequest();
        cert?.Dispose();
        while (!motdFile.isDone)
        {
            updateText.text = "Checking Message of the Day";
        }
        if (motdFile.result != UnityWebRequest.Result.Success)
        {
            HideMOTD();
            yield break;
        }
        string fullRemoteFileText = motdFile.downloadHandler.text;
        string[] dataArray = fullRemoteFileText.Split("\n");
        Dictionary<string, string> output = new Dictionary<string, string>();
        foreach(string data in dataArray)
        {
            try{
            string[] splitLine = data.Split(":");
            output.Add(splitLine[0], splitLine[1]);
            } catch { }
        }
        if(userInfo.LastMoTDVersion == Int32.Parse(output["version"]) & userInfo.LastMoTDRead){
            HideMOTD();
        } else{
            userInfo.LastMoTDVersion = Int32.Parse(output["version"]);
            MoTDCanvas.SetActive(true);
            string[] messageTextFull = output["message"].Split("--");
            string tempString = "";
            foreach(string segment in messageTextFull)
            {
                tempString += "\n" + segment;
            }
            yield return updateText.text = tempString;
        }
        yield return null;
    }
    public void HideMOTD()
    {
        MoTDCanvas.SetActive(false);
    }
    public void HideMOTDButtonMethod(){
        userInfo.LastMoTDRead = true;
        MoTDCanvas.SetActive(false);
        saveManager.Save();
    }

    public void GoToLevelSelectScene(){
        Levels.Load(Levels.LevelSelect);
    }
}
