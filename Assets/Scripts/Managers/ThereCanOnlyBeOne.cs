using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// Object.DontDestroyOnLoad example.
//
// This script example manages the playing audio. The GameObject with the
// "music" tag is the BackgroundMusic GameObject. The AudioSource has the
// audio attached to the AudioClip.

public class ThereCanOnlyBeOne : MonoBehaviour
{
    [Header("Current objects that stick around are:\nSaveManager,\nAd Managers")]
    public bool THEOG;
    public PermanentObjectType whatAmI;
    void OnEnable()
    {
        switch(whatAmI){
            case PermanentObjectType.SaveManager:
                SaveManager[] smObjs = FindObjectsByType<SaveManager>(FindObjectsSortMode.None);
                THEOG = AmIAlone(smObjs);
                break;
            default:
                break;
        }

        if(THEOG){
            gameObject.transform.parent = null; //Fixing a gameplay-only bug
            DontDestroyOnLoad(this.gameObject);
        } else {
            Destroy(this.gameObject);
        }
    }

bool AmIAlone(SaveManager[] sMs){
    return sMs.Length==1 ? true : false;
}
void AmIAlone(){
    Debug.Log("Probabilistically? No.");
}



    void Start(){

    }

    public enum PermanentObjectType {SaveManager, AdManager};
}