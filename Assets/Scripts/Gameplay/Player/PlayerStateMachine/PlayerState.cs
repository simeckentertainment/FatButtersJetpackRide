using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerState
{
    protected Player player;
    protected PlayerStateMachine playerStateMachine;
    protected int durationOfState = 0;
    protected int skindex;
    public PlayerState(Player player, PlayerStateMachine playerStateMachine)
    {
        this.player = player;
        this.playerStateMachine = playerStateMachine;

    }
    public virtual void enter()
    {
        skindex = player.skindex;
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
    public float GetNormalizedTime()
    { //Gets the normalized time, guaranteed to always be less than 1. Assumed to be layer 0.
        return player.anim.GetCurrentAnimatorStateInfo(0).normalizedTime % 1;
    }
    public float GetNormalizedTime(int layer)
    { //Gets the normalized time of a specific layer, guaranteed to always be less than 1.
        return player.anim.GetCurrentAnimatorStateInfo(layer).normalizedTime % 1;
    }
    public void PlayOneTimeAudio(AudioClip clip)
    {
        player.sfx.loop = false;
        player.sfx.volume = player.saveManager.collectibleData.SFXVolumeLevel;
        player.sfx.clip = clip;
        player.sfx.Play();
    }
    public string GetCurrentAnimName()
    {

        if (player.anim.GetCurrentAnimatorClipInfo(0).Length > 0){
            return player.anim.GetCurrentAnimatorClipInfo(0)[0].clip.name;
        } else {
            return "";
        }
    }
}
