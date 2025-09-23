using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotDroneGuard: MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public bool triggered;
    public GameObject player;
    public float speed = 5.0f;
    DataHandler dataHandler;
    void Start()
    {
        dataHandler = FindAnyObjectByType<DataHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        if(triggered && player  != null && !dataHandler.isDead){
             Vector3 direction = player.transform.position - transform.position;
             direction.Normalize();
             transform.position += direction * speed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Player"){
            player = other.gameObject;
            triggered = true;
        }
    }
}
