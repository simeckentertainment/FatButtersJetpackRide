using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class RotationToAnimClipWindow : EditorWindow
{
    private AnimationClip animationClip;
    private List<Transform> transforms = new List<Transform>();
    private Vector2 scrollPos;

    // Used to show drag area for multiple Transforms
    private Object[] dragObjects = new Object[0];

    [MenuItem("FatButters Tools/Add Rotations to Anim Clip")]
    public static void ShowWindow()
    {
        var window = GetWindow<RotationToAnimClipWindow>("Add Rotations");
        window.minSize = new Vector2(400, 350);
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Transforms to Animate", EditorStyles.boldLabel);
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Height(150));

        for (int i = 0; i < transforms.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            transforms[i] = (Transform)EditorGUILayout.ObjectField(transforms[i], typeof(Transform), true);

            if (GUILayout.Button("X", GUILayout.Width(20)))
            {
                transforms.RemoveAt(i);
                i--;
            }
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.EndScrollView();

        if (GUILayout.Button("Add Empty Slot"))
        {
            transforms.Add(null);
        }

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Bulk Add Transforms", EditorStyles.boldLabel);

EditorGUILayout.Space(10);
EditorGUILayout.LabelField("Bulk Add Transforms (Drag & Drop)", EditorStyles.boldLabel);

Rect dropArea = GUILayoutUtility.GetRect(0, 50, GUILayout.ExpandWidth(true));
GUI.Box(dropArea, "Drag GameObjects or Transforms here", EditorStyles.helpBox);

Event evt = Event.current;
if (evt.type == EventType.DragUpdated || evt.type == EventType.DragPerform)
{
    if (dropArea.Contains(evt.mousePosition))
    {
        DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

        if (evt.type == EventType.DragPerform)
        {
            DragAndDrop.AcceptDrag();

            foreach (var obj in DragAndDrop.objectReferences)
            {
                if (obj is GameObject go)
                {
                    var tf = go.transform;
                    if (!transforms.Contains(tf))
                        transforms.Add(tf);
                }
                else if (obj is Transform tf)
                {
                    if (!transforms.Contains(tf))
                        transforms.Add(tf);
                }
            }

            evt.Use();
        }
    }
}

        if (EditorGUI.EndChangeCheck() && dragObjects != null)
        {
            foreach (var obj in dragObjects)
            {
                if (obj is GameObject go)
                {
                    var tf = go.transform;
                    if (!transforms.Contains(tf))
                        transforms.Add(tf);
                }
                else if (obj is Transform tf)
                {
                    if (!transforms.Contains(tf))
                        transforms.Add(tf);
                }
            }
            dragObjects = new Object[0]; // Clear after adding
        }

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Animation Clip", EditorStyles.boldLabel);
        animationClip = (AnimationClip)EditorGUILayout.ObjectField(animationClip, typeof(AnimationClip), false);

        EditorGUILayout.Space();

        GUI.enabled = animationClip != null && transforms.Count > 0;
        if (GUILayout.Button("Add"))
        {
            AddRotationsToClip();
        }
        GUI.enabled = true;
    }

    private void AddRotationsToClip()
    {
        Undo.RecordObject(animationClip, "Add Rotations");

        foreach (var t in transforms)
        {
            if (t == null) continue;

            string relativePath = GetRelativePath(t);
            Quaternion rot = t.localRotation;

            AnimationUtility.SetEditorCurve(animationClip, EditorCurveBinding.FloatCurve(relativePath, typeof(Transform), "rotation.x"), CreateCurve(rot.x));
            AnimationUtility.SetEditorCurve(animationClip, EditorCurveBinding.FloatCurve(relativePath, typeof(Transform), "rotation.y"), CreateCurve(rot.y));
            AnimationUtility.SetEditorCurve(animationClip, EditorCurveBinding.FloatCurve(relativePath, typeof(Transform), "rotation.z"), CreateCurve(rot.z));
            AnimationUtility.SetEditorCurve(animationClip, EditorCurveBinding.FloatCurve(relativePath, typeof(Transform), "rotation.w"), CreateCurve(rot.w));
        }

        EditorUtility.SetDirty(animationClip);
        AssetDatabase.SaveAssets();
        Debug.Log("Rotations added to animation clip.");
    }

    private AnimationCurve CreateCurve(float value)
    {
        return new AnimationCurve(new Keyframe(0f, value));
    }

    private string GetRelativePath(Transform transform)
    {
        string path = transform.name;
        Transform current = transform;

        while (current.parent != null && current.parent.GetComponent<Animator>() == null)
        {
            current = current.parent;
            path = current.name + "/" + path;
        }

        return path;
    }
}
