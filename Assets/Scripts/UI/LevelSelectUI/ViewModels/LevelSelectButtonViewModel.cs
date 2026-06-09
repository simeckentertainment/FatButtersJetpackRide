using UnityEngine;

public class LevelSelectButtonViewModel : ButtonViewModel<LevelSelectUIModel>
{
    [SerializeField] public LevelButtonIDHolder levelId;
    [SerializeField] public Camera cam;
    [SerializeField] public Vector3 positionOffset;

    private CollectibleData collectibleData => SaveManager.Instance.collectibleData;

    private void Update()
    {
        transform.position = cam.WorldToScreenPoint(levelId.transform.position) + positionOffset;
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