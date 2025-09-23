using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelMachine : MonoBehaviour
{

    [SerializeField] public WheelCollider[] wheelColliders;
    [SerializeField]public Transform[] wheelModels;
    [SerializeField]public AngleMachine gyro;
    public float compensationRotation;
    public float rotateAdditive;
    float trueRotation;
    // Start is called before the first frame update
    void Start()
    {
        compensationRotation = 0.0f;
        rotateAdditive = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        trueRotation = compensationRotation+rotateAdditive;
        float visualWheelRotation = trueRotation * Time.deltaTime;
        for(int i=0;i<wheelColliders.Length;i++){
            wheelColliders[i].rotationSpeed = trueRotation;
            wheelModels[i].transform.Rotate(Vector3.right*Time.deltaTime*trueRotation);
        }
    }
}
