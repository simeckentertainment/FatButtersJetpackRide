using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Animations;

public class GameOnDemoCollectibleCounter : MonoBehaviour
{
    [SerializeField] Player player;
    public int bonesRemaining;
    public int foodsRemaining;
    public int ballsRemaining;
    public int enemiesRemaining;
    [SerializeField] Transform collectibleContainer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bonesRemaining = CountObj("Bone");
        foodsRemaining = CountObj("Food");
        ballsRemaining = CountObj("Ball");
        enemiesRemaining = CountObj("Harmful");
    }

    // Update is called once per frame
    void Update() //Using Update because FixedUpdate is misssing frames.
    {
        if (player.BoneTouch){
            bonesRemaining = CountObj("Bone");
        }
        if (player.BallTouch){
            ballsRemaining = CountObj("Food");
        }
        if (player.FoodTouch){
            foodsRemaining = CountObj("Ball");
        }
        if (player.HarmfulTouch){
            enemiesRemaining = CountObj("Harmful");
        }
    }

    int CountObj(string tag) {

        List<Transform> targetTransforms = new List<Transform>();
        Transform[] allTransforms = collectibleContainer.GetComponentsInChildren<Transform>(true);
        Debug.Log(allTransforms.Length);
        foreach (Transform child in allTransforms)
        {
            if (child.CompareTag(tag))
            {
                targetTransforms.Add(child);
            }
        }

        return targetTransforms.Count;
    }
    
}
