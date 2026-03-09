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
        switch (other.gameObject.tag)
        {
            case "Untagged":
                player.GroundTouch = true;
                SetColliderObject(other);
                break;
            case "Player":
                ClearColliderObject();
                break;
            case "PlayerDamageTrigger":
                ClearColliderObject();
                break;
            case "EnemySightBox":
                ClearColliderObject();
                break;
            default:
                player.OtherObjectTouch = true;
                SetColliderObject(other);
                break;
        }

    }
    private void OnCollisionExit(Collision other)
    {
        switch (other.gameObject.tag)
        {
            case "Untagged":
                player.GroundTouch = false;
                player.OtherObjectTouch = false;
                break;
            case "PlayerDamageTrigger":
                break;
            case "Player":
                break;
            case "EnemySightBox":
                break;
            default:
                player.OtherObjectTouch = false;
                break;
        }
        ClearColliderObject();
    }
    private void OnTriggerEnter(Collider other){
        switch (other.gameObject.tag)
        {
            case "Untagged":
                player.GroundTouch = true;
                player.OtherObjectTouch = false;
                SetTriggerObject(other);
                break;
            case "EnemyWeakspot":
                SetTriggerObject(other);
                break;
            case "Water":
                SetTriggerObject(other);
                break;
            case "Harmful":
                SetTriggerObject(other);
                player.HarmfulTouch = true;
                player.HarmfulDamageAmount = other.GetComponent<DamagePlayer>().damageAmount;
                player.HarmfulTouchObjectPosition = other.transform.position;
                break;
            case "OneHitKill":
                SetTriggerObject(other);
                player.OHKTouch = true;
                break;
            case "Finish":
                SetTriggerObject(other);
                player.FinishTouch = true;
                break;
            case "LowGravArea":
            //We need to be able to detect other collisions during no grav mode.
                player.LowGravMode = true;
                break;
            case "KillThrust":
                //We need to be able to detect other collisions during kill thrust mode.
                if(!player.CollidersInJetpackKillZone.Contains(thisCollider)){
                    player.CollidersInJetpackKillZone.Add(thisCollider);
                }
                break;
            case "Player":
                ClearTriggerObject();
                break;
            case "PlayerDamageTrigger":
                ClearTriggerObject();
                break;
            case "EnemySightBox":
                ClearTriggerObject();
                break;
            default:
                player.OtherObjectTouch = true;
                SetTriggerObject(other);
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
            case "EnemySightBox":
                break;
            default:
                player.OtherObjectTouch = false;
                break;
        }
        ClearTriggerObject();
    }
    void OnParticleCollision(GameObject other)
    {
        if (other.tag == "Harmful")
        {
            player.HarmfulTouch = true;
            player.HarmfulDamageAmount = other.GetComponent<DamagePlayer>().damageAmount;
            player.HarmfulTouchObjectPosition = other.transform.position;
        }
    }

    void SetColliderObject(Collision other)
    {
        didICollideSomethingThisTime = true;
        CollisionObject = other.gameObject;
    }
    void ClearColliderObject()
    {
        didICollideSomethingThisTime = false;
        CollisionObject = null;
    }
    void SetTriggerObject(Collider other)
    {
        didITriggerSomethingThisTime = true;
        TriggerObject = other.gameObject;
    }
    void ClearTriggerObject()
    {
        didITriggerSomethingThisTime = false;
        TriggerObject = null;
    }
}
