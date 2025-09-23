using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectPathBone : MonoBehaviour
{
    [SerializeField] ParticleSystem poof;
    // Start is called before the first frame update
    void Start()
    {
        poof.Play();
    }
    void OnEnable(){
        poof.Play();
    }
}
