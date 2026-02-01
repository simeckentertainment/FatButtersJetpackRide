using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveManager : Singleton<SaveManager>
{
    [SerializeField] public CollectibleData collectibleData;
    [SerializeField] public UserInfo userInfo;
    [SerializeField] public SceneLoadData sceneLoadData;

    private string saveFilename = "/ButtersSaveData.dat";

    protected override void Awake()
    {
        base.Awake();

        if (!markedToDestroy && !collectibleData.ignoreSaveData)
        {
            EnsureSaveFileExists();
        }
    }

    public void EnsureSaveFileExists()
    {
        if (File.Exists(Application.persistentDataPath + saveFilename))
        {
            Load();
        }
        else
        {
            CreateNewSave();
        }
    }

    public void CreateNewSave()
    {
        if(File.Exists(Application.persistentDataPath + saveFilename))
        {
            File.Delete(Application.persistentDataPath + saveFilename);
        }
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream saveFile = File.Create(Application.persistentDataPath + saveFilename);
        SaveData data = new SaveData();
        data.bones = 0;
        data.fuelUpgrade = 1;
        data.thrustUpgrade = 1;
        data.treatsUpgrade = 1;
        data.StartWithBall = false;
        data.LevelBeaten = new bool[21];
        data.LevelBeaten[0] = true;
        data.currentSkin = 0;
        data.analyticsConsentAnswered = false;
        data.ageGateQuestionAnswered = false;
        data.dataCollectionConsent = false;
        data.isOldEnoughForAds = false;
        data.MasterVolumeLevel = 1.0f;
        data.MusicVolumeLevel = 1.0f;
        data.SFXVolumeLevel = 1.0f;
        data.yearBorn = 0;
        data.monthBorn = 0;
        data.dayBorn = 0;
        data.hapticsEnabled = true;
        data.SceneToLoad = "";
        data.LastLoadedLevel = "";
        data.OnScreenControlsEnabled = false;
        data.CorgiSenseEnabled = true;
        data.SceneToLoad = "";
        data.LastLoadedLevel = "";
        data.LastLoadedLevelInt = 0;
        data.AdHistoryCounter = 0;
        data.LevelSelectBanners = false;
        data.PauseMenuBanners = true;
        data.InterstitialToggle = false;
        data.InterstitialFrequency = 3;
        data.BoneDoublerToggle = true;
        data.GraphicsQualityLevel = 1;

        //These are permanent IAP purchases and should be treated with care.
        data.killAds = false;
        data.haveSkins = new bool[collectibleData.HaveSkins.Length];
        data.haveSkins[0] = true;

        binaryFormatter.Serialize(saveFile,data);
        saveFile.Close();
        //Debug.Log("new save file created.");
        Load();
    }

    public void Save()
    {
        if(!collectibleData.ignoreSaveData)
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            if(File.Exists(Application.persistentDataPath + saveFilename))
            {
                File.Delete(Application.persistentDataPath + saveFilename);
            }
            FileStream saveFile = File.Create(Application.persistentDataPath + saveFilename);
            SaveData data = new SaveData();
            data.bones = collectibleData.BONES;
            data.fuelUpgrade = collectibleData.fuelUpgradeLevel;
            data.thrustUpgrade = collectibleData.thrustUpgradeLevel;
            data.treatsUpgrade = collectibleData.treatsUpgradeLevel;
            data.StartWithBall = collectibleData.HASBALL;
            data.killAds = collectibleData.killAds;
            data.LevelBeaten = collectibleData.LevelBeaten;
            data.currentSkin = collectibleData.CurrentSkin;
            data.haveSkins = collectibleData.HaveSkins;
            data.ageGateQuestionAnswered = userInfo.AgeGateQuestionAnswered;
            data.analyticsConsentAnswered = userInfo.analyticsConsentAnswered;
            data.dataCollectionConsent = userInfo.dataCollectionConsent;
            data.isOldEnoughForAds = userInfo.isOldEnoughForAds;
            data.MasterVolumeLevel = collectibleData.MasterVolumeLevel;
            data.MusicVolumeLevel = collectibleData.MusicVolumeLevel;
            data.SFXVolumeLevel = collectibleData.SFXVolumeLevel;
            data.hapticsEnabled = collectibleData.HapticsEnabled;
            data.OnScreenControlsEnabled = collectibleData.OnScreenControlsEnabled;
            data.CorgiSenseEnabled = collectibleData.CorgiSenseEnabled;
            data.monthBorn = userInfo.monthBorn;
            data.dayBorn = userInfo.dayBorn;
            data.yearBorn = userInfo.yearBorn;
            data.SceneToLoad = sceneLoadData.SceneToLoad;
            data.LastLoadedLevel = sceneLoadData.LastLoadedLevel;
            data.LastLoadedLevelInt = sceneLoadData.LastLoadedLevelInt;
            data.AdHistoryCounter = sceneLoadData.adHistoryCounter;
            data.LastMotdRead = userInfo.LastMoTDRead;
            data.LastMotdVersion = userInfo.LastMoTDVersion;
            data.LevelSelectBanners = userInfo.LevelSelectBanners;
            data.PauseMenuBanners = userInfo.PauseMenuBanners;
            data.InterstitialToggle = userInfo.InterstitialToggle;
            data.InterstitialFrequency = userInfo.InterstitialFrequency;
            data.BoneDoublerToggle = userInfo.BoneDoublerToggle;
            data.GraphicsQualityLevel = collectibleData.GraphicsQualityLevel;

            binaryFormatter.Serialize(saveFile,data);
            saveFile.Close();
        }
    }

    public void Load()
    {
        if(!collectibleData.ignoreSaveData)
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + saveFilename, FileMode.Open, FileAccess.Read);
            SaveData data = (SaveData)bf.Deserialize(file);
            file.Close();
            collectibleData.BONES = data.bones;
            collectibleData.fuelUpgradeLevel = data.fuelUpgrade;
            collectibleData.thrustUpgradeLevel = data.thrustUpgrade;
            collectibleData.treatsUpgradeLevel = data.treatsUpgrade;
            collectibleData.HASBALL = data.StartWithBall;
            collectibleData.killAds = data.killAds;
            collectibleData.LevelBeaten = data.LevelBeaten;
            collectibleData.CurrentSkin = data.currentSkin;
            collectibleData.HaveSkins = data.haveSkins;
            userInfo.analyticsConsentAnswered = data.analyticsConsentAnswered;
            userInfo.AgeGateQuestionAnswered = data.ageGateQuestionAnswered;
            userInfo.dataCollectionConsent = data.dataCollectionConsent;
            userInfo.isOldEnoughForAds = data.isOldEnoughForAds;
            userInfo.monthBorn = data.monthBorn;
            userInfo.dayBorn = data.dayBorn;
            userInfo.yearBorn = data.yearBorn;
            userInfo.LastMoTDRead = data.LastMotdRead;
            userInfo.LastMoTDVersion = data.LastMotdVersion;
            collectibleData.MasterVolumeLevel = data.MasterVolumeLevel;
            collectibleData.MusicVolumeLevel = data.MusicVolumeLevel;
            collectibleData.SFXVolumeLevel = data.SFXVolumeLevel;
            collectibleData.HapticsEnabled = data.hapticsEnabled;
            collectibleData.OnScreenControlsEnabled = data.OnScreenControlsEnabled;
            collectibleData.CorgiSenseEnabled = data.CorgiSenseEnabled;
            sceneLoadData.SceneToLoad = data.SceneToLoad;
            sceneLoadData.LastLoadedLevel = data.LastLoadedLevel;
            sceneLoadData.LastLoadedLevelInt = data.LastLoadedLevelInt;
            sceneLoadData.adHistoryCounter = data.AdHistoryCounter;
            userInfo.LevelSelectBanners = data.LevelSelectBanners;
            userInfo.PauseMenuBanners = data.PauseMenuBanners;
            userInfo.InterstitialToggle = data.InterstitialToggle;
            userInfo.InterstitialFrequency = data.InterstitialFrequency;
            userInfo.BoneDoublerToggle = data.BoneDoublerToggle;
            collectibleData.GraphicsQualityLevel = data.GraphicsQualityLevel;
        }
    }

    public void ResetSave()
    {
        if(!collectibleData.ignoreSaveData)
        {
            File.Delete(Application.persistentDataPath + saveFilename);
            CreateNewSave();
        }
    }

    public void DeleteSave()
    {
        if(!collectibleData.ignoreSaveData)
        {
            File.Delete(Application.persistentDataPath + saveFilename);
        }
    }

    public void CreateCompletedSave()
    {
        if(File.Exists(Application.persistentDataPath + saveFilename))
        {
            File.Delete(Application.persistentDataPath + saveFilename);
        }
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        if(File.Exists(Application.persistentDataPath + saveFilename))
        {
            File.Delete(Application.persistentDataPath + saveFilename);
        }
        FileStream saveFile = File.Create(Application.persistentDataPath + saveFilename);
        SaveData data = new SaveData();
        data.bones = 999;
        data.fuelUpgrade = 999;
        data.thrustUpgrade = 50;
        data.treatsUpgrade = 999;
        data.StartWithBall = false;
        data.killAds = true;
        data.currentSkin = 0;
        data.haveSkins = new bool[collectibleData.HaveSkins.Length];
        for (int i = 0; i < data.haveSkins.Length; i++)
        {
            data.haveSkins[i] = true;
        }
        data.analyticsConsentAnswered = true;
        data.ageGateQuestionAnswered = true;
        data.dataCollectionConsent = true;
        data.isOldEnoughForAds = false;
        data.MasterVolumeLevel = 1.0f;
        data.MusicVolumeLevel = 1.0f;
        data.SFXVolumeLevel = 1.0f;
        data.yearBorn = 0;
        data.monthBorn = 0;
        data.dayBorn = 0;
        data.hapticsEnabled = true;
        data.OnScreenControlsEnabled = false;
        data.CorgiSenseEnabled = true;
        data.LastMotdRead = true;
        data.LastMotdVersion = 0;
        data.SceneToLoad = "";
        data.LastLoadedLevel = "";
        data.LastLoadedLevelInt = 0;
        data.LevelSelectBanners = false;
        data.PauseMenuBanners = false;
        data.InterstitialToggle = false;
        data.InterstitialFrequency = 3;
        data.BoneDoublerToggle = false;
        data.GraphicsQualityLevel = 1;
        binaryFormatter.Serialize(saveFile,data);
        saveFile.Close();
        collectibleData.ignoreSaveData = false;
        Debug.Log("new Dev save file created.");
        Load();
    }
    
    //From here on in, we've got the Google Cloud save stuff.
