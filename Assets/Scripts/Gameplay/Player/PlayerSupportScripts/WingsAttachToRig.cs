using UnityEngine;
using UnityEngine.Animations;

public class WingsAttachToRig : MonoBehaviour
{
    [SerializeField] PositionConstraint pc;
    Player player;
    [SerializeField] Animator anim;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject target = GameObject.Find("butters:corgi_spine1");
        pc.SetSources(new System.Collections.Generic.List<ConstraintSource>());
        ConstraintSource source = new ConstraintSource();
        source.sourceTransform = target.transform;
        source.weight = 1f;
        pc.AddSource(source);
        pc.constraintActive = true;
        pc.locked = true; // Optional: lock to prevent drifting
        player = GameObject.Find("Player").GetComponent<Player>();
        player.secondaryAnim = anim;


        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
