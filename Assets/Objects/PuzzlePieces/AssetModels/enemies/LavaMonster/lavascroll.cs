using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lavascroll : MonoBehaviour
{
    public Vector2 scrollSpeed;
    private Renderer rend;
    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Set the offset in the material properties
        rend.material.mainTextureOffset += scrollSpeed;
    }
}
