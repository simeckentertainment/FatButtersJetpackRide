using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyReverseRot : MonoBehaviour
{
    public Transform OGButtersTrans;
    [SerializeField] public float offset;
    public AffectAxis affectAxis;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (affectAxis == AffectAxis.Z){
            transform.rotation = Quaternion.Euler(0, 0, (OGButtersTrans.rotation.z * -1) + offset);
        } else if (affectAxis == AffectAxis.Y){
            transform.rotation = Quaternion.Euler(0f, (OGButtersTrans.rotation.z * -1) + offset, 0f);

        }else{
            transform.rotation = Quaternion.Euler(0f, (OGButtersTrans.rotation.z * -1) + offset, 0f);
        }

    }

        public enum AffectAxis {X,Y,Z};

    }