using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using UnityEngine;
using UnityEngine.Animations;

public class LevelSelectLightManager : MonoBehaviour
{
    [System.NonSerialized] SaveManager saveManager;
    [SerializeField] Light sun;
    [SerializeField] Camera cam;
    [SerializeField] GameObject camFollower;
    [Header("Positional trackers")]
    public bool Episode1;
    public bool Episode2;
    public bool Episode3;
    public bool Episode4;
    public bool Episode5;

    [Header("Episode 1 stuff")]
    [SerializeField] public Material e1Skybox;
    [SerializeField] GameObject e1Lights;
    [SerializeField] int firstE1Level;
    [Header("Episode 2 stuff")]
    [SerializeField] public Material e2Skybox;
    [SerializeField] int firstE2Level;
    [Header("Episode 3 stuff")]
    [SerializeField] public Material e3Skybox;
    [SerializeField] GameObject e3Lights;
    [SerializeField] int firstE3Level;
    [Header("Episode 4 stuff")]
    [SerializeField] public Material e4Skybox;
    [SerializeField] GameObject e4Lights;
    [SerializeField] int firstE4Level;
    [Header("Episode 5 stuff")]
    [SerializeField] public Material e5Skybox;
    [SerializeField] int firstE5Level;
    [Header("Locked Episode Stuff")]
    [SerializeField] public Material LockedEpisodeSkybox;



    // Start is called before the first frame update
    void Start()
    {
        if(saveManager == null){
            saveManager = Helper.NabSaveData().GetComponent<SaveManager>();
        }
        cam = FindAnyObjectByType<Camera>();
    }
    // Update is called once per frame
    void Update()
    {
        camFollower.transform.position = cam.transform.position;
        if(Episode1){
            e1Lights.SetActive(true);
            SetLightsAndEnv(e1Skybox);
        } else {
            e1Lights.SetActive(false);
        }
        if(Episode2){
            if(saveManager.collectibleData.LevelBeaten[firstE2Level]){
                SetLightsAndEnv(e2Skybox);
            } else {
                SetLightsAndEnv(LockedEpisodeSkybox);
            }
        }
        if(Episode3){
            if(saveManager.collectibleData.LevelBeaten[firstE3Level]){
                e3Lights.SetActive(true);
                SetLightsAndEnv(e3Skybox);
            } else {
                e3Lights.SetActive(false);
                SetLightsAndEnv(LockedEpisodeSkybox);
            }
        } else {
            e3Lights.SetActive(false);
        }
        if(Episode4){
            if(saveManager.collectibleData.LevelBeaten[firstE4Level]){
                e4Lights.SetActive(true);
                SetLightsAndEnv(e4Skybox);
            } else {
                e4Lights.SetActive(false);
                SetLightsAndEnv(LockedEpisodeSkybox);
            }
        } else {
            e4Lights.SetActive(false);
        }
        if(Episode5){
            if(saveManager.collectibleData.LevelBeaten[firstE5Level]){
                SetLightsAndEnv(e5Skybox);
            } else {
                SetLightsAndEnv(LockedEpisodeSkybox);
            }
        }
    }

    void SetLightsAndEnv(Material skybox){
        RenderSettings.skybox = skybox;
        DynamicGI.UpdateEnvironment();
    }
}
