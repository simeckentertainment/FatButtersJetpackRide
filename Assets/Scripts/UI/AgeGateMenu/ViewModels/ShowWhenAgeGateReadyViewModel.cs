using UnityEngine;

public class ShowWhenAgeGateReadyViewModel : HideableViewModel<AgeGateMenuModel>
{
    [SerializeField] private bool showWhenReady;

    protected override bool IsVisible()
    {
        return Model.ReadyToGo == showWhenReady;
    }
}
