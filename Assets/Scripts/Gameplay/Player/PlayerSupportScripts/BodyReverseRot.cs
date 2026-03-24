using UnityEngine;

public class BodyReverseRot : MonoBehaviour
{
    [SerializeField] InputDriver input;
    [SerializeField] public float offset;
    [SerializeField] float aimAngleSensitivity;
    public AffectAxis affectAxis;
    [SerializeField] bool invert;
    [SerializeField] float invertFloat;
    [SerializeField] bool dynamicSensitivity;

    private void FixedUpdate()
    {
        invertFloat = invert ? -1.0f : 1.0f;


        if (dynamicSensitivity)
        {
            aimAngleSensitivity = Helper.RemapArbitraryValues(0.0f, 45.0f, 1.0f, 1.55f, Mathf.Abs(input.aimAngle));
        }
        
        if (affectAxis == AffectAxis.Z)
        {
            transform.rotation = Quaternion.Euler(0, 0, input.aimAngle * invertFloat * aimAngleSensitivity);
        }
        else if (affectAxis == AffectAxis.Y)
        {
            transform.rotation = Quaternion.Euler(0f, input.aimAngle * invertFloat * aimAngleSensitivity, 0f);
        }
        else
        {
            transform.rotation = Quaternion.Euler(input.aimAngle * invertFloat * aimAngleSensitivity, 0f, 0f);
        }




        // This shouldn't work. It works but it shouldn't. The connections I thought made it work
        // are broken now, much like my spirit. ~Randy
    }

    public enum AffectAxis {X,Y,Z};

}