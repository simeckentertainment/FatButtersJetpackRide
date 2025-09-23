using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

[Serializable]
public class SaveData{
    public int bones;
    public int fuelUpgrade;
    public int thrustUpgrade;
    public int profitUpgrade;
    public int treatsUpgrade;
    public bool StartWithBall;
    public bool killAds;
    public bool[] LevelBeaten;
    public int currentSkin;
    public bool[] haveSkins;
    public float MasterVolumeLevel;
    public float MusicVolumeLevel;
    public float SFXVolumeLevel;
    public bool ageGateQuestionAnswered;
    public bool analyticsConsentAnswered;
    public bool dataCollectionConsent;
    public bool isOldEnoughForAds;
    public int monthBorn;
    public int dayBorn;
    public int yearBorn;
    public bool hapticsEnabled;
    public int LastMotdVersion;
    public bool LastMotdRead;
    public bool OnScreenControlsEnabled;
    public string SceneToLoad;
    public string LastLoadedLevel;
    public int LastLoadedLevelInt;
    public int AdHistoryCounter;
    public bool LevelSelectBanners;
    public bool PauseMenuBanners;
    public bool InterstitialToggle;
    public int InterstitialFrequency;
    public bool BoneDoublerToggle;
    public int GraphicsQualityLevel;
}
