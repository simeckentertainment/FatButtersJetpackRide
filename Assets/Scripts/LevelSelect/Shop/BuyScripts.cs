using UnityEngine;

[RequireComponent(typeof(LevelSelectButtonManager))]
public class BuyScripts : MonoBehaviour
{
    [System.NonSerialized] LevelSelectButtonManager lsbm; //DO NOT TOUCH
    [System.NonSerialized] SaveManager sm; //DO NOT TOUCH
    [SerializeField] CollectibleData cd;


    private void Start(){
        lsbm = GetComponent<LevelSelectButtonManager>(); //preset
        sm = GameObject.FindAnyObjectByType<SaveManager>();
        lsbm.EnsureCorrectBuyButton();
    }
    public void RunBuy(int item){ //deferred from similiar method in lsbm for readability.
        switch(item){
            case 0: //Will always be fuel
                UpgradeFuel();
                break;
            case 1: //will always be thrust
                UpgradeThrust();
                break;

            case 2: //will always be tummy
                UpgradeTummy();
                break;
            default: //everything else
                BuyCurrentSkin();

                break;
        }

    }

    void BuyCurrentSkin(){
        int cost = lsbm.shopItems[lsbm.CurrentSelectedShopItem].itemPrice;
        if(cd.BONES < cost){return;}
        ReduceFunds(lsbm.shopItems[lsbm.CurrentSelectedShopItem].itemPrice);
        int skinNumber = lsbm.shopItems[lsbm.CurrentSelectedShopItem].SkinId;
        cd.HaveSkins[skinNumber] = true;
        lsbm.EnableSetSkinButton();
        sm.Save();
    }
    public void EnableCurrentSkin(){
        cd.CurrentSkin = lsbm.shopItems[lsbm.CurrentSelectedShopItem].SkinId;
        sm.Save();
        lsbm.EnableNoBuyOrSkinButton();
    }
    public void UpgradeFuel(){
        if(cd.BONES < cd.fuelUpgradeLevel){return;}
        ReduceFunds(cd.fuelUpgradeLevel);
        cd.fuelUpgradeLevel++;
        sm.Save();
        lsbm.SetCurrentItemCostText(cd.fuelUpgradeLevel);
    }
    public void UpgradeThrust(){
        if(cd.BONES < cd.thrustUpgradeLevel){return;}
        ReduceFunds(cd.thrustUpgradeLevel);
        cd.thrustUpgradeLevel++;
        sm.Save();
        lsbm.SetCurrentItemCostText(cd.thrustUpgradeLevel);
    }
    public void UpgradeTummy(){
        if(cd.BONES < cd.treatsUpgradeLevel){return;}
        ReduceFunds(cd.treatsUpgradeLevel);
        cd.treatsUpgradeLevel++;
        sm.Save();
        lsbm.SetCurrentItemCostText(cd.treatsUpgradeLevel);
    }

    public void ReduceFunds(int amount){
        cd.BONES -= amount;
    }
}
