using UnityEngine;

public class RumbaPlayerHitSensor : MonoBehaviour
{
    [SerializeField] RumbaWithKnife rumba;
    [SerializeField] HarmfulObject knife;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnCollisionEnter(Collision collision)
    {
        Player player = collision.collider.gameObject.GetComponentInParent<Player>();
        if (knife.PlayerCollisionDetected)
        {
            return;
        }
        

        if (player)
        {
            rumba.HP -=1;
            player.stateMachine.changeState(player.playerJumpOnRumbaState);
        }   
    }
}
