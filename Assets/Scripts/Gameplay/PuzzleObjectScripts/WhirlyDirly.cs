using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhirlyDirly : MonoBehaviour
{
    [SerializeField] bool primary;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(primary){
        transform.Rotate(Vector3.back*0.2f);
        } else {
            transform.Rotate(Vector3.forward*0.2f);
        }
    }
}
