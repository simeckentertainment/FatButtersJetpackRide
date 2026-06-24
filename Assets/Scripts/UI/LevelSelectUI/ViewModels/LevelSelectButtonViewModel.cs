using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelSelectButtonViewModel : ButtonViewModel<LevelSelectUIModel>, ISelectHandler
{
    [SerializeField] public LevelButtonIDHolder levelId;
    [SerializeField] public Camera cam;
    [SerializeField] public Vector3 positionOffset;
    [SerializeField] public float scaleMultiplier = 1;
    [SerializeField] public float scaleDistanceMultiplier = 1;
    [SerializeField] public float scaleDistanceOffset;
    [SerializeField] public Selectable upSelect;
    [SerializeField] public Selectable downSelect;

    private CollectibleData collectibleData => SaveManager.Instance.collectibleData;

    private void Start()
    {
        var newNavigation = new Navigation();
        newNavigation.mode = Navigation.Mode.Explicit;

        var nextLevel = Model.GetLevelSelectButtonViewModelWithId(levelId.levelID + 1);
        var prevLevel = Model.GetLevelSelectButtonViewModelWithId(levelId.levelID - 1);
        newNavigation.selectOnRight = nextLevel?.GetButton();
        newNavigation.selectOnLeft = prevLevel?.GetButton();
        newNavigation.selectOnUp = upSelect;
        newNavigation.selectOnDown = downSelect;

        Button.navigation = newNavigation;
    }

    protected override void Update()
    {
        base.Update();

        transform.position = cam.WorldToScreenPoint(levelId.transform.position) + positionOffset;

        Plane plane = new Plane();
        plane.SetNormalAndPosition(cam.transform.forward, cam.transform.position);

        var distanceToCamera = plane.GetDistanceToPoint(levelId.transform.position);
        var inverseDistance = (scaleDistanceMultiplier / distanceToCamera) + scaleDistanceOffset;
        transform.localScale = new Vector3(inverseDistance, inverseDistance, inverseDistance) * scaleMultiplier;
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

    public void OnSelect(BaseEventData eventData)
    {
        if (eventData is AxisEventData)
        {
            // this will scroll only when using the joystick or keyboard to select the level
            // it will NOT scroll when selecting the level with a mouse click or screen touch
            Model.ScrollToLevel(levelId);
        }
    }
}