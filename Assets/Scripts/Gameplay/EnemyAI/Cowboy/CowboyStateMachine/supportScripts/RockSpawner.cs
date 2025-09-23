using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class RockSpawner : MonoBehaviour
{
    [SerializeField] GameObject RockModel;
    [SerializeField] public GameObject SpawnedRock;
    [SerializeField] public float TossTime;
    [SerializeField] float tossTimeElapsed;

    public bool rockSpawned;
    [SerializeField] Vector3 Slerp1;
    [SerializeField] Vector3 Slerp2;


    void Update(){
    }
    public void ThrowRock(Vector3 playerPos){ //Z will always be zero.
        //Need to re-look at all this math.

        SpawnedRock = Instantiate(RockModel,transform.position,quaternion.identity);
        SpawnedRock.tag = "Harmful";
        CowboyRockHandler rockData = SpawnedRock.GetComponent<CowboyRockHandler>();
        rockData.midpoint = Helper.Midpoint(transform.position,playerPos);
        rockData.Target = playerPos;
        rockData.SpawnPoint = transform.position;
        rockData.dist = Vector3.Distance(transform.position,playerPos);
        rockData.rockSpawner = this;
        rockData.Apexes = rockData.CalculateApexes();
        rockData.lifeSpanCounter = 0;
        rockData.lifetimeMax = TossTime;
    }
}
