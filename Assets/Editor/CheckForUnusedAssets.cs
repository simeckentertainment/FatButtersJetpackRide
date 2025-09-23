using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using System.Collections.Generic;
using System.IO;

//Vibe coded. Also not proud.
public class MissingReferenceFinder
{
    [MenuItem("FatButters Tools/Check for Missing References")]
    public static void FindMissingReferences()
    {
        int missingCount = 0;
        List<string> brokenAssets = new List<string>();

        // Check all scenes in the project
        string[] scenePaths = AssetDatabase.FindAssets("t:Scene");
        foreach (string guid in scenePaths)
        {
            string scenePath = AssetDatabase.GUIDToAssetPath(guid);
            Scene scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
            foreach (GameObject root in scene.GetRootGameObjects())
            {
                missingCount += CheckGameObject(root, scenePath, brokenAssets);
            }
        }

        // Check all prefabs in the project
        string[] prefabPaths = AssetDatabase.FindAssets("t:Prefab");
        foreach (string guid in prefabPaths)
        {
            string prefabPath = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
            missingCount += CheckGameObject(prefab, prefabPath, brokenAssets);
        }

        if (missingCount == 0)
        {
            Debug.Log("✅ No missing references found. Safe to delete unused assets.");
        }
        else
        {
            Debug.LogWarning($"⚠ Found {missingCount} missing references in {brokenAssets.Count} assets.");
            foreach (string asset in brokenAssets)
            {
                Debug.LogWarning($"Missing references in: {asset}");
            }
        }
    }

    private static int CheckGameObject(GameObject go, string assetPath, List<string> brokenAssets)
    {
        int count = 0;
        Component[] components = go.GetComponentsInChildren<Component>(true);
        foreach (Component c in components)
        {
            if (!c) // Missing script
            {
                count++;
                if (!brokenAssets.Contains(assetPath)) brokenAssets.Add(assetPath);
                continue;
            }

            SerializedObject so = new SerializedObject(c);
            SerializedProperty sp = so.GetIterator();
            while (sp.NextVisible(true))
            {
                if (sp.propertyType == SerializedPropertyType.ObjectReference)
                {
                    if (sp.objectReferenceValue == null && sp.objectReferenceInstanceIDValue != 0)
                    {
                        count++;
                        if (!brokenAssets.Contains(assetPath)) brokenAssets.Add(assetPath);
                    }
                }
            }
        }
        return count;
    }
}
