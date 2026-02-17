using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpRandomizer : MonoBehaviour
{
    [SerializeField] List<GameObject> objectList;
    [SerializeField] GameObject standInModel;
    int rng;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(standInModel);
        rng = Random.Range(0,objectList.Count);
        GameObject replacementModel = Instantiate(objectList[rng],transform.position, Quaternion.identity);
        replacementModel.transform.parent = gameObject.transform;
    }
}