/*


    void SyncPermanentIAPs(SaveData data){

    }
#if UNITY_ANDROID
        public void ShowSelectUI() {
        uint maxNumToDisplay = 5;
        bool allowCreateNew = false;
        bool allowDelete = true;
        
        ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
        savedGameClient.ShowSelectSavedGameUI("Select saved game",
            maxNumToDisplay,
            allowCreateNew,
            allowDelete,
            OnSavedGameSelected);
    }


    public void OnSavedGameSelected (SelectUIStatus status, ISavedGameMetadata game) {
        if (status == SelectUIStatus.SavedGameSelected) {
            // handle selected game save
        } else {
            // handle cancel or error
        }
    }


        void OpenSavedCloudGame(string filename) {
            
        ISavedGameClient savedGameClient = GooglePlayGames.PlayGamesPlatform.Instance.SavedGame;
        savedGameClient.OpenWithAutomaticConflictResolution(filename, DataSource.ReadCacheOrNetwork,
            ConflictResolutionStrategy.UseLongestPlaytime, OnSavedGameOpened);
    }

    public void OnSavedGameOpened(SavedGameRequestStatus status, ISavedGameMetadata game) {
        if (status == SavedGameRequestStatus.Success) {
            // handle reading or writing of saved game.
        } else {
            // handle error
        }
    }

        void SaveCloudGame (ISavedGameMetadata game, byte[] savedData, TimeSpan totalPlaytime) {
        ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
        Texture2D savedImage = getScreenshot();
        SavedGameMetadataUpdate.Builder builder = new SavedGameMetadataUpdate.Builder();
        builder = builder
            .WithUpdatedPlayedTime(totalPlaytime)
            .WithUpdatedDescription("Saved game at " + DateTime.Now);
        //if (savedImage != null) {
            // This assumes that savedImage is an instance of Texture2D
            // and that you have already called a function equivalent to
            // getScreenshot() to set savedImage
            // NOTE: see sample definition of getScreenshot() method below
            byte[] pngData = savedImage.EncodeToPNG();
            //builder = builder.WithUpdatedPngCoverImage(pngData);
        //}
        SavedGameMetadataUpdate updatedMetadata = builder.Build();
        savedGameClient.CommitUpdate(game, updatedMetadata, savedData, OnSavedGameWritten);
    }

    public void OnSavedGameWritten (SavedGameRequestStatus status, ISavedGameMetadata game) {
        if (status == SavedGameRequestStatus.Success) {
            // handle reading or writing of saved game.
        } else {
            // handle error
        }
    }

    public Texture2D getScreenshot() {
        // Create a 2D texture that is 1024x700 pixels from which the PNG will be
        // extracted
        Texture2D screenShot = new Texture2D(1024, 700);

        // Takes the screenshot from top left hand corner of screen and maps to top
        // left hand corner of screenShot texture
        screenShot.ReadPixels(
            new Rect(0, 0, Screen.width, (Screen.width/1024)*700), 0, 0);
        return screenShot;
    }

        void LoadCloudGameData (ISavedGameMetadata game) {
        ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
        savedGameClient.ReadBinaryData(game, OnSavedGameDataRead);
    }

    public void OnSavedGameDataRead (SavedGameRequestStatus status, byte[] data) {
        if (status == SavedGameRequestStatus.Success) {
            // handle processing the byte array data
        } else {
            // handle error
        }
    }

    /*
    public PurchaseProcessingResult ProcessPurchase (PurchaseEventArgs e)
{
    bool validPurchase = true; // Presume valid for platforms with no R.V.
    #endif
    // Unity IAP's validation logic is only included on these platforms.
#if UNITY_ANDROID 
    // Prepare the validator with the secrets we prepared in the Editor
    // obfuscation window.
    var validator = new CrossPlatformValidator(GooglePlayTangle.Data(),
        AppleTangle.Data(), Application.bundleIdentifier);

    try {
        // On Google Play, result has a single product ID.
        // On Apple stores, receipts contain multiple products.
        var result = validator.Validate(e.purchasedProduct.receipt);
        // For informational purposes, we list the receipt(s)
        Debug.Log("Receipt is valid. Contents:");
        foreach (IPurchaseReceipt productReceipt in result) {
            Debug.Log(productReceipt.productID);
            Debug.Log(productReceipt.purchaseDate);
            Debug.Log(productReceipt.transactionID);
        }
    } catch (IAPSecurityException) {
        Debug.Log("Invalid receipt, not unlocking content");
        validPurchase = false;
    }


    if (validPurchase) {
        // Unlock the appropriate content here.
    }

    return PurchaseProcessingResult.Complete;
}
#endif
*/


}
