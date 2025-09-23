using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class MainMenuCameraMover : MonoBehaviour
{
    [SerializeField] public Transform MainMenu;
    [SerializeField] public Transform CreditsMenu;
    public float transitionCounter = 0;
    public float transitionMax;
    Vector3 currentTrans;
    Quaternion currentRot;
    Vector3 goalTrans;
    Quaternion goalRot;
    bool moving;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(moving){
            transitionCounter += 1;
            transform.position = Vector3.Lerp(currentTrans,goalTrans,transitionCounter/transitionMax);
            transform.rotation = Quaternion.Lerp(currentRot,goalRot,transitionCounter/transitionMax);
        }
        if(transitionCounter == transitionMax){
            moving = false;
            transitionCounter = 0;
        }
    }

    public void MoveTo(Transform targetTrans){
        transitionCounter = 0;
        currentTrans = transform.position;
        currentRot = transform.rotation;
        goalTrans = targetTrans.position;
        goalRot = targetTrans.rotation;
        moving = true;
    }
}
