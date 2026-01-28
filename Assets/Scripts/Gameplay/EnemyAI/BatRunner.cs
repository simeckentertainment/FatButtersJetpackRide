using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatRunner : MonoBehaviour
{
    enum BatState { FlyPath, Dive, Retreat }

    [Header("Movement")]
    public float speed = 3f;
    public float diveSpeed = 7f;
    public float retreatSpeed = 5f;

    [Header("Wobble")]
    public float wobbleSpeed = 6f;
    public float wobbleAmount = 0.4f;

    [Header("Attack")]
    public float detectRange = 4f;
    public float attackCooldown = 2f;

    [Header("Audio")]
    public AudioSource screech;
    public AudioClip[] batSounds;

    [HideInInspector] public List<Vector3> path;

    Transform player;
    BatState state;

    int pathIndex;
    Vector3 currentTarget;
    Vector3 diveTarget;

    float cooldownTimer;
    float wobbleOffset;
    [Header("Flocking")]
    public float neighborRadius = 3f;
    public float separationWeight = 1.2f;
    public float alignmentWeight = 0.8f;
    public float cohesionWeight = 0.6f;

    static List<BatRunner> allBats = new List<BatRunner>();

    void OnEnable()
    {
        // Cache player
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;

        // Register bat for flocking
        allBats.Add(this);

        // Reset state
        pathIndex = 0;
        state = BatState.FlyPath;
        wobbleOffset = Random.Range(0f, 7f);
        cooldownTimer = 0f;

        // Validate path
        if (path == null || path.Count < 2)
        {
            Debug.LogWarning("BatRunner disabled: invalid path");
            gameObject.SetActive(false);
            return;
        }

        // Start moving to FIRST path point
        currentTarget =  path[Random.Range(1,path.Count-1)];//player.transform.position;

        // Play random screech
        if (batSounds != null && batSounds.Length > 0)
        {
            screech.clip = batSounds[Random.Range(0, batSounds.Length)];
            screech.time = Random.Range(0f, screech.clip.length);
            screech.Play();
        }
    }

    void OnDisable()
    {
        allBats.Remove(this);
    }

    [SerializeField] float maxLifetime = 20f;
    float lifeTimer;



    void Update()
    {
        cooldownTimer -= Time.deltaTime;

        switch (state)
        {
            case BatState.FlyPath:
                FlyAlongPath();
                TryAttack();
                break;

            case BatState.Dive:
                DiveAtPlayer();
                break;

            case BatState.Retreat:
                RetreatToPath();
                break;
        }

        Despawn();

    }


    // ---------------- MOVEMENT ----------------

    void FlyAlongPath()
    {
        Vector3 dir = (currentTarget - transform.position).normalized;

        // Sin-wave wobble
        Vector3 right = Vector3.Cross(Vector3.up, dir);
        float wobble = Mathf.Sin(Time.time * wobbleSpeed + wobbleOffset) * wobbleAmount;

        // Flocking (only when not diving)
        Vector3 flocking = CalculateFlockingForce();

        Vector3 moveDir = (dir + flocking).normalized;

        Vector3 move =
            moveDir * speed +
            right * wobble;

        transform.position += move * Time.deltaTime;
        transform.LookAt(transform.position + moveDir);

    }

    // ---------------- ATTACK ----------------

    void TryAttack()
    {
        if (player == null || cooldownTimer > 0) return;

        float dist = Vector3.Distance(transform.position, player.position);
        if (dist <= detectRange)
        {
            diveTarget = player.position;
            state = BatState.Dive;
        }
    }

    void DiveAtPlayer()
    {
        Vector3 dir = (diveTarget - transform.position).normalized;
        transform.position += dir * diveSpeed * Time.deltaTime;
        transform.LookAt(diveTarget);

        if (Vector3.Distance(transform.position, diveTarget) < 0.3f)
        {
            cooldownTimer = attackCooldown;
            state = BatState.Retreat;
        }
    }

    void RetreatToPath()
    {
        Vector3 retreatTarget = path[pathIndex];
        Vector3 dir = (retreatTarget - transform.position).normalized;

        transform.position += dir * retreatSpeed * Time.deltaTime;
        transform.LookAt(retreatTarget);

        if (Vector3.Distance(transform.position, retreatTarget) < 0.3f)
        {
            state = BatState.FlyPath;
        }
    }
    Vector3 CalculateFlockingForce()
    {
        Vector3 separation = Vector3.zero;
        Vector3 alignment = Vector3.zero;
        Vector3 cohesion = Vector3.zero;

        int count = 0;

        foreach (BatRunner other in allBats)
        {
            if (other == this) continue;

            float dist = Vector3.Distance(transform.position, other.transform.position);
            if (dist > neighborRadius) continue;

            // Separation
            separation += (transform.position - other.transform.position) / dist;

            // Alignment
            alignment += other.transform.forward;

            // Cohesion
            cohesion += other.transform.position;

            count++;
        }

        if (count == 0) return Vector3.zero;

        separation /= count;
        alignment /= count;
        cohesion = ((cohesion / count) - transform.position);

        return
            separation * separationWeight +
            alignment * alignmentWeight +
            cohesion * cohesionWeight;
    }
    // ---------------- CLEANUP ----------------

    void Despawn()
    {

        lifeTimer += Time.deltaTime;
        if (lifeTimer >= maxLifetime)
        {
            screech.Stop();
            Destroy(gameObject); // replace with pool later
        }
    }

  

}
