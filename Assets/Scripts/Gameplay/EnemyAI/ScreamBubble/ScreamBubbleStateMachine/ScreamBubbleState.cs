using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ScreamBubbleState
{
    protected ScreamBubble screamBubble;
    protected ScreamBubbleStateMachine screamBubbleStateMachine;
    protected int durationOfState = 0;

    public ScreamBubbleState(ScreamBubble screamBubble, ScreamBubbleStateMachine screamBubbleStateMachine)
    {
        this.screamBubble = screamBubble;
        this.screamBubbleStateMachine = screamBubbleStateMachine;

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
    public virtual void PlayAnim(string animName)
    {
        Debug.Log("Playing " + animName);
        screamBubble.anim.Play("Base Layer." + animName, 0, 0.0f);
    }
    public virtual float GetAnimNormalizedTime()
    { //Gets the normalized time, guaranteed to always be less than 1. Assumed to be layer 0.
        return screamBubble.anim.GetCurrentAnimatorStateInfo(0).normalizedTime % 1;
    }
    public virtual float GetAnimNormalizedTime(int layer)
    { //Gets the normalized time of a specific layer, guaranteed to always be less than 1.
        return screamBubble.anim.GetCurrentAnimatorStateInfo(layer).normalizedTime % 1;
    }
    public virtual string GetCurrentAnimName()
    {
        if (screamBubble.anim.GetCurrentAnimatorClipInfo(0).Length > 0){
            return screamBubble.anim.GetCurrentAnimatorClipInfo(0)[0].clip.name;
        } else {
            return "";
        }
    }
}
