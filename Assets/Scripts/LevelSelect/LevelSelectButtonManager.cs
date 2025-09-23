using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using NUnit.Framework;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using System.Collections.Generic;
public class LevelSelectButtonManager : MonoBehaviour
{
    [SerializeField] CollectibleData collectibleData;
    [SerializeField] UserInfo userInfo;
    [SerializeField] SaveManager saveManager;
    [SerializeField] LevelSelectScroller levelSelectScroller;
    [SerializeField] GameObject MainMenuButton;
    [SerializeField] Scrollbar scrollbar;

    #region SettingsVars
    [Header("Settings Menu Stuff")]
    [SerializeField] GameObject SettingsButton;
    [SerializeField] Transform SettingsMenuHolder;
    [SerializeField] Transform SettingsMenuUpPos;
    [SerializeField] Transform SettingsMenuDownPos;
    [SerializeField] GameObject SettingsPage1;
    [SerializeField] GameObject SettingsPage2;
    [SerializeField] GameObject SettingsPage3Disclaimer;
    [SerializeField] GameObject SettingsPage3;
    [SerializeField] Toggle OSCToggle;
    [SerializeField] Toggle HapticsToggle;
    [NonSerialized] bool SettingsMenuMoving;
    [NonSerialized] float SmMoveCounter;
    [NonSerialized] float SmMoveThreshold = 45f;
    [NonSerialized] Vector3 SeMeBePos; //SeMeBe stands for SettingsMenuBeginning
    [NonSerialized] Vector3 SeMeBeRot;
    [NonSerialized] Vector3 SeMeEnPos; //SeMeEn stands for SettingsMenuEnd
    [NonSerialized] Vector3 SeMeEnRot; //also, hehe... semen... Technically semeen but still funny.
    [SerializeField] Slider MasterVolumeSlider;
    [SerializeField] Slider MusicVolumeSlider;
    [SerializeField] Slider SFXVolumeSlider;
    [SerializeField] TMP_InputField InterstitialFrequencyTextbox;
    [SerializeField] TMP_Text InterstitialTBPlaceholder;
    [SerializeField] TMP_Text InterstitialTitleText;
    [SerializeField] TMP_Dropdown GraphicsQualityDropdown;
    [Header("Ad Settings stuff")]
    [SerializeField] Toggle LsBannersToggle;
    [SerializeField] GameObject LSBBonusText;
    [SerializeField] Toggle PMBannerToggle;
    [SerializeField] GameObject PMBBonusText;
    [SerializeField] Toggle InterstitialToggle;
    [SerializeField] GameObject ITBonusText;
    [SerializeField] TMP_InputField InterstitialFrequencyInput;
    [SerializeField] TMP_Text InterstitialPlaceholderText;
    int InterstitialFrequency;
    [SerializeField] Toggle BoneDoublerToggle;
    [SerializeField] GameObject BDTBonusText;

    #endregion
    #region ShopVars
    [Header("Shop menu stuff")]
    [SerializeField] GameObject ShopButton;
    [SerializeField] Transform ShopMenuHolder;
    [SerializeField] Transform ShopUpPos;
    [SerializeField] Transform ShopDownPos;
    [NonSerialized] bool ShopMoving;
    [NonSerialized] float ShopMoveCounter;
    [NonSerialized] float ShopMoveThreshold = 45f;
    [Tooltip("Sh Be stands for Shop Beginning")]
    [NonSerialized] Vector3 ShBePos;
    [Tooltip("Sh Be stands for Shop Beginning")]
    [NonSerialized] Vector3 ShBeRot;
    [Tooltip("Sh En stands for Shop End")]
    [NonSerialized] Vector3 ShEnPos;
    [Tooltip("Sh En stands for Shop End")]
    [NonSerialized] Vector3 ShEnRot;
    [SerializeField] Sprite treatsOpen;
    [SerializeField] Sprite treatsClosed;
    [SerializeField] Image treatsImgSlot;
    [SerializeField] public int CurrentSelectedShopItem;
    [SerializeField] TMP_Text itemName;
    [SerializeField] TMP_Text itemCost;
    [SerializeField] Image itemImg;
    [SerializeField] Image JetpackImg;
    [SerializeField] Image DoghouseButtersImg;
    [SerializeField] public ShopItem[] shopItems;
    [SerializeField] public Sprite cantGetRead;
    [SerializeField] Button BuyButton;
    [SerializeField] Button SetSkinButton;
    [SerializeField] AudioClip[] barks;
    #endregion
    void Start()
    {
        saveManager.EnsureSaveFileExists();
        SettingsMenuMoving = false;
        //Set the settings menu for all the settings options that need it...
        SetSettings();
    }
    void FixedUpdate()
    {
        if (SettingsMenuMoving) { RunSettingsMover(); }
        if (ShopMoving) { RunShopMover(); }
    }

