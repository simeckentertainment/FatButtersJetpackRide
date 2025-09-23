using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstanceDataParser : MonoBehaviour
{
    //Interprets the extra data present in the .butters file, if it exists.
    public string ExtraDataString;
    string objectType;


    void Start(){
        objectType = DetermineObjectType();
        switch(objectType){
            case "KissyFish":
                LoadKissyFishData(ExtraDataString);
                break;
            case "SegwayBear":
                LoadSegwayBearData(ExtraDataString);
                break;
            case "Bat":
                LoadBatData(ExtraDataString);
                break;
            case "LevelButton":
                //LoadLevelButtonData(ExtraDataString);
                break;
            case "CowboyBoss":
                LoadCowboyBossData(ExtraDataString);
                break;
            default:

                break;
        }
    }

    public string GetLevelCompileData(){
        string type = DetermineObjectType();
        string data = "";
        switch(type){
            case "KissyFish":
                data = SaveKissyFishData();
                break;
            case "SegwayBear":
                data = SaveSegwayBearData();
                break;
            case "Bat":
             data = SaveBatData();
                break;
            case "Furret":

                break;
            case "LevelButton":
                //data = SaveLevelButtonData();
                break;
            case "CowboyBoss":
                data = SaveCowboyBossData();
                break;
            default:
                break;
        }
        return data;
    }

#region KissyFish
    public string SaveKissyFishData(){
        KissyFishSpawner kissyFishSpawner = GetComponent<KissyFishSpawner>();
        string startBoxTrans = "StartBoxTrans: " + kissyFishSpawner.spawnPoint.localPosition.ToString();
        string spawnWidthBox = "SpawnWidthBoxTrans: " + kissyFishSpawner.spawnWidthBox.localPosition.ToString();
        string spawnPowerBox = "SpawnPowerBox: " + kissyFishSpawner.spawnPowerBox.localPosition.ToString();
        string spawnChance = "SpawnChance: " + kissyFishSpawner.spawnChance.ToString();
        string maxSpawnedFish = "MaxSpawnedFish: " + kissyFishSpawner.maxSpawnedFish.ToString();
        return startBoxTrans + "/" + spawnWidthBox + "/" + spawnPowerBox + "/" + spawnChance + "/" + maxSpawnedFish;
    }
    public void LoadKissyFishData(string dataString){
        KissyFishSpawner kissyFishSpawner = GetComponent<KissyFishSpawner>();
        string[] Datalines = dataString.Split("/");
        string[] SPstring = Datalines[0].Split(":");
        string[] SWBstring = Datalines[1].Split(":");
        string[] SPBstring = Datalines[2].Split(":");
        string[] SCstring = Datalines[3].Split(":");
        string[] MSFstring = Datalines[4].Split(":");
        kissyFishSpawner.spawnPoint.transform.localPosition = ParseVector3Data(SPstring[1]);
        kissyFishSpawner.spawnWidthBox.transform.localPosition = ParseVector3Data(SWBstring[1]);
        kissyFishSpawner.spawnPowerBox.transform.localPosition = ParseVector3Data(SPBstring[1]);
        kissyFishSpawner.spawnChance = ParseIntData(SCstring[1]);
        kissyFishSpawner.maxSpawnedFish = ParseIntData(MSFstring[1]);
    }
#endregion
#region SegwayBear
    public string SaveSegwayBearData(){
        SegwayBear segwayBear = GetComponentInChildren<SegwayBear>();
        string leftBoundaryTrans = "LeftBound: " + segwayBear.boundaries[0].transform.localPosition.ToString();
        string rightBoundaryTrans = "RightBound: " + segwayBear.boundaries[1].transform.localPosition.ToString();
        return leftBoundaryTrans + "/" + rightBoundaryTrans;
    }
    public void LoadSegwayBearData(string dataString){
        SegwayBear segwayBear = GetComponentInChildren<SegwayBear>();
        string[] Datalines = dataString.Split("/");
        string[] LBstring = Datalines[0].Split(":");
        string[] RBstring = Datalines[1].Split(":");
        segwayBear.boundaries[0].transform.localPosition = ParseVector3Data(LBstring[1]);
        segwayBear.boundaries[1].transform.localPosition = ParseVector3Data(RBstring[1]);

    }
#endregion
#region Bat

    public string SaveBatData(){
        BatSpawner batSpawner = GetComponent<BatSpawner>();
        string UpperOffset = "UpperOffset: " + batSpawner.UpperOffset.localPosition.ToString();
        string LowerOffset = "LowerOffset: " + batSpawner.LowerOffset.localPosition.ToString();
        string FlightPathPoints = "FlightPathPoints: ";
        for(int i=0;i<batSpawner.FlightPath.Length;i++){
            FlightPathPoints = FlightPathPoints + batSpawner.FlightPath[i].localPosition.ToString();
        }
        string SpawnLength = "SpawnLength: " + batSpawner.spawnLength.ToString();
        string SpawnAmount = "SpawnAmount: " + batSpawner.spawnAmount.ToString();
        string BatSpeed = "BatSpeed: " + batSpawner.BatSpeed.ToString();
        return UpperOffset + "/" + LowerOffset + "/" + FlightPathPoints + "/" + SpawnLength + "/" + SpawnAmount + "/" + BatSpeed;

    }

    public void LoadBatData(string dataString){
        BatSpawner batSpawner = GetComponent<BatSpawner>();
        string[] DataLines = dataString.Split("/");
        string[] ULString = DataLines[0].Split(":");
        string[] LLString = DataLines[1].Split(":");
        string[] FPPString = DataLines[2].Split(":");
        string[] SLString = DataLines[3].Split(":");
        string[] SAString = DataLines[4].Split(":");
        string[] BSString = DataLines[5].Split(":");

        batSpawner.UpperOffset.localPosition = ParseVector3Data(ULString[1]);
        batSpawner.LowerOffset.localPosition = ParseVector3Data(LLString[1]);
        string[] FPPVector3StringArray = FPPString[1].Split(")");
        for (int i=0; i<FPPVector3StringArray.Length;i++){
            batSpawner.FlightPath[i].localPosition =  ParseVector3Data(FPPVector3StringArray[i]);
        }
        batSpawner.spawnLength = ParseIntData(SLString[1]);
        batSpawner.spawnAmount = ParseIntData(SAString[1]);
        batSpawner.BatSpeed = ParseIntData(BSString[1]);

    }
#endregion
#region CowboyBoss
    public string SaveCowboyBossData(){
        CowboyBoss cb = GetComponent<CowboyBoss>();
        string cbType = "Role:" + DetermineCowboyBossType(cb);
        return cbType;
    }
    public void LoadCowboyBossData(string dataString){
        CowboyBoss cb = GetComponent<CowboyBoss>();
        string[] Rolestring = dataString.Split(":");
        switch (Rolestring[1]){
            case "Battle":
                cb.role = CowboyBoss.Role.Battle;
                break;
            case "LevelSelect":
                cb.role = CowboyBoss.Role.LevelSelect;
                break;
            default:
                break;
        }

    }
        string DetermineCowboyBossType(CowboyBoss cb){
        switch(cb.role){ 
            case CowboyBoss.Role.Battle:
                return "Battle";
            case CowboyBoss.Role.LevelSelect:
                return "LevelSelect";
            default:
                return "";
        }
    }
#endregion
#region furret

    public string SaveFurretData(){
        Furret furret = GetComponent<Furret>();
        string furretType = "FurretType: " + DetermineFurretType(furret);

        return "";
    }
    public void LoadFurretData(string dataString){
        Furret furret = GetComponent<Furret>();

    }
    string DetermineFurretType(Furret furret){
        switch(furret.furretType){ 
            case Furret.FurretType.NosePoker:
                return "NosePoker";
            case Furret.FurretType.Jumper:
                return "Jumper";
            case Furret.FurretType.Boss:
                return "boss";
            default:
                return "";
        }
    }
#endregion
/*
#region levelButton
    public string SaveLevelButtonData(){
        string isBossLevel;
        LevelButtonIDHolder holder = GetComponent<LevelButtonIDHolder>();
        string levelId = "LevelId:" + holder.levelID.ToString();
        if(holder.IsBossLevel){
            isBossLevel = "isBossLevel:" + "y";
        } else{
            isBossLevel = "isBossLevel:" + "n";
        }

        return levelId + "/" + isBossLevel;
    }
    public void LoadLevelButtonData(string dataString){
        LevelButtonIDHolder holder = GetComponent<LevelButtonIDHolder>();
        string[] Dataline = dataString.Split("/");
        string[] lIDString = Dataline[0].Split(":");
        string[] iblString = Dataline[1].Split(":");
        holder.levelID = ParseIntData(lIDString[1]);
        if(iblString[1] == "y"){
            holder.IsBossLevel = true;
        } else {
            holder.IsBossLevel = false;
        }
        holder.fireball.SetActive(holder.IsBossLevel);
    }

#endregion
*/
    #region DataParsers
    Vector3 ParseVector3Data(string data){
        string[] charsToRemove = new string[] { "(", ")", " "};
        foreach (var c in charsToRemove){
            data = data.Replace(c,string.Empty);
        }
        string[] pureNumbers = data.Split(",");
        return new Vector3(float.Parse(pureNumbers[0]),float.Parse(pureNumbers[1]),float.Parse(pureNumbers[2]));
    }

    int ParseIntData(string data){
        string[] charsToRemove = new string[] { "(", ")", " "};
                foreach (var c in charsToRemove){
            data = data.Replace(c,string.Empty);
        }
        return Int32.Parse(data);
    }
    #endregion
    string DetermineObjectType(){
        if(gameObject.GetComponent<KissyFishSpawner>() != null){
            return "KissyFish";
        }
        if(transform.Find("segwayBearObj") != null){
            return "SegwayBear";
        }
        if(gameObject.GetComponent<BatSpawner>() != null){
            return "Bat";
        }
        if(gameObject.GetComponent<Furret>() != null){
            return "Furret";
        }
        if(gameObject.GetComponent<LevelButtonIDHolder>() != null){
            return "LevelButton";
        }
        if(gameObject.GetComponent<CowboyBoss>() != null){
            return "CowboyBoss";
        }
        return "";
    }

}
