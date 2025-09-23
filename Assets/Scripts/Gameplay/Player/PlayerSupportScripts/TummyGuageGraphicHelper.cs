using Unity.Mathematics;
using UnityEngine;

public class TummyGuageGraphicHelper : MonoBehaviour
{
    SaveManager saveManager;
    [SerializeField] Player player;
    [SerializeField] SpriteRenderer sr;
    public int emptyAmount;
    public Sprite tum0;
    public int quarterAmount;
    public Sprite tum25;
    public int halfAmount;
    public Sprite tum50;
    public int threeQuarterAmount;
    public Sprite tum75;
    public int fullAmount;
    public Sprite tum100;
    public Sprite tumnull;
    [SerializeField] int maxTummy; //maximum possible tummy is 80
    [SerializeField] int currentTummy;



    void Start(){
        if(saveManager == null){
            saveManager = Helper.NabSaveData().GetComponent<SaveManager>();
        }
        maxTummy = saveManager.collectibleData.treatsUpgradeLevel;
        if(maxTummy == 0){maxTummy++;}
        currentTummy = maxTummy;
        SetMaxTummyGraphics();
    }

    void FixedUpdate(){
        currentTummy = (int)player.tummy;
        SetCurrentTummyGraphics();
    }

    void SetCurrentTummyGraphics(){
        if(currentTummy <= emptyAmount){
            sr.sprite = tum0;
        }
        if(currentTummy == quarterAmount){
            sr.sprite = tum25;
        }
        if(currentTummy == halfAmount){
            sr.sprite = tum50;
        }
        if(currentTummy == threeQuarterAmount){
            sr.sprite = tum75;
        }
        if(currentTummy >= fullAmount){
            sr.sprite = tum100;
        }
    }





    void SetMaxTummyGraphics(){
        if(maxTummy<=emptyAmount){
            sr.sprite = tumnull;
            sr.enabled = false;
        }
        if(maxTummy == quarterAmount){
            sr.sprite = tum25;
            sr.enabled = true;
        }
        if(maxTummy == halfAmount){
            sr.sprite = tum50;
            sr.enabled = true;
        }
        if(maxTummy == threeQuarterAmount){
            sr.sprite = tum75;
            sr.enabled = true;
        }
        if(maxTummy >= fullAmount){
            sr.sprite = tum100;
            sr.enabled = true;
        }
    }
}
