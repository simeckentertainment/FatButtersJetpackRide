using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class PivotEditor
{
    private static bool editingPivot = false;
    private static GameObject pivotHandle;
    private static GameObject targetObject;

    static PivotEditor()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }

    private static void OnSceneGUI(SceneView sceneView)
    {
        Event e = Event.current;

        // Toggle pivot editing with Insert key
        if (e.type == EventType.KeyDown && e.keyCode == KeyCode.Insert)
        {
            TogglePivotEdit();
            e.Use();
        }

        if (editingPivot && pivotHandle != null && targetObject != null)
        {
            EditorGUI.BeginChangeCheck();

            // Draw Move Handle at pivotHandle position
            Vector3 newPos = Handles.PositionHandle(pivotHandle.transform.position, targetObject.transform.rotation);

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(pivotHandle.transform, "Move Pivot");
                pivotHandle.transform.position = newPos;

                // Move mesh relative to pivot so it visually stays in place
                foreach (Transform child in pivotHandle.transform)
                {
                    child.position -= (newPos - pivotHandle.transform.position);
                }
            }
        }
    }

    private static void TogglePivotEdit()
    {
        if (!editingPivot)
        {
            // Enter pivot editing
            if (Selection.activeGameObject != null)
            {
                targetObject = Selection.activeGameObject;

                // Create pivot parent if not exists
                if (targetObject.transform.parent == null || targetObject.transform.parent.name != "_Pivot")
                {
                    pivotHandle = new GameObject("_Pivot");
                    Undo.RegisterCreatedObjectUndo(pivotHandle, "Create Pivot");
                    pivotHandle.transform.position = targetObject.transform.position;
                    targetObject.transform.SetParent(pivotHandle.transform, true);
                }
                else
                {
                    pivotHandle = targetObject.transform.parent.gameObject;
                }

                editingPivot = true;
            }
        }
        else
        {
            // Exit pivot editing
            editingPivot = false;
            targetObject = null;
            pivotHandle = null;
        }

        SceneView.RepaintAll();
    }
}
