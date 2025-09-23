using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Furret : MonoBehaviour
{
    public FurretStateMachine stateMachine;
    public FurretNullState furretNullState { get; set; }
    public FurretNosePokerIdleState furretNosePokerIdleState { get; set; }
    public FurretJumperIdleState furretJumperIdleState { get; set; }
    public FurretJumperSpookedState furretJumperSpookedState { get; set; }
    [SerializeField] public Animator anim;
    public GameObject player;
    public FurretType furretType;
    
    [Header("Nose Poke stuff")]
    public bool nosePokeTriggered;
    


    // Start is called before the first frame update
    void Start()
    {
        furretNullState = new FurretNullState(this, stateMachine);
        DetermineStartState();

    }
    void DetermineStartState(){
        switch(furretType){
            case FurretType.NosePoker:
                furretNosePokerIdleState = new FurretNosePokerIdleState(this, stateMachine);
                stateMachine.Initialize(furretNosePokerIdleState);
                break;
            case FurretType.Jumper:
                furretJumperIdleState = new FurretJumperIdleState(this, stateMachine);
                furretJumperSpookedState = new FurretJumperSpookedState(this, stateMachine);
                transform.localScale = new Vector3(0.5f,0.5f,0.5f);
                stateMachine.Initialize(furretJumperIdleState);
             break;
            case FurretType.Boss:
                transform.localScale = new Vector3(5f,5f,5f);
                stateMachine.Initialize(furretNullState); //Temporary
                break;
            default:
                furretNullState = new FurretNullState(this, stateMachine);
                stateMachine.Initialize(furretNullState); //Temporary
                break;


        }


    }

    // Update is called once per frame
    void Update()
    {
    }


    public enum FurretType{NosePoker,Jumper, Boss};
}

