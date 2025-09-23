using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class creditsScroller : MonoBehaviour
{
    Vector3 startPos = new Vector3();
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = new Vector3(transform.position.x,transform.position.y+0.02f,transform.position.z);
    }

    public void Reset(){
        gameObject.transform.position = startPos;
    }
}
