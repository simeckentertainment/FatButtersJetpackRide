using UnityEngine;
using UnityEditor;

/// <summary>
/// FatButters Tools → Create Story Objects: one-click GameObjects for the story event system.
/// </summary>
public static class CreateStoryObjectsMenu
{
    const string MenuRoot = "FatButters Tools/Create Story Objects/";

    static GameObject CreateChild(string objectName, string undoLabel)
    {
        var go = new GameObject(objectName);
        Undo.RegisterCreatedObjectUndo(go, undoLabel);
        if (Selection.activeTransform != null)
            Undo.SetTransformParent(go.transform, Selection.activeTransform, "Parent " + objectName);
        Selection.activeGameObject = go;
        return go;
    }

    [MenuItem(MenuRoot + "Sequence Controller", priority = 1)]
    static void CreateSequenceController()
    {
        var go = CreateChild("StorySequenceController", "Create Story Sequence Controller");
        Undo.AddComponent<LevelStorySequenceController>(go);
    }

    [MenuItem(MenuRoot + "Story Gameplay Bridge", priority = 2)]
    static void CreateStoryGameplayBridge()
    {
        var go = CreateChild("StoryGameplayBridge", "Create Story Gameplay Bridge");
        Undo.AddComponent<StoryGameplayBridge>(go);
    }

    [MenuItem(MenuRoot + "Zone Entry Trigger", priority = 3)]
    static void CreateZoneEntryTrigger()
    {
        var go = CreateChild("StoryZoneEntryTrigger", "Create Zone Entry Trigger");
        Undo.AddComponent<EnterZoneTrigger>(go);
        var box = Undo.AddComponent<BoxCollider>(go);
        Undo.RecordObject(box, "Configure zone trigger collider");
        box.isTrigger = true;
        EditorUtility.SetDirty(box);
    }

    [MenuItem(MenuRoot + "Timer Trigger", priority = 4)]
    static void CreateTimerTrigger()
    {
        var go = CreateChild("StoryTimerTrigger", "Create Timer Trigger");
        Undo.AddComponent<TimerTrigger>(go);
    }

    [MenuItem(MenuRoot + "Manual Trigger", priority = 5)]
    static void CreateManualTrigger()
    {
        var go = CreateChild("StoryManualTrigger", "Create Manual Trigger");
        Undo.AddComponent<ManualTrigger>(go);
    }
}
