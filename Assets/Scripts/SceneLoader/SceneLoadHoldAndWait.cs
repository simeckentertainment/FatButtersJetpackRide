using TMPro;
using UnityEngine;

public class SceneLoadHoldAndWait : MonoBehaviour
{
    [SerializeField] int framesToHold;
    [System.NonSerialized] int holdCounter;
    [SerializeField] SceneLoaderClass slc;
    [SerializeField] TMP_Text percentText;
    bool doneHolding;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start(){
        percentText.text = "0%";
        doneHolding = false;
    }

    // Update is called once per frame
    void Update(){
        holdCounter++;
        if(holdCounter >= framesToHold & !doneHolding){
            doneHolding = true;
            slc.enabled = true;
        }
    }
}
