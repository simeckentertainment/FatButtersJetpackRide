using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

[System.Serializable]
public class DataHandler : MonoBehaviour
{
    [SerializeField] SaveManager saveManager;
    [SerializeField] CorgiEffectHolder FX;
    public CollectibleData collectibleData;
    public float currentFuel;
    [System.NonSerialized] public float maxFuel;
    [System.NonSerialized] public float fuelPercent;
    public float currentTummy;
    [System.NonSerialized] public float maxTummy;
    [System.NonSerialized] public float tummyPercent;
    [SerializeField] public bool OneHitKill;
    [SerializeField] public bool isDead;
    [SerializeField] InputDriver input;
    public int BonesGatheredThisLevel;
    public bool hasPermanentBall; //permanent ball lasts for one level. Costs money.
    public bool hasTemporaryBall;
    int ballTimer;
    [SerializeField] int ballTimerMax;
    public bool gameIsPaused;
    public bool levelComplete;
    [SerializeField] UIManager uIManager;
    bool gainsApplied;

    // Start is called before the first frame update
    void Start()
    {
        ballTimer = 0;
        OneHitKill = false;
        gainsApplied = false;
        levelComplete = false;
        gameIsPaused = false;
        SetupDataForLevel();

    }


    // Update is called once per frame
    void FixedUpdate(){
        if(input.GoThrust){
            if (!collectibleData.HASBALL && !collectibleData.GameplayTestingMode){
                ChangeFuelAmount(-0.5f);
            }
        }
        if (currentTummy <= 0){
            if(!isDead){
                if (OneHitKill){
                    uIManager.RunOneHitKill();
                } else { 
                    //uIManager.RunNoTummy();
                }
                Die();
            }
        }
        if (currentFuel <= 0){
            if(!isDead){
                uIManager.RunNoFuel();
                Die();
            }
        }
        if(isDead){
            if(!gainsApplied){
                ApplyAllPermanentGains();
                gainsApplied = true;
            }
        }

        FigureOutPercents();
        if(hasTemporaryBall){
            ballTimer++;
            FX.BallEffectRunner();
            if(ballTimer == ballTimerMax){
                hasTemporaryBall = false;
                ballTimer = 0;
                FX.BallEffectCanceler();
                if(!hasPermanentBall){
                    collectibleData.HASBALL = false;
                }
            }
        }
        if(hasPermanentBall){
            FX.BallEffectRunner();
        }

        if(levelComplete){
            collectibleData.HASBALL = false;
            hasPermanentBall = false;
            FX.BallEffectCanceler();
            levelComplete = false;
            saveManager.Save();
            uIManager.ActivateWinMenu();
        }
    }

    void FigureOutPercents(){
        fuelPercent = currentFuel/maxFuel;
        tummyPercent = currentTummy/maxTummy;
    }
    void SetupDataForLevel(){
        BonesGatheredThisLevel = 0;
        maxFuel = collectibleData.fuelUpgradeLevel*20.0f;
        currentFuel = maxFuel;
        maxTummy = collectibleData.treatsUpgradeLevel*20.0f;
        currentTummy = maxTummy;
        FigureOutPercents();
        hasPermanentBall = collectibleData.HASBALL;
    }
    public void ChangeFuelAmount(float amount){
        if(!collectibleData.HASBALL){
            currentFuel += amount;
            if(currentFuel > maxFuel){
                currentFuel = maxFuel;
            }
        }
    }
    public void ChangeTummyAmount(float amount){
        if(!collectibleData.HASBALL){
            currentTummy += amount;
            if(currentTummy > maxTummy){
                currentTummy = maxTummy;
            }
        }
    }
    public void Die(){
        isDead = true;
    }
    public void ClearAllPermanentGains(){
        //clears all gains.
        BonesGatheredThisLevel = 0;
    }
    public void ApplyAllPermanentGains(){
        //Applies all gains.
        collectibleData.BONES += BonesGatheredThisLevel;
        saveManager.Save();
    }
    public void ClearBall(){
        collectibleData.HASBALL = false;
    }
    public void AddBone(){
        BonesGatheredThisLevel += 1;
    } 
    public void MarkLevelComplete(int completedLevel){

        collectibleData.LevelBeaten[completedLevel] = true;
        saveManager.Save();

    }

    public void PauseGame(){
        Time.timeScale = 0.0f;
        gameIsPaused = true;
    }
    public void UnpauseGame(){
        Time.timeScale = 1.0f;
        gameIsPaused = false;
    }

    /*
    public void HandleAdStuff()
    {
        sceneLoadData.adHistoryCounter += 1;
        if(sceneLoadData.adHistoryCounter > 2 & IronSource.Agent.isInterstitialReady()) {
            sceneLoadData.adHistoryCounter = 0;
            IronSource.Agent.showInterstitial(); 
        }

    }
    */
}
