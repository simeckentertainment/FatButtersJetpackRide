using UnityEngine;

public class LevelSelectSetStateButtonViewModel : LevelSelectTopButtonViewModel
{
    [SerializeField] private LevelSelectUIState state;

    protected override void OnClick()
    {
        Model.UIState = state;
    }
}
