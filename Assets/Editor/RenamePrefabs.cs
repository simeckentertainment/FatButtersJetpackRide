using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;

public class RenamePrefabs : EditorWindow
{
    private const string Prefix = "P18_";
    private const string Suffix = "_UPFB_V001_RSS";
    private const string PrefabExtension = ".prefab";

    [MenuItem("FatButters Tools/Rename Prefabs in Selected Folder")]
    public static void ShowWindow()
    {
        // Check if a folder is selected
        if (Selection.assetGUIDs.Length == 0)
        {
            EditorUtility.DisplayDialog("No Folder Selected", "Please select a folder in the Project window.", "OK");
            return;
        }

        string path = AssetDatabase.GUIDToAssetPath(Selection.assetGUIDs[0]);

        if (!Directory.Exists(path))
        {
            EditorUtility.DisplayDialog("Invalid Selection", "The selected item is not a folder. Please select a folder.", "OK");
            return;
        }

        if (EditorUtility.DisplayDialog("Rename Prefabs?",
            $"Are you sure you want to rename all prefabs in the folder '{path}' and its subfolders?\n" +
            $"Original: FooBar.prefab\n" +
            $"New: {Prefix}FooBar{Suffix}{PrefabExtension}",
            "Yes", "No"))
        {
            RenamePrefabsInDirectory(path);
            EditorUtility.DisplayDialog("Renaming Complete", $"Prefab renaming in '{path}' and subfolders is complete.", "OK");
        }
    }

    private static void RenamePrefabsInDirectory(string directoryPath)
    {
        // Get all prefab files in the current directory
        string[] prefabGuids = AssetDatabase.FindAssets("t:Prefab", new[] { directoryPath });

        foreach (string guid in prefabGuids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            string fileName = Path.GetFileNameWithoutExtension(assetPath);
            string fileExtension = Path.GetExtension(assetPath);

            // Ensure it's a prefab file and not already renamed (to avoid double renaming issues)
            if (fileExtension.ToLower() == PrefabExtension && !fileName.StartsWith(Prefix) && !fileName.EndsWith(Suffix))
            {
                string directoryName = Path.GetDirectoryName(assetPath);
                string newFileName = Prefix + fileName + Suffix + PrefabExtension;
                string newPath = Path.Combine(directoryName, newFileName);

                // Check for naming conflicts before renaming
                if (AssetDatabase.LoadAssetAtPath<Object>(newPath) != null)
                {
                    Debug.LogWarning($"Skipping rename for '{assetPath}': A file named '{newFileName}' already exists at this location.");
                    continue;
                }

                string errorMessage = AssetDatabase.RenameAsset(assetPath, newFileName.Replace(PrefabExtension, "")); // RenameAsset expects name without extension
                if (!string.IsNullOrEmpty(errorMessage))
                {
                    Debug.LogError($"Failed to rename asset '{assetPath}': {errorMessage}");
                }
                else
                {
                    Debug.Log($"Renamed '{assetPath}' to '{newPath}'");
                }
            }
        }

        // Recursively process subdirectories
        string[] subdirectories = Directory.GetDirectories(directoryPath);
        foreach (string subDirectory in subdirectories)
        {
            RenamePrefabsInDirectory(subDirectory.Replace("\\", "/")); // Ensure forward slashes for Unity paths
        }
    }
}