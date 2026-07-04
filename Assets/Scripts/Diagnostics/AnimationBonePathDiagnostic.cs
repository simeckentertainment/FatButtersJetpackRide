using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

/// <summary>
/// Attach this to the GameObject that has the Animator component.
/// It will log the bone paths that animation clips expect vs the actual hierarchy.
/// </summary>
public class AnimationBonePathDiagnostic : MonoBehaviour
{
    [Header("Drag the Animator Controller here")]
    public RuntimeAnimatorController animatorController;

    [ContextMenu("Log Animation Clip Bone Paths")]
    public void LogAnimationClipBonePaths()
    {
        if (animatorController == null)
        {
            Debug.LogError("Animator Controller is not assigned!");
            return;
        }

        Debug.Log("===== ANIMATION CLIP BONE PATH DIAGNOSTIC =====");
        Debug.Log($"GameObject: {gameObject.name}");
        Debug.Log($"Animator Controller: {animatorController.name}");
        Debug.Log("");

        // Get all animation clips from the controller
        AnimationClip[] clips = animatorController.animationClips;
        Debug.Log($"Found {clips.Length} animation clips in the controller.");
        Debug.Log("");

        foreach (AnimationClip clip in clips)
        {
            Debug.Log($"--- Clip: {clip.name} ---");
            Debug.Log($"  Length: {clip.length} seconds");
            Debug.Log($"  Frame Rate: {clip.frameRate}");
            Debug.Log($"  Is Loopable: {clip.isLooping}");
            Debug.Log("");

            // Get all curves (bone paths) in this clip
            EditorCurveBinding[] curveBindings = AnimationUtility.GetCurveBindings(clip);
            Debug.Log($"  Number of animated properties: {curveBindings.Length}");
            Debug.Log("");

            if (curveBindings.Length == 0)
            {
                Debug.LogWarning($"  ⚠ No curves found in this clip! It won't animate anything.");
                Debug.Log("");
                continue;
            }

            // Group by bone path
            Dictionary<string, List<string>> boneProperties = new Dictionary<string, List<string>>();
            foreach (EditorCurveBinding binding in curveBindings)
            {
                if (!boneProperties.ContainsKey(binding.path))
                {
                    boneProperties[binding.path] = new List<string>();
                }
                boneProperties[binding.path].Add($"{binding.propertyName} ({binding.type.Name})");
            }

            Debug.Log($"  Bone paths expected by this clip:");
            foreach (var kvp in boneProperties)
            {
                string path = string.IsNullOrEmpty(kvp.Key) ? "[Root GameObject]" : kvp.Key;
                Debug.Log($"    📍 Path: \"{path}\"");
                foreach (string prop in kvp.Value)
                {
                    Debug.Log($"       - {prop}");
                }
            }
            Debug.Log("");
        }

        // Now log the actual bone hierarchy
        Debug.Log("===== ACTUAL BONE HIERARCHY =====");
        LogBoneHierarchy(transform, "");
        Debug.Log("");

        // Compare
        Debug.Log("===== COMPARISON =====");
        CompareClipPathsToHierarchy(clips, transform);
        Debug.Log("===== DIAGNOSTIC COMPLETE =====");
    }

    private void LogBoneHierarchy(Transform t, string indent)
    {
        Debug.Log($"  {indent}📁 {t.name}");
        foreach (Transform child in t)
        {
            LogBoneHierarchy(child, indent + "  ");
        }
    }

    private void CompareClipPathsToHierarchy(AnimationClip[] clips, Transform root)
    {
        // Collect all actual bone paths from the hierarchy
        HashSet<string> actualPaths = new HashSet<string>();
        CollectBonePaths(root, "", actualPaths);

        // Collect all unique paths expected by clips
        HashSet<string> clipPaths = new HashSet<string>();
        foreach (AnimationClip clip in clips)
        {
            EditorCurveBinding[] curveBindings = AnimationUtility.GetCurveBindings(clip);
            foreach (EditorCurveBinding binding in curveBindings)
            {
                if (!string.IsNullOrEmpty(binding.path))
                {
                    clipPaths.Add(binding.path);
                }
            }
        }

        Debug.Log($"  Actual bone paths in hierarchy: {actualPaths.Count}");
        Debug.Log($"  Bone paths expected by clips: {clipPaths.Count}");
        Debug.Log("");

        // Find mismatches
        foreach (string clipPath in clipPaths)
        {
            if (!actualPaths.Contains(clipPath))
            {
                Debug.LogWarning($"  ❌ Clip expects path \"{clipPath}\" but it was NOT found in the hierarchy!");

                // Try to find a close match
                string closestMatch = FindClosestMatch(clipPath, actualPaths);
                if (closestMatch != null)
                {
                    Debug.Log($"     → Did you mean: \"{closestMatch}\"?");
                }
            }
            else
            {
                Debug.Log($"  ✅ Path \"{clipPath}\" found in hierarchy.");
            }
        }
    }

    private void CollectBonePaths(Transform t, string currentPath, HashSet<string> paths)
    {
        string path = string.IsNullOrEmpty(currentPath) ? t.name : currentPath + "/" + t.name;
        paths.Add(path);
        foreach (Transform child in t)
        {
            CollectBonePaths(child, path, paths);
        }
    }

    private string FindClosestMatch(string clipPath, HashSet<string> actualPaths)
    {
        // Try to find a match ignoring the namespace prefix (e.g., "ScreamBubble:" prefix)
        string clipPathTrimmed = clipPath;
        int colonIndex = clipPath.IndexOf(':');
        if (colonIndex >= 0)
        {
            clipPathTrimmed = clipPath.Substring(colonIndex + 1);
        }

        foreach (string actualPath in actualPaths)
        {
            // Check if the actual path ends with the clip path (ignoring prefix)
            if (actualPath.EndsWith(clipPath) || actualPath.EndsWith(clipPathTrimmed))
            {
                return actualPath;
            }

            // Check if the clip path ends with the actual path
            if (clipPath.EndsWith(actualPath))
            {
                return actualPath;
            }
        }

        return null;
    }
}
