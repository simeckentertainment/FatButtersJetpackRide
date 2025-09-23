using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="UserInfo", menuName ="Data/User Info")]
public class UserInfo : ScriptableObject
{ //This object gets re-written to every time the game starts.
    public bool AgeGateQuestionAnswered;
    public bool analyticsConsentAnswered;
    public bool isOldEnoughForAds;
    public bool dataCollectionConsent;
    public int monthBorn;
    public int dayBorn;
    public int yearBorn;
    public bool HapticsEnabled;
    public int LastMoTDVersion;
    public bool LastMoTDRead;
    public bool LevelSelectBanners;
    public bool PauseMenuBanners;
    public bool InterstitialToggle;
    public int InterstitialFrequency;
    public bool BoneDoublerToggle;
}
