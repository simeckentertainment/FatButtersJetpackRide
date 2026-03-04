using UnityEngine;
using System.Collections.Generic;

public class CollectibleCounterModel : Model
{
    [SerializeField] private Player player;
    [SerializeField] private bool showCollectionInfoMessages;
    [SerializeField] private List<GameObject> objectsEnabledWhenCompleted;

    [SerializeField] private EditorLocalTransform collectibleArrowTransform;
    [SerializeField] private EditorLocalTransform corgiSenseArrowTransform;

    private int _totalBones;
    public int TotalBones
    {
        get
        {
            return _totalBones;
        }
        set
        {
            _totalBones = value;
            Refresh();
        }
    }

    public int BonesCollected => player.BonesCollected;

    private int _totalFoods;
    public int TotalFoods
    {
        get
        {
            return _totalFoods;
        }
        set
        {
            _totalFoods = value;
            Refresh();
        }
    }

    public int FoodsCollected => player.FoodsCollected;

    private int _totalBalls;
    public int TotalBalls
    {
        get
        {
            return _totalBalls;
        }
        set
        {
            _totalBalls = value;
            Refresh();
        }
    }

    public int BallsCollected => player.BallsCollected;

    private int _totalEnemies;
    public int TotalEnemies
    {
        get
        {
            return _totalEnemies;
        }
        set
        {
            _totalEnemies = value;
            Refresh();
        }
    }

    public int EnemiesDefeated => player.EnemiesDefeated;

    public bool AllCollectiblesCollected =>
        BonesCollected == TotalBones &&
        FoodsCollected == TotalFoods &&
        BallsCollected == TotalBalls &&
        EnemiesDefeated == TotalEnemies;

    protected override void RefreshInternal()
    {
        if (showCollectionInfoMessages && AllCollectiblesCollected)
        {
            CollectiblesCompleted();
        }
    }

    private void Awake()
    {
        player.OnPickupCollected.AddListener(Refresh);
    }

    private void OnDestroy()
    {
        player.OnPickupCollected.RemoveListener(Refresh);
    }

    private void Start()
    {
        TotalBones = CountObj("Bone");
        TotalFoods = CountObj("Food");
        TotalBalls = CountObj("Ball");
        TotalEnemies = CountObj("Harmful");

        foreach (var obj in objectsEnabledWhenCompleted)
        {
            obj.SetActive(false);
        }

        if (showCollectionInfoMessages)
        {
            player.UI.ShowInfoText("Collect!", "Collect everything!", collectibleArrowTransform);
        }
    }

    private void CollectiblesCompleted()
    {
        if (showCollectionInfoMessages)
        {
            player.UI.ShowInfoText("Success!", "Get to the finish!", corgiSenseArrowTransform);
        }

        foreach (var obj in objectsEnabledWhenCompleted)
        {
            obj.SetActive(true);
        }
    }

    private int CountObj(string tag)
    {
        // if we'd like to make this slightly more performant, we will have to maintain that all levels have 
        // a transform where all of the pickups and enemies live under, then we can just check transforms there
        return GameObject.FindGameObjectsWithTag(tag).Length;
    }
}
