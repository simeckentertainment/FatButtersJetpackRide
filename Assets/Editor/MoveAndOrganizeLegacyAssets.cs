using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using System.Collections.Generic;


//Code provided by ChatGPT.
//Yes it's vibe coded and no I'm not proud about it.
public class MoveAndOrganizeLegacyAssets
{
    private static string legacyPath = "Assets/Objects/LegacyObjectsDONOTDELETE";
    private static string destinationRoot = "Assets/Objects/PuzzlePieces/AssetModels/PreAlphaAssets";

    // Toggle this to false when you're ready to actually move files
    private static bool preview = false;

    private static string[] scenePaths = {
        "Assets/Scenes",
        "Assets/Scenes/PuzzleLevels",
        "Assets/Scenes/Sandboxes"
    };

    [MenuItem("FatButters Tools/Move and Organize Used Legacy Assets")]
    public static void MoveAndOrganize()
    {
        // Step 1: Gather legacy GUIDs
        string[] legacyGuids = AssetDatabase.FindAssets("", new[] { legacyPath });

        // Step 2: Map all project assets to GUIDs
        string[] allProjectAssets = AssetDatabase.GetAllAssetPaths()
            .Where(p => p.StartsWith("Assets/"))
            .ToArray();

        Dictionary<string, string> guidToPath = new Dictionary<string, string>();
        foreach (string assetPath in allProjectAssets)
        {
            string guid = AssetDatabase.AssetPathToGUID(assetPath);
            if (!string.IsNullOrEmpty(guid))
                guidToPath[guid] = assetPath;
        }

        // Step 3: Get all starting scene files
        List<string> startFiles = new List<string>();
        foreach (var sceneDir in scenePaths)
        {
            if (Directory.Exists(sceneDir))
                startFiles.AddRange(Directory.GetFiles(sceneDir, "*.unity", SearchOption.AllDirectories));
        }

        // Step 4: BFS to find all used GUIDs
        HashSet<string> visitedGuids = new HashSet<string>();
        Queue<string> toProcess = new Queue<string>();

        foreach (string scenePath in startFiles)
        {
            foreach (string guid in ExtractGuidsFromTextFile(scenePath))
                toProcess.Enqueue(guid);
        }

        while (toProcess.Count > 0)
        {
            string currentGuid = toProcess.Dequeue();
            if (!visitedGuids.Add(currentGuid)) continue;

            if (!guidToPath.ContainsKey(currentGuid)) continue;

            string path = guidToPath[currentGuid];

            if (path.EndsWith(".prefab") || path.EndsWith(".unity") ||
                path.EndsWith(".mat") || path.EndsWith(".asset"))
            {
                foreach (string referencedGuid in ExtractGuidsFromTextFile(path))
                    toProcess.Enqueue(referencedGuid);
            }
        }

        // Step 5: Identify used legacy assets
        var usedLegacyAssets = legacyGuids.Where(g => visitedGuids.Contains(g))
                                          .Select(g => AssetDatabase.GUIDToAssetPath(g))
                                          .ToList();

        // Step 6: Move or preview assets into organized folders
        foreach (var assetPath in usedLegacyAssets)
        {
            string typeFolder = GetTypeFolder(assetPath);
            string targetDir = Path.Combine(destinationRoot, typeFolder).Replace("\\", "/");

            if (!AssetDatabase.IsValidFolder(targetDir))
            {
                string parent = Path.GetDirectoryName(targetDir).Replace("\\", "/");
                string folderName = Path.GetFileName(targetDir);
                if (!preview) AssetDatabase.CreateFolder(parent, folderName);
            }

            string fileName = Path.GetFileName(assetPath);
            string newPath = Path.Combine(targetDir, fileName).Replace("\\", "/");

            if (preview)
            {
                Debug.Log($"[PREVIEW] Would move {assetPath} → {newPath}");
            }
            else
            {
                AssetDatabase.MoveAsset(assetPath, newPath);
            }
        }

        if (!preview)
        {
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        Debug.Log(preview
            ? $"[PREVIEW MODE] Found {usedLegacyAssets.Count} assets that would be moved."
            : $"Moved and organized {usedLegacyAssets.Count} assets to {destinationRoot}");
    }

    private static IEnumerable<string> ExtractGuidsFromTextFile(string path)
    {
        string text = File.ReadAllText(path);
        var matches = System.Text.RegularExpressions.Regex.Matches(text, @"[0-9a-f]{32}");
        foreach (System.Text.RegularExpressions.Match match in matches)
            yield return match.Value;
    }

    private static string GetTypeFolder(string assetPath)
    {
        string ext = Path.GetExtension(assetPath).ToLower();
        switch (ext)
        {
            case ".fbx":
            case ".obj":
            case ".blend":
                return "Models";
            case ".png":
            case ".jpg":
            case ".jpeg":
            case ".tga":
            case ".psd":
                return "Textures";
            case ".mat":
                return "Materials";
            case ".prefab":
                return "Prefabs";
            case ".anim":
            case ".controller":
                return "Animations";
            case ".asset":
                return "ScriptableObjects";
            default:
                return "Misc";
        }
    }
}
