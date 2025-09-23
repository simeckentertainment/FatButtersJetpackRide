using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulleyManager : MonoBehaviour
{

    //[SerializeField] Player player;
    public bool PlayerOnLeft;
    public bool PlayerOnRight;
    public float maxLength;
    static float leftDefaultLength;
    static float rightDefaultLength;
    bool snapped;
    float leftCurrentLength;
    float rightCurrentLength;
    [SerializeField] Transform leftStretchRope;
    [SerializeField] Transform rightStretchRope;
    [SerializeField] Transform leftControlKnot;
    [SerializeField] Transform rightControlKnot;
    [SerializeField] GameObject leftPlatform;
    [SerializeField] GameObject rightPlatform;
    [SerializeField] Transform[] leftDistanceObjects;
    [SerializeField] Transform[] rightDistanceObjects;

    // Start is called before the first frame update
    void Start()
    {
        snapped = false;
        leftCurrentLength = leftDefaultLength = Vector3.Distance(leftDistanceObjects[0].position,leftDistanceObjects[1].position);
        rightCurrentLength = rightDefaultLength = Vector3.Distance(rightDistanceObjects[0].position,rightDistanceObjects[1].position);
        maxLength = leftDefaultLength + rightDefaultLength;
    }

    // Update is called once per frame

    void FixedUpdate()
    {
        if(!snapped){
            RunPlayerInteraction();
            AdjustRopeScale();
        }
    }
    private void RunPlayerInteraction(){
        if (PlayerOnLeft){
            RunPlayerOnLeft();
        }if (PlayerOnRight){
            RunPlayerOnRight();
        }
    }

    void RunPlayerOnLeft(){
        if(Helper.isWithinMarginOfError(leftCurrentLength,maxLength,0.25f)){
            Snap();
            return;
        }
        leftControlKnot.position = new Vector3(leftControlKnot.position.x,leftControlKnot.position.y-0.1f,leftControlKnot.position.z);
        rightControlKnot.position = new Vector3(rightControlKnot.position.x,rightControlKnot.position.y+0.1f,rightControlKnot.position.z);


    }
    void RunPlayerOnRight(){
        if(Helper.isWithinMarginOfError(rightCurrentLength,maxLength,0.25f)){
            Snap();
            return;
        }
        rightControlKnot.position = new Vector3(rightControlKnot.position.x,rightControlKnot.position.y-0.1f,rightControlKnot.position.z);
        leftControlKnot.position = new Vector3(leftControlKnot.position.x,leftControlKnot.position.y+0.1f,leftControlKnot.position.z);

    }
    void Snap(){
        snapped = true;
        leftPlatform.AddComponent<Rigidbody>();
        leftPlatform.GetComponent<BoxCollider>().isTrigger = false;
        rightPlatform.AddComponent<Rigidbody>();
        rightPlatform.GetComponent<BoxCollider>().isTrigger = false;
    }

    void AdjustRopeScale(){
        leftCurrentLength = Vector3.Distance(leftDistanceObjects[0].position,leftDistanceObjects[1].position);
        rightCurrentLength = Vector3.Distance(rightDistanceObjects[0].position,rightDistanceObjects[1].position);
        leftStretchRope.localScale = new Vector3(leftStretchRope.localScale.x,leftCurrentLength/leftDefaultLength,leftStretchRope.localScale.z);
        rightStretchRope.localScale = new Vector3(rightCurrentLength/leftDefaultLength,rightStretchRope.localScale.y,rightStretchRope.localScale.z);
    }

}
