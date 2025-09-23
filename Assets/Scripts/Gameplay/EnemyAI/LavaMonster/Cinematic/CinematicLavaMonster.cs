using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicLavaMonster : MonoBehaviour
{
    public bool triggered;
    [SerializeField] Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        triggered = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(triggered){
            anim.Play("Rise");
            triggered = false;
        }
    }
    void OnTriggerEnter(Collider other){
        if(other.gameObject.tag == "Player"){
            triggered = true;
        }
    }
}
