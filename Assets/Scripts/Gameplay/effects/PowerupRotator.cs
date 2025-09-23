using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PowerupRotator : MonoBehaviour
{


    [Header("Powerup bonus amounts are determined by object scale for fuel and treats.\nFor the ball, just check isBall.")]
    [SerializeField] float rotationAmount;
    [System.NonSerialized] public float increaseFuel = 25;
    [System.NonSerialized] public float increaseTreats = 25;
    public bool isBall;
    void Start(){
        try { //Make sure you're calling the scale amount of the parent object.
        increaseFuel *= transform.parent.localScale.x;
        increaseTreats *= transform.parent.localScale.x;
        } catch { }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0,rotationAmount,0));
    }

}