    #region SettingsMethods
    void SetSettings()
    {
        saveManager.Load();
        //shop stuff
        SetAllShopText();
        //settings page 1 stuff
        MasterVolumeSlider.value = collectibleData.MasterVolumeLevel;
        MusicVolumeSlider.value = collectibleData.MusicVolumeLevel;
        SFXVolumeSlider.value = collectibleData.SFXVolumeLevel;
        //Settings page 2 stuff
        OSCToggle.isOn = collectibleData.OnScreenControlsEnabled;
        HapticsToggle.isOn = collectibleData.HapticsEnabled;
        GraphicsQualityDropdown.value = QualitySettings.GetQualityLevel();
        ShowSettingsPage(1);
        saveManager.Save();
    }

    private void SetAllShopText()
    {
        itemName.text = GetCurrentItemName();
        SetCurrentItemCostText(GetCurrentItemCost());
        itemImg.sprite = GetCurrentItemImg();
        JetpackImg.sprite = GetCurrentJetpackImg();
        if (CurrentSelectedShopItem < 3)
        {
            DoghouseButtersImg.sprite = shopItems[collectibleData.CurrentSkin + 3].dogHouseButtersImg;
            return;
        }
        //if (!DoWeOwnThisSkin()) { return; } //You know what? Let them see the skins in their full glory.
        if (GetCurrentDoghouseImg() != null)
        {
            DoghouseButtersImg.sprite = GetCurrentDoghouseImg();
        }
        else
        {
            DoghouseButtersImg.sprite = cantGetRead;
        }
    }

    bool DoWeOwnThisSkin()
    {
        return collectibleData.HaveSkins[CurrentSelectedShopItem - 3];
    }
    public void OpenSettingsMenu()
    {
        levelSelectScroller.menuOpen = true;
        SettingsMenuMoving = true;
        SetUiButtonVisibility(false);
        SeMeBePos = SettingsMenuDownPos.position;
        SeMeBeRot = SettingsMenuDownPos.rotation.eulerAngles;
        SeMeEnPos = SettingsMenuUpPos.position;
        SeMeEnRot = SettingsMenuUpPos.rotation.eulerAngles;
        SmMoveCounter = 0f;
        ShowSettingsPage(1);
    }
    public void CloseSettingsMenu()
    {
        levelSelectScroller.menuOpen = false;
        SettingsMenuMoving = true;
        SetUiButtonVisibility(true);
        SeMeBePos = SettingsMenuUpPos.position;
        SeMeBeRot = SettingsMenuUpPos.rotation.eulerAngles;
        SeMeEnPos = SettingsMenuDownPos.position;
        SeMeEnRot = SettingsMenuDownPos.rotation.eulerAngles;
        SmMoveCounter = 0f;
    }

    public void AdjustMasterVolume(Single value)
    {
        float adjustedVal = value * 0.9f;

        collectibleData.MasterVolumeLevel = adjustedVal;
        if (collectibleData.SFXVolumeLevel > adjustedVal)
        {
            collectibleData.SFXVolumeLevel = adjustedVal;
            SFXVolumeSlider.value = adjustedVal;
        }
        if (collectibleData.MusicVolumeLevel > adjustedVal)
        {
            collectibleData.MusicVolumeLevel = adjustedVal;
            MusicVolumeSlider.value = adjustedVal;
        }
        saveManager.Save();
    }
    public void AdjustMusicVolume(Single value)
    {

        float adjustedVal = value * 0.9f;
        collectibleData.MusicVolumeLevel = adjustedVal;
        if (adjustedVal > collectibleData.MasterVolumeLevel)
        {
            collectibleData.MusicVolumeLevel = collectibleData.MasterVolumeLevel;
            MusicVolumeSlider.value = collectibleData.MasterVolumeLevel;
        }
        saveManager.Save();
    }

    public void AdjustSFXVolume(Single value)
    {
        float adjustedVal = value * 0.9f;
        collectibleData.SFXVolumeLevel = adjustedVal;
        if (adjustedVal > collectibleData.MasterVolumeLevel)
        {
            collectibleData.SFXVolumeLevel = collectibleData.MasterVolumeLevel;
            SFXVolumeSlider.value = collectibleData.MasterVolumeLevel;
        }
        saveManager.Save();
    }

    public void ToggleOnScreenControls()
    {

        if (OSCToggle.isOn)
        {
            collectibleData.OnScreenControlsEnabled = true;
        }
        else
        {
            collectibleData.OnScreenControlsEnabled = false;
        }
        saveManager.Save();
    }
    public void ToggleHaptics()
    {
        if (HapticsToggle.isOn)
        {
            collectibleData.HapticsEnabled = true;
        }
        else
        {
            collectibleData.HapticsEnabled = false;
        }
        saveManager.Save();
    }

