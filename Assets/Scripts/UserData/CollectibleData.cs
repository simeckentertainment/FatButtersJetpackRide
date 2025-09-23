using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="newCollectibleData", menuName ="Data/Collectible Data")] //
public class CollectibleData : ScriptableObject
{
    public int BONES; 
    [Header("Store purchases")]
    public int fuelUpgradeLevel;
    public int thrustUpgradeLevel;
    public int treatsUpgradeLevel;
    public bool HASBALL; //ball is 10 seconds of non-explodey time.
    public bool killAds;
    public bool[] LevelBeaten;
    public int CurrentSkin;
    public bool[] HaveSkins;
    public float MasterVolumeLevel;
    public float MusicVolumeLevel;
    public float SFXVolumeLevel;
    public bool HapticsEnabled;
    public bool OnScreenControlsEnabled;
    public int GraphicsQualityLevel;
    
    [Header("Dev Options")]
    public bool GameplayTestingMode;
    public bool ignoreSaveData;
}
