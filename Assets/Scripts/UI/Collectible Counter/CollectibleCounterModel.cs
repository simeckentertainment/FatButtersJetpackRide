using UnityEngine;

public class CollectibleCounterModel : Model
{
    [SerializeField] private Player player;

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
    }

    private int CountObj(string tag)
    {
        // if we'd like to make this slightly more performant, we will have to maintain that all levels have 
        // a transform where all of the pickups and enemies live under, then we can just check transforms there
        return GameObject.FindGameObjectsWithTag(tag).Length;
    }
}
