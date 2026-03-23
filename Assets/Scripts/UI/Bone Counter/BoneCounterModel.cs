using UnityEngine;

public class BoneCounterModel : Model
{
    [Tooltip("Nullable, if there is no player it will just use the save data.")]
    [SerializeField] private Player player;

    private CollectibleData collectibleData => SaveManager.Instance.collectibleData;

    private int _boneCount;
    public int BoneCount
    {
        get
        {
            return _boneCount;
        }
        set
        {
            _boneCount = value;
            Refresh();
        }
    }

    private void Start()
    {
        if (player != null)
        {
            player.OnPickupCollected.AddListener(OnBonesChanged);
        }

        collectibleData.OnBonesChanged.AddListener(OnBonesChanged);

        OnBonesChanged();
    }

    private void OnBonesChanged()
    {
        BoneCount = collectibleData.BONES + (player?.BonesCollected ?? 0);
    }
}
