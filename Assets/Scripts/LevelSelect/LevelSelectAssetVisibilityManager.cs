using UnityEngine;

public class LevelSelectAssetVisibilityManager : MonoBehaviour
{
    [SerializeField] public GameObject cam;
    [SerializeField] AnimationCurve animCurve;
    [SerializeField] int camMoveCounter;
    [SerializeField] int InitialCameraMoveFrameThreshold;
    [SerializeField] Vector3 InitCamPos;
    [SerializeField] Vector3 FinalCamPos;
    [SerializeField] bool CameraInPosition;
    [SerializeField] int BoneTimerThreshold;
    [SerializeField] int BoneTimerCounter;
    [SerializeField] float BoneTimerPercent;
    [SerializeField] float NextBoneMilestonePercentage;
    [SerializeField] float IndividualBonePercentage;
    [SerializeField] float BonesPercentDone;
    [SerializeField] int firstUnbeatenLevel;
    [SerializeField] MeshRenderer[] PopInBones;

    [SerializeField] LevelSelectUIModel levelSelectUI;

    bool PlayerCanMoveCamera;
    [Header("Dynamic level select obs. 0 is a null placeholder.")] //Js don't work in the header for some reason. Unity bug?
    public Transform[] LevelPathHolders;
    bool pathsDone;
    public GameObject[] LevelButtons;
    bool levelObjsDone;
    [Header("Other effect-based OBs")]
    public GameObject boat;
    [SerializeField] LevelSelectScroller lss;

    private CollectibleData collectibleData => SaveManager.Instance.collectibleData;

    // Start is called before the first frame update
    void Start()
    {
        if (cam == null)
        {
            cam = FindFirstObjectByType<Camera>().gameObject;
        }
        CameraInPosition = false;
        BoneTimerCounter = 0;
        camMoveCounter = 0;
        PlayerCanMoveCamera = false;
        firstUnbeatenLevel = GetIndexForFirstUnbeatenLevel();
        
        if(firstUnbeatenLevel > 3)
        {
            InitCamPos = GetCamPosForLastBeatenLevel();
            FinalCamPos = GetCamPosForFirstUnbeatenLevel();
        }
        else
        {
            InitCamPos = FindFirstObjectByType<LevelSelectScroller>().LeftBound.position;
            FinalCamPos = InitCamPos;
        }
        //set the scrollbar now.
        levelSelectUI.LevelSelectScrollValue = Helper.RemapToBetweenZeroAndOne(lss.LeftBound.position.x, lss.RightBound.position.x, FinalCamPos.x);

        Keyframe[] keys = new Keyframe[2];
        keys[0] = new Keyframe(0f,InitCamPos.x);
        keys[1] = new Keyframe(1f,FinalCamPos.x);
        //keys[0].outTangent = 1f;
        keys[0].inTangent = -1f;
        animCurve = new AnimationCurve(keys);
        cam.transform.position = InitCamPos;
        if(firstUnbeatenLevel > 1)
        {
            PopInBones = LevelPathHolders[firstUnbeatenLevel-1].GetComponentsInChildren<MeshRenderer>();
            SetRegularPathBoneVisibility();
        }
        else
        {
            DisableAllPathBones();
        }
        //Turn off all the dynamics for unbeaten levels.
        //That way I don't have to worry about whether or not I
        //disabled them appropriately in the inspector.
        SetLevelButtonAesthetics();
    }

    void Update()
    {
        if(firstUnbeatenLevel < 1) { return; }
        if(camMoveCounter < InitialCameraMoveFrameThreshold)
        {
            camMoveCounter++;
            float CurrentTimeIndex = (float)camMoveCounter / (float)InitialCameraMoveFrameThreshold;
            float currentCurveValue = animCurve.Evaluate(CurrentTimeIndex);
            cam.transform.position = new Vector3(currentCurveValue,cam.transform.position.y,cam.transform.position.z);
        }
        else
        {
            CameraInPosition = true;
            if (levelSelectUI.UIState == LevelSelectUIState.None)
            {
                // make sure we don't repeatedly set this back to base
                levelSelectUI.UIState = LevelSelectUIState.Base;
            }
        }

        if (CameraInPosition && BoneTimerCounter < BoneTimerThreshold)
        {
            RunBonePopin();
        }
    }

