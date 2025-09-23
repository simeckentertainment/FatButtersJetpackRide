using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatSailOut : MonoBehaviour
{
    [SerializeField] CollectibleData data;
    [SerializeField] Vector3 OgPos;
    [SerializeField] Vector3 SailedPos;
    int sailTimeCounter;
    [SerializeField] int sailTimeThreshold;
    bool doISail;
    // Start is called before the first frame update
    void Start()
    {
        //The only time the boat sails is when it is appropriate to.
        sailTimeCounter = 0;
        doISail = false;
        if(data.LevelBeaten[4] & !data.LevelBeaten[5]){
            doISail = true;
        }

        if(data.LevelBeaten[5]){
            transform.position = SailedPos;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(doISail & sailTimeCounter < sailTimeThreshold){
            sailTimeCounter++;
            transform.position = Vector3.Lerp(OgPos,SailedPos,Helper.RemapToBetweenZeroAndOne(0,sailTimeThreshold,sailTimeCounter));
        }
    }
}
