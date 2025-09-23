using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatRunner : MonoBehaviour
{
    public List<Vector3> path;
    [SerializeField] int pathCheckpoint;
    public Vector3 endPoint;
    [SerializeField] public AudioSource screech;
    [SerializeField] public AudioClip[] BatSounds;
    [SerializeField] int timer = 0;
    [SerializeField] public int timerEnd = 10;
    Vector3[] SlerpAroundCoords;
    bool slerpIt;
    // Start is called before the first frame update
    void Start()
    {
        screech.clip = BatSounds[Random.Range(0,2)];
        pathCheckpoint = 0;
        SlerpAroundCoords = GetSlerpCoords();
    }

    // Update is called once per frame
    void Update()
    {
        if(timer < timerEnd){
            //if(slerpIt){
                //transform.position = Vector3.Slerp(SlerpAroundCoords[0],SlerpAroundCoords[1],Helper.RemapToBetweenZeroAndOne(0,timerEnd,timer));
            //}else{
                transform.position = Vector3.Lerp(path[pathCheckpoint],path[pathCheckpoint+1],Helper.RemapToBetweenZeroAndOne(0,timerEnd,timer));
            //}
            timer +=1;
            transform.LookAt(path[pathCheckpoint+1]);
        } else {
            timer = 0;
            pathCheckpoint +=1;
            //SlerpAroundCoords = GetSlerpCoords();
        }
        if(pathCheckpoint == path.Count-1){
            screech.Stop();
            Destroy(gameObject);
        }
    }

    Vector3[] GetSlerpCoords(){
        Vector3 thisPos = path[pathCheckpoint];
        Vector3 nextPos = path[pathCheckpoint+1];
        Vector3 midPos = (nextPos+thisPos)/2.0f;
        //Vector3 leftPos = new Vector3(midPos.x-1.0f,midPos.y,midPos.z);
        //Vector3 rightPos = new Vector3(midPos.x+1.0f,midPos.y,midPos.z);
        //if(Vector3.Distance(transform.position,leftPos) < Vector3.Distance(transform.position,rightPos) ){
            thisPos = thisPos + midPos;
            nextPos = nextPos + midPos;
            //slerpIt = true;
        //} else if( Vector3.Distance(transform.position,leftPos) > Vector3.Distance(transform.position,rightPos)){
            //thisPos = thisPos + midPos;
            //nextPos = nextPos + midPos;
            //slerpIt = true;
        //} else {
            //slerpIt = false;
        //}
        Vector3[] returnPos = {thisPos,nextPos};
        return returnPos;

    }



}
