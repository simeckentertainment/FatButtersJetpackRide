using System.Collections;
using Unity.Cinemachine;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;


public class EventTriggeers : MonoBehaviour
{

    [SerializeField] GameObject virtualCameraGO;
    [SerializeField] GameObject jetPackCameraGO;
    [SerializeField] CinemachineCamera lootAtCamera;
    [SerializeField] Player player;
    [SerializeField] Transform bear;
    [SerializeField] PlayableDirector timeline;
   private float keyboardOffset;
   public bool killThrust = true;
   



    // kill thrust until player picks jetpack.
    void Update()
    {
        if(killThrust)
        {
          //  player.input.GoThrust = false;
        }
    }

    // Play bear timeline

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            timeline.Play();
        }
    }

 
    //set up camera transition and keyboard input offset 
    public void CameraTransition(bool inputDriverBool)
    {
        lootAtCamera.LookAt = bear;
        //player.GetComponent<InputDriver>().enabled = inputDriverBool;
       // player.input.aimAngle = 0;
        //virtualCameraGO.transform.rotation = quaternion.Euler(0,0,0);
    }

 

   
}
