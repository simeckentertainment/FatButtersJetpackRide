using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using TMPro;
public class AgeGateChecker : InitializationCommonFunctions
{
    [SerializeField] GameObject[] AgeGateUiObjects;
    [SerializeField] TMP_Dropdown DayPicker;
    [SerializeField] TMP_Dropdown MonthPicker;
    [SerializeField] TMP_Dropdown YearPicker;
    [SerializeField] GameObject logo;
    UnityEngine.UI.Image logoImg;
    public List<string> YearOptions = new List<string>();
    bool ReadyToGo;
    int ReadyToGoThreshold = 30;
    int readyToGoCounter;
    [SerializeField] UnityEngine.UI.Image darkener;

    // Start is called before the first frame update
    void Start()
    {
        logoImg = logo.GetComponent<UnityEngine.UI.Image>();
        ReadyToGo = false;
        readyToGoCounter = 0;
        saveManager.Load();
        CreateYearOptions();

        if(saveManager.userInfo.AgeGateQuestionAnswered){
            ensureCorrectStatus();
            return;
        } else {
            darkener.gameObject.SetActive(true);
            logo.SetActive(false);
            foreach (GameObject obj in AgeGateUiObjects)
            {
                obj.SetActive(true);
            }
        }
    }
    void Update() {
        if(saveManager.userInfo.AgeGateQuestionAnswered){
            ReadyToGo = true;
            
        }
        if(ReadyToGo){
            readyToGoCounter++;
        }
        if(readyToGoCounter <= ReadyToGoThreshold){
            darkener.color = new Color(0,0,0,Helper.RemapArbitraryValues(0f,1f,0f,0.8f,(ReadyToGoThreshold-readyToGoCounter)/ReadyToGoThreshold));
            logoImg.color = new Color(1f,1f,1f,(ReadyToGoThreshold-readyToGoCounter)/ReadyToGoThreshold);
        } else {
            saveManager.sceneLoadData.SceneToLoad = Levels.TitleScreen;
            saveManager.sceneLoadData.LastLoadedLevelInt = 0;
            saveManager.sceneLoadData.LastLoadedLevel = "";
            saveManager.Save();
            SceneManager.LoadScene(Levels.SceneLoader);
        }
    }
    void ensureCorrectStatus(){
        //start with the year
        if(!saveManager.userInfo.isOldEnoughForAds){
            if(saveManager.userInfo.yearBorn < DateTime.Now.Year-13){ //if year older
                saveManager.userInfo.isOldEnoughForAds = true;
                saveManager.Save();
                return;
            } else if (saveManager.userInfo.yearBorn == DateTime.Now.Year-13) { //if at year threshold
                if(saveManager.userInfo.monthBorn < DateTime.Now.Month){ //if month older
                saveManager.userInfo.isOldEnoughForAds = true;
                saveManager.Save();
                return;
                } else if (saveManager.userInfo.monthBorn == DateTime.Now.Month){ //if at month threshold
                    if(saveManager.userInfo.dayBorn <= DateTime.Now.Day){ //if day older or at day threshold
                        saveManager.userInfo.isOldEnoughForAds = true;
                        saveManager.Save(); 
                    }  else { //if day younger
                        return;
                    }
                } else { //if month younger
                    return;
                }
            } else { //if year younger
                return;
            }
        }
    }

    private void CreateYearOptions()
    {
        YearPicker.ClearOptions();
        YearOptions.Add("Year");
        for (int i = 0; i < 100; i++)
        {
            YearOptions.Add((DateTime.Now.Year - i).ToString());
        }
        YearPicker.AddOptions(options: YearOptions);
    }

    // Update is called once per frame




    public void writeAgeValues(){
        saveManager.userInfo.monthBorn = MonthPicker.value;
        saveManager.userInfo.dayBorn = DayPicker.value;
        saveManager.userInfo.yearBorn = int.Parse(YearPicker.options[YearPicker.value].text);
        saveManager.Save();
    }
    public void isOldEnoughForAds(){
        saveManager.userInfo.isOldEnoughForAds = true;
        saveManager.userInfo.AgeGateQuestionAnswered = true;
        saveManager.Save();
    }
    public void notOldEnoughForAds(){
        saveManager.userInfo.isOldEnoughForAds = false;
        saveManager.userInfo.AgeGateQuestionAnswered = true;
        saveManager.Save();
    }
    public void submissionChecker(){

        int yearInt;
        yearInt = int.Parse(YearPicker.options[YearPicker.value].text);
        if(yearInt > DateTime.Now.Year - 13){ //if year younger
            writeAgeValues();
            notOldEnoughForAds();
            return;
        } else if ( YearPicker.value == 13) {//if same year
            if(MonthPicker.value > DateTime.Now.Month){ //if month younger
                writeAgeValues();
                notOldEnoughForAds();
                return;
            } else if( MonthPicker.value == DateTime.Now.Month){ //if same month
                if(DayPicker.value > DateTime.Now.Day){ //if day younger
                    writeAgeValues();
                    notOldEnoughForAds();
                    return;
                } else { //if day older or same as threshold
                    writeAgeValues();
                    isOldEnoughForAds();
                    return;
                }
            } else { //if month older
                writeAgeValues();
                isOldEnoughForAds();
                return;
            }
        } else { //if year older
            writeAgeValues();
            isOldEnoughForAds();
            return;
        }
    }
}