    private void SetLevelButtonAesthetics()
    {
        for (int i = 1; i < LevelButtons.Length; i++){
            LevelButtonIDHolder buttonData = LevelButtons[i].GetComponent<LevelButtonIDHolder>();
            buttonData.SetButtonAesthetics(i, collectibleData.LevelBeaten[i], collectibleData.LevelBeaten[i - 1]);
        }
    }

    void DisableAllPathBones()
    {
        for (int i = 1; i < LevelPathHolders.Length; i++){ //starting at 1 because 0 is always null.
            MeshRenderer[] pathBones = LevelPathHolders[i].GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer pathBone in pathBones){
                pathBone.transform.parent.gameObject.SetActive(false);
            }
        }
    }

    void SetRegularPathBoneVisibility()
    {
        for (int i = 1; i < LevelPathHolders.Length; i++) //starting at 1 because 0 is always null.
        {
            MeshRenderer[] pathBones = LevelPathHolders[i].GetComponentsInChildren<MeshRenderer>();
            if (collectibleData.LevelBeaten[i])
            {
                foreach (MeshRenderer pathBone in pathBones)
                {
                    if(pathBone.transform.parent.parent != LevelPathHolders[firstUnbeatenLevel-1].transform)
                    {
                        pathBone.transform.parent.gameObject.SetActive(true);
                    }
                    else
                    {
                        pathBone.transform.parent.gameObject.SetActive(false);
                    }
                }
            }
            else
            {
                foreach (MeshRenderer pathBone in pathBones){
                    pathBone.transform.parent.gameObject.SetActive(false);
                }
            }
        }
    }

    Vector3 GetCamPosForFirstUnbeatenLevel()
    {
        firstUnbeatenLevel = FindFirstFalseIndex(collectibleData.LevelBeaten);
        return new Vector3(LevelButtons[firstUnbeatenLevel].transform.position.x,cam.transform.position.y,cam.transform.position.z);
    }

    Vector3 GetCamPosForLastBeatenLevel()
    {
        int lastBeatenLevel = firstUnbeatenLevel-1;
        return new Vector3(LevelButtons[lastBeatenLevel].transform.position.x,cam.transform.position.y,cam.transform.position.z);
    }

    int GetIndexForFirstUnbeatenLevel()
    {
        return FindFirstFalseIndex(collectibleData.LevelBeaten);
    }

    int FindFirstFalseIndex(bool[] LevelsComplete)
    {
        for (int i = 0; i< collectibleData.LevelBeaten.Length; i++)
        {
            if(!collectibleData.LevelBeaten[i])
            {
                return i;
            }
        }
        return collectibleData.LevelBeaten.Length-1;
    }

    void RunBonePopin()
    {
        BoneTimerCounter++;
        if(NumberOfBonesVisible() == PopInBones.Length){return;} //If we're done, don't worry about it.
        //Bones pop in at fractional intervals between 0 and BoneTimerThreshold frames, starting from when the camera finishes moving.
        BoneTimerPercent = ((float)BoneTimerCounter) / ((float)BoneTimerThreshold);
        BonesPercentDone = ((float)NumberOfBonesVisible()) / ((float)PopInBones.Length); //may not need this.
        IndividualBonePercentage = 1f/((float)PopInBones.Length);
        NextBoneMilestonePercentage = ((float)NumberOfBonesVisible()+1f) / ((float)PopInBones.Length);
        if(BoneTimerPercent >= NextBoneMilestonePercentage)
        {
            EnableGameObjectAndAllChildren(PopInBones[NumberOfBonesVisible()].transform.parent.gameObject);
            NextBoneMilestonePercentage += IndividualBonePercentage;
        }
    }

    private void EnableGameObjectAndAllChildren(GameObject obj)
    {
        // Enable the GameObject
        obj.SetActive(true);
        // Recursively enable all children
        foreach (Transform child in obj.transform)
        {
            EnableGameObjectAndAllChildren(child.gameObject);
        }
    }

    int NumberOfBonesVisible()
    {
        int visibleBoneCounter = 0;
        foreach (MeshRenderer bone in PopInBones)
        {
            if(bone.transform.parent.gameObject.activeSelf)
            {
                visibleBoneCounter++;
            }
        }
        return visibleBoneCounter;
    }
}
