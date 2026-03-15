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
    [MenuItem("FatButters Tools/Copy Editor CollectibleData to local Build Save Data")]
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
}
