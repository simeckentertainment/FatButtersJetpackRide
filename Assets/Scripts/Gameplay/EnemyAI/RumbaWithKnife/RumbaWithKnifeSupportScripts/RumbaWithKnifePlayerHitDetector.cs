using UnityEngine;

public class RumbaWithKnifePlayerHitDetector : MonoBehaviour
{
    [SerializeField] RumbaWithKnife rumba;

    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.CompareTag("Player"))
        {
            rumba.HP -= 1.0f;
        }
    }
}
