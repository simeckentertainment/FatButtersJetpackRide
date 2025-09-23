using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class foodRandomizer : MonoBehaviour
{
    MeshFilter meshFilter;
    MeshRenderer meshRenderer;
    [SerializeField] List<GameObject> objectList;
    GameObject foodData;
    int rng;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        rng = Random.Range(0,objectList.Count);
        foodData = Instantiate(objectList[rng],transform.position, Quaternion.identity);
        foodData.transform.parent = gameObject.transform;
        foodData.tag = "Food";

        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();

        meshFilter.mesh = objectList[rng].gameObject.GetComponent<MeshFilter>().mesh;
        meshRenderer.material = objectList[rng].gameObject.GetComponent<MeshRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
