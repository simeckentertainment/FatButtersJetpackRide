using UnityEngine;

public class RumbaExplosionLifetimeTracker : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] int counterMax = 5;
    int counterCurrent = 0;
    void Start()
    {
        counterCurrent = 0;
    }

    // Update is called once per frame
    void Update()
    {
        counterCurrent++;
        if(counterCurrent >= counterMax)
        {
            Destroy(gameObject);
        }
    }
}
