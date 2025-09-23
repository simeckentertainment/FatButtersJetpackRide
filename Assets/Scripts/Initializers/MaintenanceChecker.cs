using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using TMPro;

public class MaintenanceChecker : InitializationCommonFunctions
{
    [SerializeField] bool bypassMaintenance;
    public bool connectSuccess = false;
    public Dictionary<string,string> maintenanceData;
    [SerializeField] GameObject darkener;
    [SerializeField] public TMP_Text MaintenanceText;

    // Start is called before the first frame update
    void Start()
    {
        if(bypassMaintenance){
            GoToNextStep();
        } else {
            maintenanceData = new Dictionary<string, string>();
            StartCoroutine("MaintenanceCheckerTopLayer");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator MaintenanceCheckerTopLayer(){
        yield return StartCoroutine("GetMaintenanceData");
        yield return StartCoroutine("RespondToMaintenanceMode");
    }

    IEnumerator GetMaintenanceData(){
        var cert = new ForceAcceptAll();
        UnityWebRequest MaintenanceFile = new UnityWebRequest(remoteDir + "maintenanceData.txt");
        MaintenanceFile.certificateHandler = cert;
        MaintenanceFile.downloadHandler = new DownloadHandlerBuffer();
        yield return MaintenanceFile.SendWebRequest();
        while (!MaintenanceFile.isDone){} //just wait it out.
        cert?.Dispose();
        if(MaintenanceFile.result != UnityWebRequest.Result.Success){
            connectSuccess = false;
        } else {
            connectSuccess = true;
            string[] textLines = MaintenanceFile.downloadHandler.text.Split("\n");
            foreach (string line in textLines){
                string[] splitLine = line.Split(":");
                maintenanceData.Add(splitLine[0],splitLine[1]);
            }
            yield return maintenanceData;
        }
        yield return connectSuccess;

    }

    IEnumerator RespondToMaintenanceMode(){
        if(!connectSuccess){
            darkener.SetActive(true);
            MaintenanceText.gameObject.SetActive(true);
            if (!File.Exists(Application.persistentDataPath + "/AssetBundles/FatButtersAssetBundleVersions.txt")){
                yield return MaintenanceText.text = "You must connect to a network so I can acquire the corgis!";
            } else {
                yield return MaintenanceText.text = "Unable to check for updates. Defaulting\n to local files.\nYou will not have access to achievements\nEnjoy the game!";
                Invoke("SkipRestOfTheProcess",3.0f);
            }
        yield return null;
        } else {
            if(maintenanceData["maintenanceMode"].Contains("yes")){
                darkener.SetActive(true);
                MaintenanceText.gameObject.SetActive(true);
                yield return MaintenanceText.text = "The game is currently in maintenance mode. We are scheduled to return " + maintenanceData["maintenanceModeCompleteTime"]; 
            } else {
                yield return MaintenanceText.text = "Initializing Asset pack updates!";
                Invoke("GoToNextStep",3.0f);
            }
        }
        yield return null;
    }
    void SkipRestOfTheProcess(){
        SceneManager.LoadScene("Scenes/StatsAndLoaderScenes/6-AssetLoader");
    }
    void GoToNextStep(){
        SceneManager.LoadScene("Scenes/StatsAndLoaderScenes/4-AssetDownloader");
    }
    void SkipDownload()
    {
        SceneManager.LoadScene("Scenes/StatsAndLoaderScenes/5-SocialNetworkInitialization");
    }
}
