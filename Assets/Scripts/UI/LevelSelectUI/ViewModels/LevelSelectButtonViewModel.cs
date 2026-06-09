using UnityEngine;

public class LevelSelectButtonViewModel : ButtonViewModel<LevelSelectUIModel>
{
    [SerializeField] public LevelButtonIDHolder levelId;
    [SerializeField] public Camera cam;
    [SerializeField] public Vector3 positionOffset;
    [SerializeField] public float scaleMultiplier = 1;
    [SerializeField] public float scaleDistanceMultiplier = 1;
    [SerializeField] public float scaleDistanceOffset;

    private CollectibleData collectibleData => SaveManager.Instance.collectibleData;

    private void Update()
    {
        transform.position = cam.WorldToScreenPoint(levelId.transform.position) + positionOffset;
        var distanceToCamera = Vector3.Distance(cam.transform.position, levelId.transform.position);
        var inverseDistance = (scaleDistanceMultiplier / distanceToCamera) + scaleDistanceOffset;
        transform.localScale = new Vector3(inverseDistance, inverseDistance, inverseDistance) * scaleMultiplier;
        // small ones need to be bigger, big ones need to be smaller
    }

    protected override bool IsEnabled()
    {
        if (levelId.levelID <= 1)
        {
            return true; // level 1 always unlocked
        }

        return collectibleData.LevelBeaten[levelId.levelID - 1];
    }

    protected override void OnClick()
    {
        Model.GoToLevel(levelId.levelID);
    }
}