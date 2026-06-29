using UnityEngine;

public class RumbaCollisionSensor : MonoBehaviour
{
    [SerializeField] RumbaWithKnife rumba;

    private void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.CompareTag("Player")){DetectWallHit(other);}
    }

    private void DetectWallHit(Collision other)
    {
        if(other.gameObject.tag == "Player" ||other.gameObject.tag == "PlayerDamageTrigger" ) {return;}
        BoxCollider box = GetComponent<BoxCollider>();
        Vector3 contactPoint = other.contacts[0].point;
        Vector3 localContact = transform.InverseTransformPoint(contactPoint);
        // Box extents in local space
        Vector3 halfExtents = box.size * 0.5f;
        float tolerance = 0.01f;
        if (Mathf.Abs(localContact.x - halfExtents.x) < tolerance)
        {
            rumba.wallDetected = RumbaWithKnife.Direction.Right;
        }
        else if (Mathf.Abs(localContact.x + halfExtents.x) < tolerance)
        {
            rumba.wallDetected = RumbaWithKnife.Direction.Left;
        }
    }
}
