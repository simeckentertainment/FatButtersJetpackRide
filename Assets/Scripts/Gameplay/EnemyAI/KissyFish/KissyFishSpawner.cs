using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KissyFishSpawner : MonoBehaviour
{
    [SerializeField] GameObject fish;
    [SerializeField] public Transform spawnPoint;
    [SerializeField] public Transform spawnWidthBox;
    [SerializeField] public Transform spawnPowerBox;
    //[SerializeField] GameObject waterSurface;
    float spawnWidth;
    float spawnPower;
    bool spawnFrame;
    [SerializeField] public int spawnChance;
    [SerializeField] public int maxSpawnedFish;
    Vector3 currentSpawnPoint;
    GameObject thisFish;

    List<KissyFish> spawnedFish;
    // Start is called before the first frame update
    void Start()
    {
        spawnedFish = new List<KissyFish>();
    }

    // Update is called once per frame
    void Update()
    {
        for(int i=spawnedFish.Count-1;i>=0;i--){
            if(spawnedFish[i].touchedWater){
                Destroy(spawnedFish[i].gameObject);
                spawnedFish.RemoveAt(i);
            }
        }
        if(DoWeSpawnThisFrame()){
            if(spawnedFish.Count >= maxSpawnedFish){
                try{
                Destroy(spawnedFish[0].gameObject);
                spawnedFish.RemoveAt(0);
                } catch { }
            }
            spawnFish();
        }

        

    }


    void spawnFish(){ //stands in for kissyFish's Start() functionality, outside of statemachine setup.
        //calculating unique data.
        spawnWidth = Vector3.Distance(spawnPoint.position,spawnWidthBox.position);
        spawnPower = Vector3.Distance(spawnPoint.position,spawnPowerBox.position);
        float modifier = Random.Range(spawnPoint.position.x-spawnWidth,spawnPoint.position.x+spawnWidth);
        //Instantiating the fish.
        thisFish = Instantiate(fish,spawnPoint);
        //populating data that is consistent upon all fish.
        KissyFish kf = thisFish.GetComponent<KissyFish>();
        spawnedFish.Add(kf);
        //kf.waterCollider = waterSurface.GetComponent<Collider>();
        //populating unique data.
        thisFish.transform.position = new Vector3(modifier,spawnPoint.transform.position.y,spawnPoint.position.z);
        kf.ApexTargetCoords = new Vector3(spawnPowerBox.position.x+modifier,spawnPowerBox.position.y,spawnPowerBox.position.z);
        //be free, my pretties!


        
    }

    bool DoWeSpawnThisFrame(){
        if(spawnPoint.localPosition == Vector3.zero & spawnWidthBox.localPosition == Vector3.zero & spawnPowerBox.localPosition == Vector3.zero){
            return false;
        }

        if(Random.Range(0,1000) < spawnChance){
            return true;
        } else {
            return false;
        }
    }
}
