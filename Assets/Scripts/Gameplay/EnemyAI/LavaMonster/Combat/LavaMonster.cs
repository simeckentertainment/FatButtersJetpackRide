using Unity.Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class LavaMonster : MonoBehaviour
{
    [SerializeField] public LavaMonsterStateMachine stateMachine;
    public LavaMonsterSpawnState lavaMonsterSpawnState { get; set; }
    public LavaMonsterWalkState lavaMonsterWalkState { get; set; }
    public LavaMonsterHighSwipeState lavaMonsterHighSwipeState { get; set; }
    public LavaMonsterFireballAttackState lavaMonsterFireballAttackState { get; set; }
    public LavaMonsterFallState lavaMonsterFallState { get; set; }
    [SerializeField] public Transform upDownDeterminator;
    [SerializeField] public GameObject player;
    [SerializeField] public Animator anim;
    [SerializeField] public float WalkSpeed;
    [SerializeField] public bool attackNow;
    [SerializeField] public float maximumAttackDistance;
    public bool dieNow;
    [SerializeField] CinemachineCamera cam;
    [SerializeField] float DistanceToPlayer;
    [SerializeField] public GameObject FireballModel;
    [System.NonSerialized] public string FireballLocation = "Assets/Objects/PuzzlePieces/prefabs/dynamics/lavaMonsterFireball.prefab";

    // Start is called before the first frame update
    void Start()
    {
        player = Helper.NabPlayerObj();
        cam = Helper.NabGameplayCamera().GetComponent<CinemachineCamera>();
        SetCameraDistance();
        lavaMonsterSpawnState = new LavaMonsterSpawnState(this, stateMachine);
        lavaMonsterWalkState = new LavaMonsterWalkState(this, stateMachine);
        lavaMonsterHighSwipeState = new LavaMonsterHighSwipeState(this, stateMachine);
        lavaMonsterFireballAttackState = new LavaMonsterFireballAttackState(this, stateMachine);
        lavaMonsterFallState = new LavaMonsterFallState(this, stateMachine);
        stateMachine.Initialize(lavaMonsterSpawnState);
    }

    private void SetCameraDistance()
    {   //I'll just need to turn this off for now...
        cam.gameObject.GetComponent<CinemachineThirdPersonFollow>().CameraDistance = 50;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (transform.position.x > 600.0f)
        {
            stateMachine.changeState(lavaMonsterFallState);
        }
        DistanceToPlayer = CheckPlayerDistance();
    }

    public float CheckPlayerHeight()
    {
        return player.transform.position.y;

    }
    public float CheckPlayerDistance()
    {
        //return Vector3.Distance(transform.position, player.transform.position);
        return player.transform.position.x - transform.position.x;
    }

    void SetPlayerCameraDistance()
    {

    }
    public void ActivateHitWithBallPowerup()
    {
        stateMachine.changeState(lavaMonsterFallState);
    }
}

