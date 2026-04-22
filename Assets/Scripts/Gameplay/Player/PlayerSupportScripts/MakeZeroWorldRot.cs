using UnityEngine;

public class MakeZeroWorldRot : MonoBehaviour
{
    [SerializeField] private Vector3 rotationAngle = Vector3.zero;

    private void Update()
    {
        transform.rotation = Quaternion.Euler(rotationAngle);
    }
}
