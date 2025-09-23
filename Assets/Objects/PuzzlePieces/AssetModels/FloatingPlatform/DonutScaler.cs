using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DonutScaler : MonoBehaviour
{
    [SerializeField] Transform scalarTrans;
    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = scalarTrans.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
