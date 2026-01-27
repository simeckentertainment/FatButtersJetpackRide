using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhirlyDirly : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private bool primary;

    private void FixedUpdate()
    {
        var angularVeclocity = (primary ? Vector3.back : Vector3.forward) * rotationSpeed * Time.deltaTime;

        transform.Rotate(angularVeclocity);
    }
}
