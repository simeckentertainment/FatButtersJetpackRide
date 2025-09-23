using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameplayButtonManager : MainMenuCommonData
{
    [SerializeField] Player player;
    [SerializeField] CollectibleData collectibleData;
    [SerializeField] UIManager uIManager;
    [SerializeField] GameObject[] UIElements;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject pauseButton;
    [SerializeField] Button BoneDoublerButton;
    [SerializeField] TMP_Text boneDoublerText;
    [SerializeField] Toggle vibrationToggle;
    [SerializeField] Toggle OnScreenControlToggle;
    [SerializeField] GameObject OnScreenControlObj;


    void Start()
    {
        saveManager = Helper.NabSaveData().GetComponent<SaveManager>();
        saveManager.Load();
        SetToggleButtons();


    }

    void FixedUpdate()
    {

    }

    private void SetToggleButtons()
    {
        vibrationToggle.isOn = collectibleData.HapticsEnabled;
        OnScreenControlToggle.isOn = collectibleData.OnScreenControlsEnabled;
        OnScreenControlObj.SetActive(collectibleData.OnScreenControlsEnabled);
    }

    public void SaveAndReturnToMainMenu()
    {
        uIManager.UnpauseGame();
        saveManager.collectibleData.BONES += player.tempBones;
        saveManager.collectibleData.HASBALL = false;
        saveManager.Save();
        Levels.Load(Levels.LevelSelect);
    }
    public void DiscardAndReturnToMainMenu()
    {
        uIManager.UnpauseGame();
        saveManager.collectibleData.HASBALL = false;
        saveManager.Save();
        Levels.Load(Levels.LevelSelect);
    }
    public void PauseGame()
    {
        pauseButton.SetActive(false);
        pauseMenu.SetActive(true);
        foreach (GameObject uiElement in UIElements)
        {
            uiElement.SetActive(false);
        }
        uIManager.PauseGame();
    }
    public void ResumeGame()
    {
        pauseButton.SetActive(true);
        pauseMenu.SetActive(false);
        foreach (GameObject uiElement in UIElements)
        {
            uiElement.SetActive(true);
        }
        saveManager.Save();
        uIManager.UnpauseGame();
    }


    public void ToggleHaptics()
    {
        collectibleData.HapticsEnabled = !collectibleData.HapticsEnabled;
        saveManager.Save();
    }
    public void ToggleOnScreenControls()
    {
        collectibleData.OnScreenControlsEnabled = !collectibleData.OnScreenControlsEnabled;
        OnScreenControlObj.SetActive(collectibleData.OnScreenControlsEnabled);
        saveManager.Save();
    }


}
