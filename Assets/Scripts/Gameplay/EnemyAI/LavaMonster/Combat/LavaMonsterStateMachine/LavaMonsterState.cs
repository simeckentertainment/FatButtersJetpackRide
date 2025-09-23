using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LavaMonsterState
{
    protected LavaMonster lavaMonster;
    protected LavaMonsterStateMachine lavaMonsterStateMachine;
    protected int durationOfState = 0;

    public LavaMonsterState(LavaMonster lavaMonster, LavaMonsterStateMachine lavaMonsterStateMachine)
    {
        this.lavaMonster = lavaMonster;
        this.lavaMonsterStateMachine = lavaMonsterStateMachine;

    }
    public virtual void enter()
    {
        durationOfState = 0;
    }
    public virtual void enterNoanimate()
    {
        durationOfState = 0;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public virtual void Update()
    {

    }

    public virtual void FixedUpdate()
    {
        durationOfState++;
    }

    public virtual void exit()
    {

    }
    public float GetAnimTime()
    {
        return lavaMonster.anim.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }
}
