using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CowboyBossMasterState{
    protected CowboyBoss cowboyBoss;
    protected CowboyBossStateMachine cowboyBossStateMachine;
    protected int durationOfState = 0;
    public CowboyBossMasterState(CowboyBoss cowboyBoss, CowboyBossStateMachine cowboyBossStateMachine){
        this.cowboyBoss = cowboyBoss;
        this.cowboyBossStateMachine = cowboyBossStateMachine;
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
    public float GetAnimTime(){
        return cowboyBoss.anim.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }

    public bool ReachedGoalAnimTime(float input){
        return cowboyBoss.anim.GetCurrentAnimatorStateInfo(0).normalizedTime > input ? true : false;
    }

    public bool CheckAnimName(string name){
        return cowboyBoss.anim.GetCurrentAnimatorStateInfo(0).IsName(name);
    }
}
