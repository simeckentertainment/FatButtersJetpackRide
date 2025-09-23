using UnityEditor;
using UnityEditor.AssetImporters;
using UnityEditor.Build;
using UnityEngine;
using System.IO;

using System.Collections.Generic;
using Unity;
using System.Linq;

/*
public class CreateWindowsAssetBundles
{
    [MenuItem("FatButters Tools/Build Asset Bundles/Windows Only")]
    static void BuildWindowsAssetBundles()
    {
        AssetDatabase.Refresh();
        string assetBundleDirectory = "Assets/AssetBundles/Windows";
        if(!Directory.Exists(assetBundleDirectory))
        {
            Directory.CreateDirectory(assetBundleDirectory);
        }
        BuildPipeline.BuildAssetBundles(assetBundleDirectory, 
                                        BuildAssetBundleOptions.None, 
                                        BuildTarget.StandaloneWindows);
        AssetDatabase.Refresh();
    }
    */
/*
}
public class CreateAndroidAssetBundles
{
    [MenuItem("FatButters Tools/Build Asset Bundles/Android Only")]
    static void BuildAndroidAssetBundles()
    {
        AssetDatabase.Refresh();
        string assetBundleDirectory = "Assets/AssetBundles/Android";
        if(!Directory.Exists(assetBundleDirectory))
        {
            Directory.CreateDirectory(assetBundleDirectory);
        }
        BuildPipeline.BuildAssetBundles(assetBundleDirectory, 
                                        BuildAssetBundleOptions.None, 
                                        BuildTarget.Android);
        AssetDatabase.Refresh();
    }
}
*/
/*

public class CreateIOSAssetBundles
{
    [MenuItem("FatButters Tools/Build Asset Bundles/iOS")]
    static void BuildIOSAssetBundles()
    {
        EditorUserBuildSettings.overrideMaxTextureSize = 2048;
        AssetDatabase.Refresh();
        string assetBundleDirectory = "Assets/AssetBundles/iOS";
        if(!Directory.Exists(assetBundleDirectory))
        {
            Directory.CreateDirectory(assetBundleDirectory);
        }
        BuildPipeline.BuildAssetBundles(assetBundleDirectory, 
                                        BuildAssetBundleOptions.None, 
                                        BuildTarget.iOS);
        EditorUserBuildSettings.overrideMaxTextureSize = 512;
        AssetDatabase.Refresh();
    }

}
*/
/*
public class CreateLevelBundle{
    [MenuItem("FatButters Tools/Level Tools/Bundle Levels")]
    static void BuildLevelBundle(){
        List<string> levelFiles = new List<string>();
        string levelFolderString = "Assets/Scenes/puzzleLevels/";
        string topFolderString = "Assets/Scenes/";
        string[] LevelFolderFileList = Directory.GetFiles(levelFolderString);
        string[] TopFolderFileList = Directory.GetFiles(topFolderString);
        foreach(string file in LevelFolderFileList){
            if(file.Contains(".butters") & !file.Contains(".meta")){
                levelFiles.Add(file);
            }
        }
        foreach (string file in TopFolderFileList){
            if(file.Contains(".butters") & !file.Contains(".meta")){
                levelFiles.Add(file);
            }
        }
        AssetBundleBuild[] buildMap = new AssetBundleBuild[1];
        buildMap[0].assetBundleName = "levels";
        buildMap[0].assetNames = levelFiles.ToArray();
        //BuildPipeline.BuildAssetBundles("Assets/AssetBundles/iOS", buildMap, BuildAssetBundleOptions.None, BuildTarget.iOS);
        BuildPipeline.BuildAssetBundles("Assets/AssetBundles/Windows", buildMap, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
        BuildPipeline.BuildAssetBundles("Assets/AssetBundles/Android", buildMap, BuildAssetBundleOptions.None, BuildTarget.Android);

    }
}

public class CreateAllAssetBundles
{
    [MenuItem("FatButters Tools/Build Asset Bundles/Build All Asset Bundles")]
    static void BuildALLAssetBundles()
    {


        AssetDatabase.Refresh(); //first refresh the database so we are always working with up to date info
        //string iOSDir = "Assets/AssetBundles/iOS"; //Set iOS directory
        string AndroidDir = "Assets/AssetBundles/Android"; //Set Android directory
        string WindowsDir = "Assets/AssetBundles/Windows"; //set Win directory
        /*if(!Directory.Exists(iOSDir)){
            Directory.CreateDirectory(iOSDir);
        }

        //Prepping for building level bundles, too. Let's make this a one-click solution.
        List<string> levelFiles = new List<string>();
        string levelFolderString = "Assets/Scenes/puzzleLevels/";
        string topFolderString = "Assets/Scenes/";
        string[] LevelFolderFileList = Directory.GetFiles(levelFolderString);
        string[] TopFolderFileList = Directory.GetFiles(topFolderString);
        foreach(string file in LevelFolderFileList){
            if(file.Contains(".butters") & !file.Contains(".meta")){
                levelFiles.Add(file);
            }
        }
        foreach (string file in TopFolderFileList){
            if(file.Contains(".butters") & !file.Contains(".meta")){
                levelFiles.Add(file);
            }
        }
        AssetBundleBuild[] buildMap = new AssetBundleBuild[1];
        buildMap[0].assetBundleName = "levels";
        buildMap[0].assetNames = levelFiles.ToArray();
        /*if(!Directory.Exists(iOSDir)){
            Directory.CreateDirectory(iOSDir);
        }
        BuildPipeline.BuildAssetBundles(iOSDir, BuildAssetBundleOptions.None, BuildTarget.iOS);
        BuildPipeline.BuildAssetBundles("Assets/AssetBundles/iOS", buildMap, BuildAssetBundleOptions.None, BuildTarget.iOS);

        if(!Directory.Exists(WindowsDir)){
            Directory.CreateDirectory(WindowsDir);
        }
        //Building for placeable objects first because it generates an unusable level bundle.
        //BuildPipeline.BuildAssetBundles(WindowsDir, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
        //NOW build the level bundle.
        BuildPipeline.BuildAssetBundles(WindowsDir, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
        BuildPipeline.BuildAssetBundles(WindowsDir, buildMap, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
        
        if(!Directory.Exists(AndroidDir)){
            Directory.CreateDirectory(AndroidDir);
        }
        //Building for placeable objects first because it generates an unusable level bundle.
        BuildPipeline.BuildAssetBundles(AndroidDir, BuildAssetBundleOptions.None, BuildTarget.Android);
        //NOW build the level bundle.
        BuildPipeline.BuildAssetBundles(AndroidDir, buildMap, BuildAssetBundleOptions.None, BuildTarget.Android);
        AssetDatabase.Refresh();
        if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.Android){
            // Switch the build target to Android
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);
            // Log a message indicating the change
        }
    }
 //done
}
public class MoveBundlesForEditor {
    [MenuItem("FatButters Tools/Build Asset Bundles/Install Editor Bundles")]
    static void MoveBundles(){
        string source = "Assets/AssetBundles/Windows";
        string destination = Application.persistentDataPath + "/AssetBundles/";
        //first delete all files in destination except for the important one.
        string[] filesToDelete = Directory.GetFiles(destination);
        foreach (string file in filesToDelete){
            if (Path.GetFileName(file) != "FatButtersAssetBundleVersions.txt")
            {
                File.Delete(file);
            }
        }
        string[] filesToCopy = Directory.GetFiles(source);
        foreach (string file in filesToCopy)
        {
            string destinationFilePath = Path.Combine(destination, Path.GetFileName(file));
            if(!file.Contains(".meta") & !file.Contains("Windows/Windows")){
                File.Copy(file, destinationFilePath, true);
            }
        }
         //Refresh the Unity Editor to reflect changes in the Asset Database
        Debug.Log("Asset Bundles updated successfully!");
    }
    
}
*/
//
// Class to make XLF files recognized as Text Assets
// 
[ScriptedImporter(1, "butters")]
public class BUTTERSImporter : ScriptedImporter
{
    public override void OnImportAsset(AssetImportContext ctx)
    {

        var butters = new TextAsset(File.ReadAllText(ctx.assetPath));

        ctx.AddObjectToAsset("main", butters);
        ctx.SetMainObject(butters);
    }
}