using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MrSudsMasterState{
    protected MrSuds mrSuds;
    protected MrSudsStateMachine mrSudsStateMachine;
    protected int durationOfState = 0;
    public MrSudsMasterState(MrSuds mrSuds, MrSudsStateMachine mrSudsStateMachine){
        this.mrSuds = mrSuds;
        this.mrSudsStateMachine = mrSudsStateMachine;
    }
    public virtual void enter(){
        durationOfState = 0;
    }
    public virtual void enterNoanimate(){
        durationOfState = 0;
    }
    // Start is called before the first frame update
    void Start(){
        
    }
    // Update is called once per frame
    public virtual void Update(){

    }
    public virtual void FixedUpdate(){
        durationOfState++;
    }
    public virtual void exit(){

    }
}
