using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class tutorialObjectRotator : MonoBehaviour
{
    [SerializeField] int current;
    [SerializeField] int timer;
    int timerThreshold = 100;
    [SerializeField] Texture2D[] images;
    // Start is called before the first frame update
    void Start()
    {
        current = 0;
        timer = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timer++;
        if(timer>timerThreshold){
            Flip();
        }
    }

    void Flip(){
        current++;
        timer=0;
        if(current >= images.Length){
            current = 0;
        }
        GetComponent<MeshRenderer>().material.SetTexture("_BaseMap",images[current]);
    }
}
