using UnityEngine;

public class RumbaPlayerHitSensor : MonoBehaviour
{
    [SerializeField] RumbaWithKnife rumba;
    [SerializeField] HarmfulObject knife;
    //if = invincibilityFrame
    [SerializeField] private float ifMax = 60f;
    [SerializeField] private float ifCounter;
    [SerializeField] private bool ifCounting;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ifCounter = 0f;
        ifCounting = false;
    }

    void Update()
    {
        if (ifCounting)
        {
            ifCounter++;
            if(ifCounter > ifMax)
            {
                ifCounting = false;
                ifCounter = 0f;
            }
        }
    }
    
    void OnCollisionEnter(Collision collision)
    {
        if(ifCounting) return;
        Player player = collision.collider.GetComponentInParent<Player>();
        if (player != null && player.gameObject.CompareTag("Player")) 
        {
            rumba.HP -=1; 
            ifCounting = true;
        }
    }
}
