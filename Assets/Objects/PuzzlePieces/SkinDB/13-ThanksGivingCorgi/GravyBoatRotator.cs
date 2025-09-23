using UnityEngine;

public class GravyBoatRotator : MonoBehaviour
{
    Player player;

    public float activeAngle = 360f;
    public float passiveAngle = 270f;
    [SerializeField] float currentGoal;
    [SerializeField] float currentRot;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        if (player.gbr1 == null)
        {
            player.gbr1 = this;
        }
        else
        {
            player.gbr2 = this;
        }

        transform.localRotation = Quaternion.Euler(passiveAngle, 0f, 0f);
        currentRot = NormalizeAngle(transform.localRotation.eulerAngles.x);
        currentGoal = passiveAngle;
    }

    void Update()
    {
        currentRot = NormalizeAngle(transform.localRotation.eulerAngles.x);

        if (Mathf.Approximately(currentRot, currentGoal))
            return;

        if (AngleDifference(currentRot, currentGoal) > 0)
        {
            MoveOneStepUp();
        }
        else
        {
            MoveOneStepDown();
        }
    }

    void MoveOneStepUp()
    {
        float newRotX = currentRot + 5f;
        if (AngleDifference(newRotX, currentGoal) <= 0)
            newRotX = currentGoal;

        transform.localRotation = Quaternion.Euler(newRotX, 0f, 0f);
    }

    void MoveOneStepDown()
    {
        float newRotX = currentRot - 5f;
        if (AngleDifference(newRotX, currentGoal) >= 0)
            newRotX = currentGoal;

        transform.localRotation = Quaternion.Euler(newRotX, 0f, 0f);
    }

    float NormalizeAngle(float angle)
    {
        angle %= 360f;
        if (angle < 0f)
            angle += 360f;
        return angle;
    }

    float AngleDifference(float from, float to)
    {
        return Mathf.DeltaAngle(from, to); // Gives signed shortest angle difference
    }

    public void ActivateBoat()
    {
        currentGoal = activeAngle;
    }

    public void DeactivateBoat()
    {
        currentGoal = passiveAngle;
    }
}
