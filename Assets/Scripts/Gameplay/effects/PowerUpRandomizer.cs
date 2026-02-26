using System.Collections.Generic;
using UnityEngine;

public class PowerUpRandomizer : MonoBehaviour
{
    [SerializeField] private List<GameObject> objectList;
    [SerializeField] private GameObject standInModel;

    private void Start()
    {
        Destroy(standInModel);
        var rng = Random.Range(0,objectList.Count);
        var replacementModel = Instantiate(objectList[rng],transform.position, Quaternion.identity);
        replacementModel.transform.parent = gameObject.transform;
    }
}
