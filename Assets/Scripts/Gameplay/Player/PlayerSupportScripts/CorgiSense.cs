using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.AdaptivePerformance;
public class CorgiSense : MonoBehaviour
{
    [SerializeField] Transform playerTransform;
    [SerializeField] Transform Finish;
    [SerializeField] Sprite HaveFinishSprite;
    [SerializeField] Sprite NoFinishSprite;
    [SerializeField] Image SpriteHolder;
    [SerializeField] Transform HolderObj;
    [SerializeField] Transform uICanvas;
    [SerializeField] TMP_Text DistanceText;
    int dist;
    [SerializeField] bool haveFinish;
    [SerializeField] Vector3 GoalPos;
    // Start is called before the first frame update
    void Start()
    {
        GetFinish();

    }

    private void GetFinish(){
        try{
        Finish = GameObject.FindGameObjectWithTag("Finish").transform;
        } catch{
            haveFinish = false;
            return;
        }
        if (Finish == null){
            haveFinish = false;
            SpriteHolder.sprite = NoFinishSprite;
        }else{
            haveFinish = true;
            SpriteHolder.sprite = HaveFinishSprite;
        }
        GoalPos = new Vector3(Finish.transform.position.x,Finish.transform.position.y, uICanvas.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        if(haveFinish){
            AdjustCorgiSense();
            AdjustText();
        } else {
            GetFinish();
        }
    }

    private void AdjustCorgiSense(){
        float angle = Mathf.Rad2Deg * (Mathf.Atan2(GoalPos.y - uICanvas.position.y, GoalPos.x - uICanvas.position.x));
        HolderObj.rotation = Quaternion.Euler(new Vector3(0,0,angle));
    }
    private void AdjustText(){
        dist = ((int)Vector3.Distance(playerTransform.position, Finish.position));
        DistanceText.text = dist.ToString() + " ft.";
    }
}
