using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;



[System.Serializable]
public class OtherFatButtersTools
{
    [MenuItem("FatButters Tools/Save Current Collectible Data State to Disk")]
    static void SaveEditorData(){
        CollectibleData workingData = AssetDatabase.LoadAssetAtPath<CollectibleData>("Assets/Scripts/UserData/FatButtersData.asset");
        UserInfo workingInfo = AssetDatabase.LoadAssetAtPath<UserInfo>("Assets/Scripts/UserData/UserInfo.asset");
        //Begin save stuff
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream saveFile = File.Create(Application.persistentDataPath  + "/ButtersSaveData.dat");
        SaveData data = new SaveData();
            data.bones = workingData.BONES;
            data.fuelUpgrade = workingData.fuelUpgradeLevel;
            data.thrustUpgrade = workingData.thrustUpgradeLevel;
            data.treatsUpgrade = workingData.treatsUpgradeLevel;
            data.StartWithBall = workingData.HASBALL;
            data.killAds = workingData.killAds;
            data.LevelBeaten = workingData.LevelBeaten;
            data.currentSkin = workingData.CurrentSkin;
            data.haveSkins = workingData.HaveSkins;
            data.analyticsConsentAnswered = workingInfo.analyticsConsentAnswered;
            data.ageGateQuestionAnswered = workingInfo.AgeGateQuestionAnswered;
            data.isOldEnoughForAds = workingInfo.isOldEnoughForAds;
            data.MasterVolumeLevel = workingData.MasterVolumeLevel;
            data.MusicVolumeLevel = workingData.MusicVolumeLevel;
            data.SFXVolumeLevel = workingData.SFXVolumeLevel;
            data.hapticsEnabled = workingData.HapticsEnabled;
            data.OnScreenControlsEnabled = workingData.OnScreenControlsEnabled;
            data.monthBorn = workingInfo.monthBorn;
            data.dayBorn = workingInfo.dayBorn;
            data.yearBorn = workingInfo.yearBorn;
            data.LastMotdRead = workingInfo.LastMoTDRead;
            data.LastMotdVersion = workingInfo.LastMoTDVersion;
            data.LevelSelectBanners = workingInfo.LevelSelectBanners;
            data.PauseMenuBanners = workingInfo.PauseMenuBanners;
            data.InterstitialToggle = workingInfo.InterstitialToggle;
            data.InterstitialFrequency = workingInfo.InterstitialFrequency;
            data.BoneDoublerToggle = workingInfo.BoneDoublerToggle;
        binaryFormatter.Serialize(saveFile,data);
        saveFile.Close();
        Debug.Log("Data saved!");
    }
    [MenuItem("FatButters Tools/Set Endgame stats")]
    static void SetEndgameStats(){
        CollectibleData workingData = AssetDatabase.LoadAssetAtPath<CollectibleData>("Assets/Scripts/FatButtersData.asset");
        UserInfo workingInfo = AssetDatabase.LoadAssetAtPath<UserInfo>("Assets/Scripts/UserInfo.asset");
        //Begin save stuff
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream saveFile = File.Create(Application.persistentDataPath  + "/ButtersSaveData.dat");
        SaveData data = new SaveData();
        data.bones = 999;
        workingData.BONES = 999;
        data.fuelUpgrade = 50;
        workingData.fuelUpgradeLevel = 50;
        data.thrustUpgrade = 50;
        workingData.thrustUpgradeLevel = 50;
        data.treatsUpgrade = 50;
        workingData.treatsUpgradeLevel = 50;
        data.StartWithBall = false;
        workingData.HASBALL = false;
        data.killAds = true;
        workingData.killAds = true;
        for(int i=0;i<workingData.LevelBeaten.Length;i++){
            workingData.LevelBeaten[i] = true;
        }
        data.LevelBeaten = workingData.LevelBeaten;
        data.currentSkin = 0;
        workingData.CurrentSkin = 0;
        for(int i=0;i<workingData.HaveSkins.Length;i++){
            workingData.HaveSkins[i] = true;
        }
        data.haveSkins = workingData.HaveSkins;
        data.analyticsConsentAnswered = true;
        workingInfo.analyticsConsentAnswered = true;
        data.ageGateQuestionAnswered = true;
        data.isOldEnoughForAds = true;
        data.MasterVolumeLevel = 1f;
        workingData.MasterVolumeLevel = 1f;
        data.MusicVolumeLevel = 1f;
        workingData.MusicVolumeLevel = 1f;
        data.SFXVolumeLevel = 1f;
        workingData.SFXVolumeLevel = 1f;
        data.monthBorn = 3;
        data.dayBorn = 10;
        data.yearBorn = 1988;
        binaryFormatter.Serialize(saveFile,data);
        saveFile.Close();
        Debug.Log("Data saved!");
    }

    [MenuItem("FatButters Tools/Prep data for build")]
    static void PrepDataForBuild(){
        CollectibleData prebuildData = AssetDatabase.LoadAssetAtPath<CollectibleData>("Assets/Scripts/UserData/FatButtersData.asset");
        UserInfo prebuildInfo = AssetDatabase.LoadAssetAtPath<UserInfo>("Assets/Scripts/UserData/UserInfo.asset");
            prebuildData.BONES = 0;
            prebuildData.fuelUpgradeLevel = 1;
            prebuildData.thrustUpgradeLevel = 1;
            prebuildData.treatsUpgradeLevel = 1;
            prebuildData.HASBALL = false;
            prebuildData.killAds = false;

            for(int i=0; i<prebuildData.LevelBeaten.Length;i++){
                prebuildData.LevelBeaten[i] = false;
            }
            prebuildData.LevelBeaten[0] = true;
            for(int i=0; i<prebuildData.HaveSkins.Length;i++){
                prebuildData.HaveSkins[i] = false;
            }
            prebuildData.HaveSkins[0] = true;
            prebuildData.CurrentSkin = 0;
            prebuildInfo.analyticsConsentAnswered = false;
            prebuildInfo.dataCollectionConsent = false;
            prebuildInfo.AgeGateQuestionAnswered = false;
            prebuildInfo.isOldEnoughForAds = false;
            prebuildInfo.monthBorn = 0;
            prebuildInfo.dayBorn = 0;
            prebuildInfo.yearBorn = 0;
            prebuildData.MasterVolumeLevel = 1;
            prebuildData.MusicVolumeLevel = 1;
            prebuildData.SFXVolumeLevel = 1;
            prebuildData.HapticsEnabled = true;
            prebuildData.OnScreenControlsEnabled = false;
            prebuildData.GameplayTestingMode = false;
            prebuildData.ignoreSaveData = false;
            prebuildInfo.analyticsConsentAnswered = false;
            prebuildInfo.dataCollectionConsent = false;
            prebuildInfo.LevelSelectBanners = false;
            prebuildInfo.PauseMenuBanners = false;
            prebuildInfo.InterstitialToggle = false;
            prebuildInfo.InterstitialFrequency = 3;
            prebuildInfo.BoneDoublerToggle = true;
    }
}
