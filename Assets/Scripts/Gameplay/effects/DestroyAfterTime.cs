using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    [SerializeField] private float lifetimeRemaining;

    private void FixedUpdate()
    {
        lifetimeRemaining -= Time.deltaTime;

        if (lifetimeRemaining <= 0)
        {
            Destroy(gameObject);
        }
    }
}
