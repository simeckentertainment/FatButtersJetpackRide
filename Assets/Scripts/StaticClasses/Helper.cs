using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Networking;
using System;
using Unity.Mathematics;
public class Helper
{

    public static string AdIDString = "1b33dd9c5";
    public static int InterstitialAdCountThreshold = 2;
    public static bool isWithinMarginOfError(Vector3 currentPos, Vector3 targetPos, float marginAmount)
    {
        bool result;
        Vector3 MaxVal = new Vector3(targetPos.x + marginAmount, targetPos.y + marginAmount, targetPos.z + marginAmount);
        Vector3 MinVal = new Vector3(targetPos.x - marginAmount, targetPos.y - marginAmount, targetPos.z - marginAmount);
        bool[] weGood = { false, false, false }; //assume not good unless proven otherwise.
        if (currentPos.x >= MinVal.x && currentPos.x <= MaxVal.x)
        {
            weGood[0] = true;
        }
        if (currentPos.y >= MinVal.y && currentPos.y <= MaxVal.y)
        {
            weGood[1] = true;
        }
        if (currentPos.z >= MinVal.z && currentPos.z <= MaxVal.z)
        {
            weGood[2] = true;
        }

        if (weGood[0] == true && weGood[1] == true && weGood[2] == true)
        {
            //Debug.Log("we good");
            result = true;
        }
        else
        {
            //Debug.Log("Not there yet");
            result = false;
        }
        return result;
    }


    public static bool isWithinMarginOfError(float current, float target, float marginAmount)
    {
        float maxVal = target + marginAmount;
        float minVal = target - marginAmount;
        bool weGood;
        if (minVal < current && maxVal > current)
        {
            weGood = true;
        }
        else
        {
            weGood = false;
        }
        return weGood;
    }

    public static float RemapArbitraryValues(float a, float b, float c, float d, float x)
    {
        return math.remap(a, b, c, d, x);
    }
    public static float RemapToBetweenZeroAndOne(float a, float b, float x)
    {
        return math.remap(a, b, 0.0f, 1.0f, x);
    }

    public static Vector3 Midpoint(Vector3 start, Vector3 finish)
    {
        return (finish + start) / 2.0f;
    }
    public static Vector2 Midpoint(Vector2 start, Vector2 finish)
    {
        return (finish + start) / 2.0f;
    }
    public static float Midpoint(float start, float finish)
    {
        return (finish + start) / 2.0f;
    }

    public static void LoadToLevel(string levelName)
    {
        SaveManager sM = GameObject.FindAnyObjectByType<SaveManager>();
        SceneLoadData sld = sM.sceneLoadData;
        sld.SceneToLoad = levelName;
        sld.LastLoadedLevelInt = 0;
        sld.LastLoadedLevel = "";
        sM.Save();
        //IronSource.Agent.hideBanner();
        SceneManager.LoadScene(Levels.SceneLoader);

    }
    public static GameObject NabSaveData()
    {
        return MonoBehaviour.FindAnyObjectByType<SaveManager>().gameObject;
    }

    public static GameObject getRandomItemFromArray(GameObject[] a)
    {
        int randnum = UnityEngine.Random.Range(0, a.Length);
        return a[randnum];
    }
    public static Material getRandomItemFromArray(Material[] a)
    {
        int randnum = UnityEngine.Random.Range(0, a.Length);
        return a[randnum];
    }
    public static RuntimeAnimatorController getRandomItemFromArray(RuntimeAnimatorController[] a)
    {
        int randnum = UnityEngine.Random.Range(0, a.Length);
        return a[randnum];
    }

    public static GameObject NabPlayerObj()
    {
        return GameObject.Find("Player");
    }
    public static GameObject NabGameplayCamera()
    {
        return GameObject.Find("virtualCamCam");
    }