    public void OpenPrivacyPolicy()
    {
        Application.OpenURL("https://fatbutters.simeck.com/privacyPolicy.txt");
    }
    private void RunSettingsMover()
    {
        SmMoveCounter++;
        SettingsMenuHolder.position = Vector3.Lerp(SeMeBePos, SeMeEnPos, SmMoveCounter / SmMoveThreshold);
        SettingsMenuHolder.rotation = Quaternion.Euler(SettingsMenuHolder.rotation.eulerAngles.x, SettingsMenuHolder.rotation.eulerAngles.y, Mathf.Lerp(SeMeBeRot.z + 360, SeMeEnRot.z + 360, SmMoveCounter / SmMoveThreshold));
        if (SmMoveCounter >= SmMoveThreshold)
        {
            SettingsMenuMoving = false;
        }
    }

    public void SetGraphicsQualityLevel(Int32 setting)
    {
        QualitySettings.SetQualityLevel(setting);
        collectibleData.GraphicsQualityLevel = setting;
        saveManager.Save();
    }
    public void ShowSettingsPage(int page)
    {
        switch (page)
        {
            case 1:
                SettingsPage1.SetActive(true);
                SettingsPage2.SetActive(false);
                SettingsPage3Disclaimer.SetActive(false);
                SettingsPage3.SetActive(false);
                break;
            case 2:
                SettingsPage1.SetActive(false);
                SettingsPage2.SetActive(true);
                SettingsPage3Disclaimer.SetActive(false);
                SettingsPage3.SetActive(false);
                break;
            case 3:
                SettingsPage1.SetActive(false);
                SettingsPage2.SetActive(false);
                SettingsPage3Disclaimer.SetActive(false);
                SettingsPage3.SetActive(true);
                break;
            case 9: //this one is the ad disclaimer
                SettingsPage1.SetActive(false);
                SettingsPage2.SetActive(false);
                SettingsPage3Disclaimer.SetActive(true);
                SettingsPage3.SetActive(false);
                break;
            default:

                break;
        }
    }

    #endregion
    #region AdSettingsMethods
    public void ToggleLevelSelectBanners(Boolean whatDo)
    {
        LSBBonusText.SetActive(whatDo);
        userInfo.LevelSelectBanners = whatDo;
        saveManager.Save();
    }
    public void TogglePauseMenuBanners(bool whatDo)
    {
        PMBBonusText.SetActive(whatDo);
        userInfo.LevelSelectBanners = whatDo;
        saveManager.Save();
    }
    public void ToggleInterstitials(bool whatDo)
    {
        ITBonusText.SetActive(whatDo);
        userInfo.InterstitialToggle = whatDo;
        InterstitialFrequencyTextbox.gameObject.SetActive(whatDo);
        InterstitialTitleText.text = whatDo ? "Ads after every              levels" : "Ads after levels";
        InterstitialPlaceholderText.text = userInfo.InterstitialFrequency.ToString();
        saveManager.Save();
    }

    public void ChanceInterstitialFrequency(string amount)
    {
        userInfo.InterstitialFrequency = Int32.Parse(amount);
        saveManager.Save();
    }
    public void ToggleBoneDoubler(bool whatDo)
    {
        BDTBonusText.SetActive(whatDo);
        userInfo.BoneDoublerToggle = whatDo;
        saveManager.Save();
    }
    #endregion
    #region ShopMethods

    public void OpenShopMenu()
    {
        levelSelectScroller.menuOpen = true;
        ShopMoving = true;
        SetUiButtonVisibility(false);
        ShBePos = ShopUpPos.position;
        ShEnPos = ShopDownPos.position;
        ShopMoveCounter = 0;
    }
    public void CloseShopMenu()
    {
        levelSelectScroller.menuOpen = false;
        ShopMoving = true;
        SetUiButtonVisibility(true);
        ShBePos = ShopDownPos.position;
        ShEnPos = ShopUpPos.position;
        ShopMoveCounter = 0;
    }
    private void RunShopMover()
    {
        ShopMoveCounter++;
        ShopMenuHolder.position = Vector3.Lerp(ShBePos, ShEnPos, ShopMoveCounter / ShopMoveThreshold);
        if (ShopMoveCounter >= ShopMoveThreshold)
        {
            ShopMoving = false;
        }
    }
    public void ToggleTreatsHolder()
    {
        if (treatsImgSlot.sprite == treatsOpen)
        {
            treatsImgSlot.sprite = treatsClosed;
        }
        else
        {
            treatsImgSlot.sprite = treatsOpen;
        }
    }
    string GetCurrentItemName()
    {
        return shopItems[CurrentSelectedShopItem].itemName;
    }

