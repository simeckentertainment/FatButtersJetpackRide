using UnityEngine;

public class DafuqDidIHit : MonoBehaviour
{

    [SerializeField] GameObject CollisionObject;
    [SerializeField] GameObject TriggerObject;
    private void OnCollisionEnter(Collision other)
    {
        CollisionObject = other.gameObject;
    }

    private void OnCollisionExit(Collision other)
    {
        CollisionObject = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        TriggerObject = other.gameObject;
    }
    private void OnTriggerExit(Collider other)
    { 
        TriggerObject = null;
    }
}
