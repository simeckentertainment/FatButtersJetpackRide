using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] SaveManager saveManager;
    public GameObject playerObj;
    [SerializeField] Player player;
    [SerializeField] public TMP_Text bonesText;
    [SerializeField] Image fuelGuage;
    [SerializeField] Sprite[] FuelGuageColors;
    [Header("Pause menu Stuff")]
    [SerializeField] GameObject PauseButton;
    [SerializeField] GameObject PauseMenu;

    [Header("Failure Menu Stuff")]
    [SerializeField] public TMP_Text FailText;
    [SerializeField] public GameObject FailMenu;
    [SerializeField] public GameObject savedBonesText;
    [Header("Success Menu Stuff")]
    [SerializeField] public GameObject WinMenu;
    [SerializeField] public TMP_Text endLevelStats;

    [Header("Damage Indicator stuff")]
    [SerializeField] Renderer HurtIndicator;
    [SerializeField] public bool runningHurt;
    [SerializeField] public int hurtCounter = 0;
    int hurtCounterThreshold = 20;
    [SerializeField] GameObject GameplayIndicators;



    // Start is called before the first frame update
    void Start()
    {
        if (saveManager == null) {
            saveManager = Helper.NabSaveData().GetComponent<SaveManager>();
        }
        HurtIndicator.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        SetUIIndicators();
    }
    public void ActivateHurt()
    {
        runningHurt = true;
    }
    public void SetUIIndicators()
    {
        fuelManager();
        bonesText.text = (player.saveManager.collectibleData.BONES + player.tempBones).ToString();
        if (runningHurt)
        {
            HurtIndicator.gameObject.SetActive(true);
            HurtRunner();
        }
    }
    public void SetEndLevelStats(int newbones) {
        endLevelStats.text = "You have found " + newbones + " new bones!";
    }
    public void HurtRunner() {
        hurtCounter++;
        Vector2 inTiling = Vector2.one;
        Vector2 outTiling = new Vector2(0.1f, 0.1f);
        Vector2 inOffset = Vector2.zero;
        Vector2 outOffset = new Vector2(0.5f, 0.5f);
        if (hurtCounter <= hurtCounterThreshold * 0.5f) {
            HurtIndicator.material.mainTextureScale = Vector2.Lerp(outTiling, inTiling, Helper.RemapToBetweenZeroAndOne(0.0f, hurtCounterThreshold * 0.5f, hurtCounter));
            HurtIndicator.material.mainTextureOffset = Vector2.Lerp(outOffset, inOffset, Helper.RemapToBetweenZeroAndOne(0.0f, hurtCounterThreshold * 0.5f, hurtCounter));
        } else if (hurtCounter > hurtCounterThreshold * 0.5f && hurtCounter < hurtCounterThreshold) {
            HurtIndicator.material.mainTextureScale = Vector2.Lerp(inTiling, outTiling, Helper.RemapToBetweenZeroAndOne(hurtCounterThreshold * 0.5f, hurtCounterThreshold, hurtCounter));
            HurtIndicator.material.mainTextureOffset = Vector2.Lerp(inOffset, outOffset, Helper.RemapToBetweenZeroAndOne(hurtCounterThreshold * 0.5f, hurtCounterThreshold, hurtCounter));
        } else {
            hurtCounter = 0;
            runningHurt = false;
            HurtIndicator.gameObject.SetActive(false);
        }
    }
    public void RunOneHitKill()
    {
        DisableGameplayIndicators();
        FailMenu.SetActive(true);
        playerObj.gameObject.SetActive(false);
        FailText.text = "You are no more.\nMaybe don't touch that again!";
    }
    public void RunNoFuel() {
        FailMenu.SetActive(true);
        FailText.text = "You ran out of fuel!\nTry upgrading your fuel in the store!";
    }
    public void ActivateWinMenu() {
        DisableGameplayIndicators();
        WinMenu.SetActive(true);
    }
    public void ActivateFailMenu() {
        DisableGameplayIndicators();
        FailMenu.SetActive(true);
    }
    public void PauseGame() {
        DisableGameplayIndicators();
        Time.timeScale = 0.0f;
    }
    public void UnpauseGame() {
        saveManager.Save();
        EnableGameplayIndicators();
        Time.timeScale = 1.0f;
    }

    public void EnableGameplayIndicators()
    {
        GameplayIndicators.SetActive(true);
    }
    public void DisableGameplayIndicators()
    {
        GameplayIndicators.SetActive(false);
    }

    #region buttonStuff
    public void FailToLevelSelect()
    {
        saveManager.collectibleData.BONES = saveManager.collectibleData.BONES + player.tempBones;
        saveManager.collectibleData.HASBALL = false;
        saveManager.Save();
        //UnpauseGame();
        Helper.LoadToLevel(Levels.LevelSelect);
    }
    public void PauseToLevelSelect() {
        saveManager.collectibleData.HASBALL = false;
        saveManager.Save();
        UnpauseGame();
        Helper.LoadToLevel(Levels.LevelSelect);
    }
    public void WinToLevelSelect() {
        saveManager.collectibleData.BONES = saveManager.collectibleData.BONES + player.tempBones;
        saveManager.collectibleData.HASBALL = false;
        saveManager.collectibleData.LevelBeaten[saveManager.sceneLoadData.LastLoadedLevelInt] = true;
        saveManager.Save();
        Helper.LoadToLevel(Levels.LevelSelect);

    }
    public void ActivatePauseMenu() {
        PauseButton.SetActive(false);
        PauseMenu.SetActive(true);
        PauseGame();
    }
    public void DeActivatePauseMenu() {
        PauseButton.SetActive(true);
        PauseMenu.SetActive(false);
        UnpauseGame();
    }
    #endregion
    #region fuelStuff
    void fuelManager()
    {
        //fuelGuage.m_FillAmount = player.fuel/player.maxFuel;
        fuelGuage.fillAmount = player.fuel / player.maxFuel;
        if (fuelGuage.fillAmount > 0.5f)
        {
            fuelGuage.sprite = FuelGuageColors[0];
        }
        if (fuelGuage.fillAmount <= 0.5f & fuelGuage.fillAmount > 0.25f)
        {
            fuelGuage.sprite = FuelGuageColors[1];
        }
        if (fuelGuage.fillAmount <= 0.25f)
        {
            fuelGuage.sprite = FuelGuageColors[2];
        }

    }
#endregion
}