    public int GetCurrentItemCost()
    {
        if (shopItems[CurrentSelectedShopItem].itemPrice == 0)
        { //If the price is 0, query the save data and respond accordingly.
          //There's only 3 dynamically priced items so we can just use a switch.
            switch (true)
            {
                case bool _ when Regex.IsMatch(shopItems[CurrentSelectedShopItem].itemName, ".*Tummy.*"):
                    return collectibleData.treatsUpgradeLevel;
                case bool _ when Regex.IsMatch(shopItems[CurrentSelectedShopItem].itemName, ".*Thrust.*"):
                    return collectibleData.thrustUpgradeLevel;
                case bool _ when Regex.IsMatch(shopItems[CurrentSelectedShopItem].itemName, ".*Fuel.*"):
                    return collectibleData.fuelUpgradeLevel;
                default:
                    return -1; //an error has occurred.
            }
        }
        else
        {
            return shopItems[CurrentSelectedShopItem].itemPrice;
        }
    }
    public void SetCurrentItemCostText(int amount)
    {
        if (amount == 0)
        {
            itemCost.text = "";
        }
        else
        {
            itemCost.text = amount.ToString() + "/" + collectibleData.BONES.ToString() + " Bones    ";
        }

    }
    Sprite GetCurrentItemImg()
    {
        return shopItems[CurrentSelectedShopItem].itemImg;
    }
    Sprite GetCurrentJetpackImg()
    {
        return shopItems[CurrentSelectedShopItem].JetpackImg;
    }

    Sprite GetCurrentDoghouseImg()
    {
        return shopItems[CurrentSelectedShopItem].dogHouseButtersImg;
    }
    public void BuyCurrentItem()
    {
        //Deferred to BuyScripts, but here for readability.
        GetComponent<BuyScripts>().RunBuy(CurrentSelectedShopItem);
    }

    public void SetCurrentSkin()
    {
        GetComponent<BuyScripts>().EnableCurrentSkin();
    }
    public void runRightShopButton()
    {
        CurrentSelectedShopItem++;
        if (CurrentSelectedShopItem >= shopItems.Length)
        {
            CurrentSelectedShopItem = 0;
        }
        SetAllShopText();
        EnsureCorrectBuyButton();
    }
    public void runLeftShopButton()
    {
        CurrentSelectedShopItem--;
        if (CurrentSelectedShopItem < 0)
        {
            CurrentSelectedShopItem = shopItems.Length - 1;
        }
        SetAllShopText();
        EnsureCorrectBuyButton();
    }
    public void EnsureCorrectBuyButton()
    { //This method uses a nested if statement. This bears looking at later and rethinking.
        if (CurrentSelectedShopItem < 3)
        {
            EnableBuyButton();
            return;
        }
        if (collectibleData.HaveSkins[shopItems[CurrentSelectedShopItem].SkinId] == true)
        { //if we have the skin...
            if (shopItems[CurrentSelectedShopItem].SkinId == collectibleData.CurrentSkin)
            { //And we have the skin equipped...
                EnableNoBuyOrSkinButton(); //kill the buttons.
            }
            else
            {
                EnableSetSkinButton(); //If we have the skin but it's unequipped, enable the equip button.
            }
        }
        else
        {
            EnableBuyButton(); //Otherwise enable the buy button.
        }
    }


    public void EnableBuyButton()
    {
        BuyButton.gameObject.SetActive(true);
        SetSkinButton.gameObject.SetActive(false);
    }
    public void EnableSetSkinButton()
    {
        BuyButton.gameObject.SetActive(false);
        SetSkinButton.gameObject.SetActive(true);
    }
    public void EnableNoBuyOrSkinButton()
    {
        BuyButton.gameObject.SetActive(false);
        SetSkinButton.gameObject.SetActive(false);
    }
    #endregion
    public void SetUiButtonVisibility(bool visibility)
    {
        MainMenuButton.SetActive(visibility);
        ShopButton.SetActive(visibility);
        SettingsButton.SetActive(visibility);
    }
    public void GoToTitleScreenButton()
    {
        Levels.Load(Levels.TitleScreen);

    }

    public void SetLevelScroll()
    {
        levelSelectScroller.SetLeftRightScrollAmount(scrollbar.value);
    }


    public void DoBark()
    {
        AudioClip thisBark = barks[UnityEngine.Random.Range(0, barks.Length - 1)];
        AudioSource sauce = DoghouseButtersImg.gameObject.GetComponent<AudioSource>();
        sauce.clip = thisBark;
        sauce.Play();

    }


}
