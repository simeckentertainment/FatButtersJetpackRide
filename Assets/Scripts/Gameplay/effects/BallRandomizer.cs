using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallRandomizer : MonoBehaviour
{
    [SerializeField] GameObject StandinBall;
    [SerializeField] List<GameObject> objectList;
    [SerializeField] GameObject BallData;
    [SerializeField] int rng;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(StandinBall);
        rng = Random.Range(0,objectList.Count);
        BallData = Instantiate(objectList[rng],transform.position, Quaternion.identity);
        BallData.transform.parent = gameObject.transform;
        BallData.tag = "Ball";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
