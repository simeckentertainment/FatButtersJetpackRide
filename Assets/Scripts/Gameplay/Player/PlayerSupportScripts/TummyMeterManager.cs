using UnityEngine;

public class TummyMeterManager : MonoBehaviour
{
    [SerializeField] Player player;
    SaveManager saveManager;
    public Sprite tum100;
    public Sprite tum75;
    public Sprite tum50;
    public Sprite tum25;
    public Sprite tum0;
    public Sprite tumnull;

    [SerializeField] SpriteRenderer[] GuageTummies;

    [SerializeField] int maxTummy; //maximum possible tummy is 80
    [SerializeField] int currentTummy;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(saveManager == null){
            saveManager = Helper.NabSaveData().GetComponent<SaveManager>();
        }
        maxTummy = saveManager.collectibleData.treatsUpgradeLevel;
        currentTummy = maxTummy;
    }

    // Update is called once per frame
    void Update()
    {
        currentTummy = (int)player.tummy;
    }



    void SetMaxTummies(){
        foreach(SpriteRenderer tummy in GuageTummies){
            tummy.enabled = false;
        }

        for(int i=0;i<maxTummy;i++){
            
        }
    }



}