    public static void ReassignParentConstraint(GameObject constrainedObject, GameObject newParent)
    {
        if (constrainedObject == null || newParent == null)
        {
            Debug.LogError("ReassignParentConstraint: One or both GameObjects are null.");
            return;
        }
        UnityEngine.Animations.ParentConstraint constraint = constrainedObject.GetComponent<UnityEngine.Animations.ParentConstraint>();
        if (constraint == null)
        {
            Debug.LogError("ReassignParentConstraint: The object does not have a ParentConstraint component.");
            return;
        }
        // Temporarily disable constraint to move manually
        constraint.constraintActive = false;

        // Clear existing sources
        constraint.SetSources(new System.Collections.Generic.List<UnityEngine.Animations.ConstraintSource>());

        // Set object to new parent position/rotation
        constrainedObject.transform.position = newParent.transform.position;
        constrainedObject.transform.rotation = newParent.transform.rotation;

        UnityEngine.Animations.ConstraintSource source = new UnityEngine.Animations.ConstraintSource
        {
            sourceTransform = newParent.transform,
            weight = 1f
        };
        constraint.AddSource(source);

        // Enable constraint
        constraint.constraintActive = true;

        // Lock offsets to maintain the current world transform
        constraint.SetTranslationOffset(0, Vector3.zero);
        constraint.SetRotationOffset(0, Vector3.zero);

        // Recalculate and apply
        constraint.translationAtRest = constrainedObject.transform.localPosition;
        constraint.rotationAtRest = constrainedObject.transform.localRotation.eulerAngles;
    }

    public static void ReassignParentConstraint(GameObject constrainedObject, GameObject parentA, GameObject parentB)
    {
        UnityEngine.Animations.ParentConstraint constraint = constrainedObject.GetComponent<UnityEngine.Animations.ParentConstraint>();
        if (constraint == null || parentA == null || parentB == null)
        {
            UnityEngine.Debug.LogError("Missing required components for constraint reassignment.");
            return;
        }

        // Temporarily disable constraint
        constraint.constraintActive = false;

        // Clear previous sources
        constraint.SetSources(new System.Collections.Generic.List<UnityEngine.Animations.ConstraintSource>());

        // Compute average world position and rotation
        Vector3 avgPosition = (parentA.transform.position + parentB.transform.position) / 2f;
        Quaternion avgRotation = Quaternion.Slerp(parentA.transform.rotation, parentB.transform.rotation, 0.5f);

        // Set object to the average location
        constrainedObject.transform.position = avgPosition;
        constrainedObject.transform.rotation = avgRotation;

        // Add both parents as sources with 0.5 weight
        var sourceA = new UnityEngine.Animations.ConstraintSource
        {
            sourceTransform = parentA.transform,
            weight = 0.5f
        };
        var sourceB = new UnityEngine.Animations.ConstraintSource
        {
            sourceTransform = parentB.transform,
            weight = 0.5f
        };
        constraint.AddSource(sourceA);
        constraint.AddSource(sourceB);

        // Enable the constraint
        constraint.constraintActive = true;

        // Zero offsets to make object follow average position/rotation
        constraint.SetTranslationOffset(0, Vector3.zero);
        constraint.SetRotationOffset(0, Vector3.zero);
        constraint.SetTranslationOffset(1, Vector3.zero);
        constraint.SetRotationOffset(1, Vector3.zero);

        // Recalculate rest poses
        constraint.translationAtRest = constrainedObject.transform.localPosition;
        constraint.rotationAtRest = constrainedObject.transform.localRotation.eulerAngles;
    }

    public static void ReassignPositionConstraint(GameObject constrainedObject, GameObject newParent)
    {
        UnityEngine.Animations.PositionConstraint constraint = constrainedObject.GetComponent<UnityEngine.Animations.PositionConstraint>();
        if (constraint == null || newParent == null)
        {
            UnityEngine.Debug.LogError("Missing required components for position constraint reassignment.");
            return;
        }

        // Disable the constraint temporarily
        constraint.constraintActive = false;

        // Clear previous sources
        constraint.SetSources(new System.Collections.Generic.List<UnityEngine.Animations.ConstraintSource>());

        // Move the constrained object to the parent's position
        constrainedObject.transform.position = newParent.transform.position;

        // Create a new constraint source
        var source = new UnityEngine.Animations.ConstraintSource
        {
            sourceTransform = newParent.transform,
            weight = 1f
        };
        constraint.AddSource(source);

        // Enable the constraint
        constraint.constraintActive = true;

        // Set rest pose (in local space, after constraint is active)
        //constraint.translationAtRest = constrainedObject.transform.localPosition;
        constrainedObject.transform.localPosition = Vector3.zero;
}



}
