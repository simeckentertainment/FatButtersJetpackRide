using UnityEngine;

public class SettingsMenuModel : Model
{
    [SerializeField] public float VolumeMultiplier = 0.9f;

    private CollectibleData collectibleData => SaveManager.Instance.collectibleData;
    private UserInfo userInfo => SaveManager.Instance.userInfo;

    #region properties

    private SettingsPage _currentPage = SettingsPage.Base;
    public SettingsPage CurrentPage
    {
        get
        {
            return _currentPage;
        }
        set
        {
            _currentPage = value;
            Refresh();
        }
    }

    public float MasterVolume
    {
        get
        {
            return collectibleData.MasterVolumeLevel / VolumeMultiplier;
        }
        set
        {
            collectibleData.MasterVolumeLevel = value * VolumeMultiplier;
            FixVolumeSlidersFromMaster();

            Save();
        }
    }

    public float MusicVolume
    {
        get
        {
            return collectibleData.MusicVolumeLevel / VolumeMultiplier;
        }
        set
        {
            collectibleData.MusicVolumeLevel = value * VolumeMultiplier;
            FixVolumeSlidersFromMaster();

            Save();
        }
    }

    public float SfxVolume
    {
        get
        {
            return collectibleData.SFXVolumeLevel / VolumeMultiplier;
        }
        set
        {
            collectibleData.SFXVolumeLevel = value * VolumeMultiplier;
            FixVolumeSlidersFromMaster();

            Save();
        }
    }

    public int GraphicsQuality
    {
        get
        {
            return collectibleData.GraphicsQualityLevel;
        }
        set
        {

            QualitySettings.SetQualityLevel(value);
            collectibleData.GraphicsQualityLevel = value;
            Save();
        }
    }

    public bool HapticsEnabled
    {
        get
        {
            return collectibleData.HapticsEnabled;
        }
        set
        {
            collectibleData.HapticsEnabled = value;
            Save();
        }
    }

    public bool OnScreenControlsEnabled
    {
        get
        {
            return collectibleData.OnScreenControlsEnabled;
        }
        set
        {
            collectibleData.OnScreenControlsEnabled = value;

            Save();
        }
    }

    public bool CorgiSenseEnabled
    {
        get
        {
            return collectibleData.CorgiSenseEnabled;
        }
        set
        {
            collectibleData.CorgiSenseEnabled = value;
            Save();
        }
    }

    public bool LevelSelectAdsEnabled
    {
        get
        {
            return userInfo.LevelSelectBanners;
        }
        set
        {
            userInfo.LevelSelectBanners = value;
            Save();
        }
    }

    public bool PauseMenuAdsEnabled
    {
        get
        {
            return userInfo.PauseMenuBanners;
        }
        set
        {
            userInfo.PauseMenuBanners = value;
            Save();
        }
    }

    public bool PostLevelAdsEnabled
    {
        get
        {
            return userInfo.InterstitialToggle;
        }
        set
        {
            userInfo.InterstitialToggle = value;
            Save();
        }
    }

    public bool BoneDoublerVideoAdsEnabled
    {
        get
        {
            return userInfo.BoneDoublerToggle;
        }
        set
        {
            userInfo.BoneDoublerToggle = value;
            Save();
        }
    }

    #endregion

    public void ShowPrivacyPolicy()
    {
        Application.OpenURL("https://fatbutters.simeck.com/privacyPolicy.txt");
    }

    public void ToLevelSelect()
    {
        collectibleData.HASBALL = false;
        SaveManager.Instance.Save();

        PauseUtility.Resume();

        Levels.Load(Levels.LevelSelect);
    }

    private void Save()
    {
        SaveManager.Instance.Save();
        Refresh();
    }

    private void FixVolumeSlidersFromMaster()
    {
        // MasterVolumeLevel is a limiter for the other volume sliders, they should never exceed MasterVolumeLevel.

        if (collectibleData.SFXVolumeLevel > collectibleData.MasterVolumeLevel)
        {
            collectibleData.SFXVolumeLevel = collectibleData.MasterVolumeLevel;
        }

        if (collectibleData.MusicVolumeLevel > collectibleData.MasterVolumeLevel)
        {
            collectibleData.MusicVolumeLevel = collectibleData.MasterVolumeLevel;
        }
    }
}

public enum SettingsPage
{
    Base,
    Ads,
    ConfirmLeave
}
