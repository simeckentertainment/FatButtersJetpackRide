using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RumbaWithKnifeMasterState{
    protected RumbaWithKnife rumba;
    protected RumbaWithKnifeStateMachine rumbaWithKnifeStateMachine;
    protected int durationOfState = 0;
    public RumbaWithKnifeMasterState(RumbaWithKnife rumba, RumbaWithKnifeStateMachine rumbaWithKnifeStateMachine){
        this.rumba = rumba;
        this.rumbaWithKnifeStateMachine = rumbaWithKnifeStateMachine;
    }
    public virtual void enter(){
        durationOfState = 0;
        rumba.CalibrateRaycastNodes(); //Make sure that the raycast nodes are properly calibrated upon every state entry.
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
    public virtual void PlayAnim(string animName)
    {
        rumba.anim.Play(animName);
    }

    public virtual bool CheckAnimName(string animName)
    {
        return rumba.anim.GetCurrentAnimatorStateInfo(0).IsName(animName);
    }
    public virtual string GetCurrentAnimName()
    {
        return rumba.anim.GetCurrentAnimatorStateInfo(0).ToString();
    }
    public virtual float AnimNormalizedTime()
    {
        return rumba.anim.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }
    public virtual bool AnimFinished()
    {
        return rumba.anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f;
    }
    public virtual bool PlayerDetected(){
        return rumba.PlayerDetected;
    }
    public virtual void SetRumbaRotation(float targetRot)
    {
        rumba.transform.rotation = Quaternion.Euler(new Vector3(rumba.transform.rotation.eulerAngles.x, targetRot, rumba.transform.rotation.eulerAngles.z));
    }
    public virtual void MoveToSpotForThisFrame(float speed)
    {
        Vector3 newPos = rumba.transform.position + rumba.transform.forward * speed * Time.fixedDeltaTime;
        newPos.z = 0f;
        rumba.transform.position = newPos;
    }
}
