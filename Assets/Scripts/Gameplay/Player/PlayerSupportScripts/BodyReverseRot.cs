using UnityEngine;

public class BodyReverseRot : MonoBehaviour
{
    public Transform OGButtersTrans;
    [SerializeField] public float offset;
    public AffectAxis affectAxis;

    private void FixedUpdate()
    {
        // float zAngle = OGButtersTrans.localEulerAngles.z;
        // float invertedAngle = (-zAngle + offset);

        if (affectAxis == AffectAxis.Z)
        {
            transform.rotation = Quaternion.Euler(0, 0, (OGButtersTrans.rotation.z * -1) + offset);
        }
        else if (affectAxis == AffectAxis.Y)
        {
            transform.rotation = Quaternion.Euler(0f, (OGButtersTrans.rotation.z * -1) + offset, 0f);
        }
        else
        {
            transform.rotation = Quaternion.Euler((OGButtersTrans.rotation.z * -1) + offset, 0f, 0f);
        }
        // This shouldn't work. It works but it shouldn't. The connections I thought made it work
        // are broken now, much like my spirit. ~Randy
    }

    public enum AffectAxis {X,Y,Z};

}