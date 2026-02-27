using UnityEngine;

public class PowerupRotator : MonoBehaviour
{
    [SerializeField] float rotationAmount;

    private void FixedUpdate()
    {
        transform.Rotate(new Vector3(0, rotationAmount, 0));
    }
}
