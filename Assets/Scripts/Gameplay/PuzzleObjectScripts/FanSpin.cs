using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanSpin : MonoBehaviour
{
    [SerializeField] Transform fanObj;
    [SerializeField] ParticleSystem windshaft;
    // Start is called before the first frame update
    void Start()
    {
        ParticleSystem.ShapeModule shape = windshaft.shape;
        shape.radius = transform.localScale.x*shape.radius;

    }

    // Update is called once per frame
    void Update()
    {
        fanObj.Rotate((Vector3.up*2)*(1/transform.localScale.x));
    }
}
