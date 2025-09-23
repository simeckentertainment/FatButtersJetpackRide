using UnityEngine;

public class PlayerCollisionReporter : MonoBehaviour
{
    [SerializeField] public Player player;
    [SerializeField] bool didITriggerSomethingThisTime;
    [SerializeField] bool didICollideSomethingThisTime;
    [Header("Sanity Checkers")]
    [SerializeField] GameObject CollisionObject;
    [SerializeField] GameObject TriggerObject;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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
            case "Fuel":
                player.JerryCanTouch = true;
                player.FuelAdditionAmount = other.gameObject.GetComponent<PowerupRotator>().increaseFuel;
                Destroy(other.gameObject);
                break;
            case "Food":
                player.FoodTouch = true;
                player.FoodAdditionAmount = other.gameObject.GetComponent<PowerupRotator>().increaseTreats;
                Destroy(other.gameObject);
                break;
            case "Bone":
                player.BoneTouch = true;
                Destroy(other.gameObject);
                break;
            case "EnemyWeakspot":

                break;
            case "Ball":
                player.BallTouch = true;
                Destroy(other.gameObject);
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
                player.JerryCanTouch = false;
                break;
            case "Food":
                player.FoodTouch = false;
                break;
            case "Bone":
                player.BoneTouch = false;
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
