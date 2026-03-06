using UnityEngine;

public class PlayerCollisionReporter : MonoBehaviour
{
    [SerializeField] public Player player;
    [SerializeField] bool didITriggerSomethingThisTime;
    [SerializeField] bool didICollideSomethingThisTime;
    Collider thisCollider;
    [Header("Sanity Checkers")]
    [SerializeField] GameObject CollisionObject;
    [SerializeField] GameObject TriggerObject;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        thisCollider = gameObject.GetComponent<Collider>();
        didITriggerSomethingThisTime = false;
        didICollideSomethingThisTime = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision other)
    {
        didICollideSomethingThisTime = true;
        CollisionObject = other.gameObject;
        switch (other.gameObject.tag)
        {
            case "Untagged":
                player.GroundTouch = true;
                break;
            case "PlayerDamageTrigger":
                break;
            default:
                player.OtherObjectTouch = true;
                break;
        }

    }
    private void OnCollisionExit(Collision other)
    {
        switch (other.gameObject.tag)
        {
            case "Untagged":
                player.GroundTouch = false;
                break;
            case "PlayerDamageTrigger":
                break;
            default:
                player.OtherObjectTouch = false;
                break;
        }
        didICollideSomethingThisTime = false;
        CollisionObject = null;
    }
    private void OnTriggerEnter(Collider other){
        didITriggerSomethingThisTime = true;
        TriggerObject = other.gameObject;
        switch (other.gameObject.tag)
        {
            case "Untagged":
                player.GroundTouch = true;
                break;
            case "EnemyWeakspot":

                break;
            case "Water":

                break;
            case "Harmful":
                player.HarmfulTouch = true;
                player.HarmfulDamageAmount = other.GetComponent<DamagePlayer>().damageAmount;
                player.HarmfulTouchObjectPosition = other.transform.position;
                break;
            case "OneHitKill":
                player.OHKTouch = true;
                break;
            case "Finish":
                player.FinishTouch = true;
                break;
            case "LowGravArea":
                player.LowGravMode = true;
                break;
            case "KillThrust":
                if(!player.CollidersInJetpackKillZone.Contains(thisCollider)){
                    player.CollidersInJetpackKillZone.Add(thisCollider);
                }
                break;
            case "Player":
                break;
            case "PlayerDamageTrigger":
                break;
            default:
                player.OtherObjectTouch = true;
                break;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Untagged":
                player.GroundTouch = false;
                break;
            case "Fuel":
                player.FuelTouch = false;
                break;
            case "EnemyWeakspot":

                break;
            case "Ball":
                player.BallTouch = false;
                break;
            case "Water":

                break;
            case "Harmful":
                player.HarmfulTouch = false;
                break;
            case "OneHitKill":
                player.OHKTouch = false;
                break;
            case "Finish":
                player.FinishTouch = false;
                break;
            case "LowGravArea":
                player.LowGravMode = false;
                break;
            case "KillThrust":
                if (player.CollidersInJetpackKillZone.Contains(thisCollider))
                {
                    player.CollidersInJetpackKillZone.Remove(thisCollider);
                }
                break;
            case "Player":
                break;
            case "PlayerDamageTrigger":
                break;
            default:
                player.OtherObjectTouch = false;
                break;
        }
        didITriggerSomethingThisTime = false;
        TriggerObject = null;
    }
    void OnParticleCollision(GameObject other){
        if(other.tag == "Harmful"){
            player.HarmfulTouch = true;
            player.HarmfulDamageAmount = other.GetComponent<DamagePlayer>().damageAmount;
            player.HarmfulTouchObjectPosition = other.transform.position;
        }
    }
}
