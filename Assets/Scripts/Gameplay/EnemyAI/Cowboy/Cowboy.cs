using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cowboy : MonoBehaviour
{
    public CowboyStateMachine stateMachine;
    public CowboyChaseState cowboyChaseState { get; set;}
    public CowboyIdleState cowboyIdleState { get; set; }
    public CowboyThrowRockState cowboyThrowRockState {get; set;}
    public CowboyUpDieState cowboyUpDieState {get; set;}
    public CowboyDownDieState cowboyDownDieState {get; set;}
    [SerializeField] public Animator anim;
    public GameObject player;
    public float playerDist;
    [SerializeField] public float runSpeed;
    [SerializeField] public float maxThrowDistance;
    [SerializeField] public float aiTriggerDistance;
    [SerializeField] public RockSpawner rockSpawner;
    public PlayerDirection playerDirection;
    public float moveSpeed;
    public bool hitTop;
    public bool hitBot;
    [SerializeField] public CapsuleCollider cowboyCollider;
    public bool isDead;
    


    // Start is called before the first frame update
    void Start()
    {
        isDead = false;
        hitTop = false;
        hitBot = false;
        player = GameObject.Find("Player");
        playerDist = Vector3.Distance(transform.position,player.transform.position);
        cowboyChaseState = new CowboyChaseState(this, stateMachine);
        cowboyIdleState = new CowboyIdleState(this, stateMachine);
        cowboyThrowRockState = new CowboyThrowRockState(this, stateMachine);
        cowboyUpDieState = new CowboyUpDieState(this, stateMachine);
        cowboyDownDieState = new CowboyDownDieState(this, stateMachine);
        stateMachine.Initialize(cowboyIdleState);
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnCollisionEnter(Collision other) {
        if(other.GetContact(0).point.y >= transform.position.y){
            hitTop = true;
        } else {
            hitBot = true;
        }
    }

    public enum PlayerDirection{Left,Right};
}

