using UnityEngine;

public class PlayerHitDetection : MonoBehaviour
{
    [SerializeField] SegwayBear segwayBear;
    [SerializeField] SphereCollider sphereCollider;
    bool isDead;

    void Start()
    {
        isDead = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isDead)
        {
            return;
        }

        if (other.GetComponentInParent<Player>() != null)
        {
            segwayBear.beaten = true;
            isDead = true;
            segwayBear.stateMachine.changeState(segwayBear.seBBeatenState);
        }
    }
}
