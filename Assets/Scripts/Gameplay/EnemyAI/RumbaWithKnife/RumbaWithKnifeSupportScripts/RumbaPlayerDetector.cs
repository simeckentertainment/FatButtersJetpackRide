using UnityEngine;

public class RumbaPlayerDetector : MonoBehaviour
{
    public bool PlayerDetected { get; private set; }
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SetPlayerDetection(true);
        }
    }

    public void SetPlayerDetection(bool value)
    {
        PlayerDetected = value;
    }
}
