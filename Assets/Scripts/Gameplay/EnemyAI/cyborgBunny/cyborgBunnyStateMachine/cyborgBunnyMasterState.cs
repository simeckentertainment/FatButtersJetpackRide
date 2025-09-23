using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CyborgBunnyMasterState
{
    protected CyborgBunny cyborgBunny;
    protected CyborgBunnyStateMachine cyborgBunnyStateMachine;
    protected int durationOfState = 0;
    public CyborgBunnyMasterState(CyborgBunny cyborgBunny, CyborgBunnyStateMachine cyborgBunnyStateMachine)
    {
        this.cyborgBunny = cyborgBunny;
        this.cyborgBunnyStateMachine = cyborgBunnyStateMachine;
    }
    GameObject[] lightningPlates;
    public virtual void enter()
    {
        durationOfState = 0;
        lightningPlates = cyborgBunny.lightningPlates;
        foreach (GameObject plate in lightningPlates){plate.SetActive(false);}
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
        RunSpawner();
        EnsureOnZOrigin();

    }
    public virtual void exit()
    {

    }

    public Vector3 GetPlayerCoords()
    {
        return cyborgBunny.player.transform.position;
    }
    public void PlayAnim(string animName)
    {
        cyborgBunny.anim.Play("Bunny." + animName, 0, 0.0f);
    }
    void RunSpawner()
    {
        //If the player has been seen once or more, spawn a bunny every 5 seconds.
        if (cyborgBunny.playerSeen)
        {
            if (durationOfState % cyborgBunny.bunnySpawnFrequency == 0)
            {
                cyborgBunny.MultiplyRabbit();
            }
        }
    }

    public bool CastLineOfSightToPlayer()
    {
        Vector3 directionVector = (GetPlayerCoords() - cyborgBunny.LineOfSightSourceObj.position).normalized;
        LayerMask layerMask = LayerMask.GetMask("PlayerColliderBone");
        Debug.DrawRay(cyborgBunny.LineOfSightSourceObj.position, directionVector * cyborgBunny.LineOfSightMaxDistance, Color.green);
        RaycastHit outlook;
        if (Physics.Raycast(cyborgBunny.LineOfSightSourceObj.position, directionVector, out outlook, cyborgBunny.LineOfSightMaxDistance, layerMask))
        {
            if (outlook.transform.gameObject.tag == "Player")
            {
                if (!cyborgBunny.playerSeen)
                {
                    cyborgBunny.playerSeen = true;
                    cyborgBunny.stateMachine.changeState(cyborgBunny.cyborgBunnyNoticePlayerState);
                    return true;
                }
                else
                {
                    cyborgBunny.stateMachine.changeState(cyborgBunny.cyborgBunnyChasePlayerState);
                    return true;
                }
            }
            else
            {
                return false;
            }
        }
        return false;
    }


    void EnsureOnZOrigin()
    { //Keeps the bunny on the Z axis with the player.
        if (cyborgBunny.transform.position.z != 0)
        {
            cyborgBunny.transform.position = new Vector3(cyborgBunny.transform.position.x, cyborgBunny.transform.position.y, 0.0f);
        }
    }

        public void RunLightning()
    {   //This method runs the spinning lightning bolts when the Cyborg Bunny spawns.
        //Rather than plan something explicit, I'm using the Modulo operator as a way of mathematically running through
        //the items in the lightningPlate array and activating them one by one for only their animation length.
        DeactivateAllPlates();
        int currentRawCycleNumber = ((int)cyborgBunny.spawnAnimLength - ((int)cyborgBunny.spawnAnimLength - durationOfState)) / 4; //4 because there's 4 frames for the lightning animation.
        int numberOfBolts = cyborgBunny.lightningPlates.Length;
        int currentAdjustedCycleNumber = currentRawCycleNumber % numberOfBolts;
        cyborgBunny.lightningPlates[currentAdjustedCycleNumber].SetActive(true);
    }

    public void DeactivateAllPlates()
    {
        foreach (GameObject plate in lightningPlates)
        {
            plate.SetActive(false);
        }
    }
}
