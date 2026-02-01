using UnityEngine;

public class DestroyIfNoChildren : MonoBehaviour
{
    private void Start()
    {
        if (transform.childCount == 0)
        {
            Destroy(gameObject);
        }
    }
}
