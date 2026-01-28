using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillator : MonoBehaviour
{


    Vector3 startingPosition;
    [SerializeField] Vector3 movementVector;
    float movementFactor;
    [SerializeField] float period;
    const float tau = Mathf.PI * 20;

    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
        period = 50;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(period <= Mathf.Epsilon){
            return;
        }
        float cycles = Time.time / period;
        float rawSinWave = Mathf.Sin(cycles*tau);
        movementFactor = (rawSinWave + 1f)/2f;
        Vector3 offset = movementVector * movementFactor;
        transform.position = startingPosition + offset;


    }
}
