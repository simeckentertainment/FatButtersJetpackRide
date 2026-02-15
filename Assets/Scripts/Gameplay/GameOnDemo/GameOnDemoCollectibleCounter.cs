using System.Linq;
using UnityEngine;

public class GameOnDemoCollectibleCounter : MonoBehaviour
{
    [SerializeField] Player player;
    public int bonesRemaining;
    public int foodsRemaining;
    public int ballsRemaining;
    public int enemiesRemaining;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bonesRemaining = CountObj("Bone");
        bonesRemaining = CountObj("Food");
        ballsRemaining = CountObj("Ball");
        enemiesRemaining = CountObj("Harmful");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (player.BoneTouch){
            bonesRemaining = CountObj("Bone");
        }
        if (player.BallTouch){
            bonesRemaining = CountObj("Food");
        }
        if (player.FoodTouch){
            ballsRemaining = CountObj("Ball");
        }
        if (player.HarmfulTouch){
            enemiesRemaining = CountObj("Harmful");
        }
    }

    int CountObj(string tag) {
        return GameObject.FindGameObjectsWithTag(tag).Length;
    }
    
}
