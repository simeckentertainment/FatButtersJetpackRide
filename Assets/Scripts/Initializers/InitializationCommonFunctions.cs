using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
public class InitializationCommonFunctions : MonoBehaviour
{

    [System.NonSerialized] public string remoteDir = "https://fatbutters.simeck.com/AssetBundles/";
    //public string AssetBundleDir = "/AssetBundles/";
    [System.NonSerialized] public SaveManager saveManager;
    public bool updateComplete = false;
    void OnEnable(){
        saveManager = Helper.NabSaveData().GetComponent<SaveManager>();
    }
}

public class ForceAcceptAll : CertificateHandler{
    protected override bool ValidateCertificate(byte[] certificateData){
        return true;
    }
}

