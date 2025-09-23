using UnityEngine;
using UnityEngine.UI;
public class ShowCurrentSkinInStore : MonoBehaviour
{
    Image img;
    SaveManager sm;
    [SerializeField] LevelSelectButtonManager lsbm;
    [SerializeField] CollectibleData cd;
    [SerializeField] Sprite cantGetReadSprite;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sm = GameObject.FindAnyObjectByType<SaveManager>();
        img = GetComponent<Image>();
        int shopItemIndex = cd.CurrentSkin+3;
        if(lsbm.shopItems[shopItemIndex].dogHouseButtersImg != null){
            img.sprite = lsbm.shopItems[shopItemIndex].dogHouseButtersImg;
        } else{
            img.sprite = cantGetReadSprite;
        }
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}
