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

    private void OnCollisionEnter(Collision other)
    {
        switch (other.gameObject.tag)
        {
            case "Untagged":
                player.AddGroundCollider(thisCollider, other.collider);
                break;
            case "Harmful":
                DamagePlayer(other.collider);
                break;
            default:
                break;
        }
        SetColliderObject(other);
    }

    private void OnCollisionExit(Collision other)
    {
        switch (other.gameObject.tag)
        {
            case "Untagged":
                player.RemoveGroundCollider(thisCollider, other.collider);
                break;

            case "Harmful":
                player.HarmfulTouch = false;
                break;
            default:
                break;
        }
        ClearColliderObject();
    }

    private void OnTriggerEnter(Collider other){
        switch (other.gameObject.tag)
        {
            case "EnemyWeakspot":
                break;
            case "Water":
                break;
            case "OneHitKill":
                player.OHKTouch = true;
                break;
            case "Finish":
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
            default:
                break;
        }
        SetTriggerObject(other);
    }

    private void OnTriggerExit(Collider other)
    {
        switch (other.gameObject.tag)
        {
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
            default:
                break;
        }
        ClearTriggerObject();
    }

    public void DamagePlayer(Collider other)
    {
        player.HarmfulTouch = true;
        player.HarmfulDamageAmount = other.GetComponent<DamagePlayer>().damageAmount;
        player.HarmfulTouchObjectPosition = other.transform.position;
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
