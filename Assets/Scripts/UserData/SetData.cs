using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class SetData : MainMenuCommonData
{
    [SerializeField] TMP_InputField codeEntry;
    [SerializeField] TMP_Text codeFailText;
    //This is a set of dev cheats so that I can get around easily and prototype stuff.
    //during production I will change these to unlock stuff with (very) long codes.

    public void codefail(){
        codeEntry.text = "";
        codeFailText.gameObject.SetActive(true);
    }
    public void Test(){
        Debug.Log("Derp");
    }
    public void DevBones(){
        saveManager.collectibleData.BONES = 999;
        save();
    }
    public void DevFuel(){
        saveManager.collectibleData.fuelUpgradeLevel = 999;
        save();
    }
    public void DevThrust(){
        saveManager.collectibleData.thrustUpgradeLevel = 50;
        save();
    }
    public void DevKillAds(){
        saveManager.collectibleData.killAds = true;
        save();
    }
    public void DevBall(){
        saveManager.collectibleData.HASBALL = true;
        save();
    }
    public void DevSkin1(){
        saveManager.collectibleData.HaveSkins[1] = true;
        save();
    }
    public void DevSkin2(){
        saveManager.collectibleData.HaveSkins[2] = true;
        save();
    }
    public void DevSkin3(){
        saveManager.collectibleData.HaveSkins[3] = true;
        save();
    }
    public void DevSkin4(){
        saveManager.collectibleData.HaveSkins[4] = true;
        save();
    }
    public void DevSkin5(){
        saveManager.collectibleData.HaveSkins[5] = true;
        save();
    }
    public void DevSkin6(){
        saveManager.collectibleData.HaveSkins[6] = true;
        save();
    }
    public void DevIDKFA(){
        saveManager.CreateCompletedSave();
    }
    public void DevLevels(){
        for(int i=0;i<=21;i++){
            saveManager.collectibleData.LevelBeaten[i] = true;
        }
        save();
    }

    void save(){
        saveManager.Save();
    }
}
