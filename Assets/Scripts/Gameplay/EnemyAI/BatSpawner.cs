using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatSpawner : MonoBehaviour
{

    [SerializeField] GameObject bat;
    [SerializeField] Transform SpawnLocation;
    [SerializeField] Transform EndLocation;
    [SerializeField]public Transform UpperOffset;
    [SerializeField]public Transform LowerOffset;
    [SerializeField]public Transform[] FlightPath;
    [SerializeField] Collider ActivationTrigger;
    [SerializeField] public int spawnLength;
    [SerializeField] public int spawnAmount;
    [SerializeField] bool triggered;
    int spawnTimer;
    [SerializeField] public int BatSpeed;
    // Start is called before the first frame update
    void Start()
    {
        triggered = false;
        spawnTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(triggered && spawnTimer < spawnLength){
            float offset = BatOffset();
            Vector3 spawnLoc = new Vector3(SpawnLocation.position.x,SpawnLocation.position.y+offset,SpawnLocation.position.z);
            Vector3 endLoc = new Vector3(EndLocation.position.x,EndLocation.position.y+offset,EndLocation.position.z);
            GameObject spawnedBat = Instantiate(bat,spawnLoc,Quaternion.identity);
            spawnedBat.GetComponent<BatRunner>().path = GetPath(offset);
            spawnedBat.GetComponent<BatRunner>().endPoint = endLoc;
            spawnedBat.GetComponent<BatRunner>().timerEnd = BatSpeed;
            spawnedBat.GetComponent<BatRunner>().screech.clip = spawnedBat.GetComponent<BatRunner>().BatSounds[Random.Range(0,2)];
            spawnedBat.GetComponent<BatRunner>().screech.time = Random.Range(0.0f,spawnedBat.GetComponent<BatRunner>().screech.clip.length);
            spawnedBat.GetComponent<BatRunner>().screech.Play();
            spawnTimer +=1;
        }
        if(!triggered) spawnTimer = 0;
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Player") {
            triggered = true;
        }
    }
    float BatOffset(){
        return Random.Range(Vector3.Distance(SpawnLocation.position,LowerOffset.position)*-1,Vector3.Distance(SpawnLocation.position,UpperOffset.position));
    }
    List<Vector3> GetPath(float offset){
        List<Vector3> path = new List<Vector3>();
        foreach (Transform pathPoint in FlightPath){
            path.Add(new Vector3(pathPoint.position.x,pathPoint.position.y+offset,pathPoint.position.z));
        }
        return path;
    }
}


