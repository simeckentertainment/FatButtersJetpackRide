using UnityEngine;

public class GasMeter : MonoBehaviour
{
    [SerializeField] Rigidbody playerRigidbody;
    [SerializeField] CorgiEffectHolder corgiEffectHolder;
    public float movementThreshold = 0.1f;  // Minimum velocity to be considered "moving"
    public float idleTimeLimit = 5f;        // Time (in seconds) before declaring "No movement"

    private float idleTimer = 0f;
    private bool isMoving = true;
    [SerializeField] AudioSource growlAudioSource;
    [SerializeField] AudioClip growlAudioClip;

    Player player;
    InputDriver inputDriver;

    void Start()
    {
        // player = playerRigidbody.GetComponent<Player>();
        // inputDriver = player.input;

        player = GetComponent<Player>();
        inputDriver = GetComponent<InputDriver>();
    }

    // NOTE: Boost logic has been moved to PlayerThrustState within the FSM
    // This method is kept for backwards compatibility but boost is now handled by the state machine
    // The boost detection is now in InputDriver.GoBoost

    void FixedUpdate()
    {
        
        float speed = playerRigidbody.linearVelocity.magnitude;

        if (speed >= movementThreshold)
        {
            // Player is moving
            if (!isMoving)
            {
                //Debug.Log("Movement happening");
                isMoving = true;
            }

            idleTimer = 0f; // Reset timer when movement occurs
        }
        else
        {
            // Player is not moving
            idleTimer += Time.fixedDeltaTime;

            if (idleTimer >= idleTimeLimit)// && isMoving)
            {
                //Debug.Log("No movement");
                playerRigidbody.GetComponent<Player>().Fuel += 0.5f;
                if (isMoving)
                {
                    growlAudioSource.clip = growlAudioClip;
                    growlAudioSource.Play();
                    corgiEffectHolder.StartMinusRotParticles();
                    corgiEffectHolder.StartPlusRotParticles();
                    isMoving = false;
                }

            }
        }
    }
}
