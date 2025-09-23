using Unity.Mathematics;
using UnityEngine;

public class CowboyRockHandler : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField]public Vector3 SpawnPoint;
    [SerializeField]public Vector3 Target;
    [SerializeField]public float dist;
    [SerializeField]public Vector3 midpoint;
    //No longer doing Slerp, it's too unpredictable. Instead we're just going with a Lerp and adjusting the Y value over the course of the toss.
    [System.NonSerialized] public float lifeSpanCounter = 0;
    [SerializeField]public float lifetimeMax;
    [SerializeField] public RockSpawner rockSpawner;
    [System.NonSerialized] public Vector3[] Apexes;
    

    // Update is called once per frame
    void Update(){
        RunTossCourse();
        // The rotation is taken care of like this for my game, but for yours you can have it face away from the previous frame's coordinates.
        transform.Rotate(Vector3.left);
        lifeSpanCounter++;
    }
    void OnCollisionEnter(){
        if (lifeSpanCounter > 5)
        {
            rockSpawner.SpawnedRock = null;
            rockSpawner.rockSpawned = false;
            //Destroy(gameObject);

            if (gameObject.GetComponent<Rigidbody>() == null){
                gameObject.AddComponent<Rigidbody>();
            }

        }
    }

    public Vector3[] CalculateApexes(){
        float highHeight = dist*0.4f;
        float lowheight = dist*0.3f;
        Vector3 nearMid = Midpoint(midpoint,SpawnPoint);
        Vector3 farMid = Midpoint(Target,midpoint);
        Vector3[] returnApexes = { 
            SpawnPoint,
            new Vector3(nearMid.x,nearMid.y+lowheight,nearMid.z),
            new Vector3(midpoint.x,midpoint.y+highHeight,midpoint.z),
            new Vector3(farMid.x,farMid.y+lowheight,farMid.z),
            Target
        };
        return returnApexes;
    }
    void RunTossCourse(){
        float TossCompletionPercent = lifeSpanCounter/lifetimeMax;
        if (TossCompletionPercent <= 0.25f)
        {
            float courseRan = RemapToBetweenZeroAndOne(Mathf.Epsilon, 0.25f, TossCompletionPercent);
            transform.position = Vector3.Lerp(Apexes[0], Apexes[1], courseRan);
        }
        else if (TossCompletionPercent > 0.25f & TossCompletionPercent <= 0.5f)
        {
            float courseRan = RemapToBetweenZeroAndOne(0.25f, 0.5f, TossCompletionPercent);
            transform.position = Vector3.Lerp(Apexes[1], Apexes[2], courseRan);
        }
        else if (TossCompletionPercent > 0.5f & TossCompletionPercent <= 0.75f)
        {
            float courseRan = RemapToBetweenZeroAndOne(0.5f, 0.75f, TossCompletionPercent);
            transform.position = Vector3.Lerp(Apexes[2], Apexes[3], courseRan);
        }
        else if (TossCompletionPercent > 0.75f & TossCompletionPercent <= 1.0f)
        {
            //if (gameObject.GetComponent<Rigidbody>() == null)
            //{
            //gameObject.AddComponent<Rigidbody>();
            //}
            float courseRan = RemapToBetweenZeroAndOne(0.75f, 1.0f, TossCompletionPercent);
            transform.position = Vector3.Lerp(Apexes[3], Apexes[4], courseRan);
        }
        else
        {
            rb.angularVelocity = (Apexes[4] - Apexes[3]).normalized;
            //if (gameObject.GetComponent<Rigidbody>() == null)
            //{
            //gameObject.AddComponent<Rigidbody>();
            //}
            //Destroy(gameObject);
        }
    }
    float RemapToBetweenZeroAndOne(float a,float b,float x){
        return math.remap(a,b,0.0f,1.0f,x);
    }
    public Vector3 Midpoint(Vector3 start,Vector3 finish){
        return (finish+start)/2.0f;
    }
    public Vector2 Midpoint(Vector2 start, Vector2 finish){
        return (finish+start)/2.0f;
    }
    public float Midpoint(float start, float finish){
        return (finish+start)/2.0f;
    }
}
