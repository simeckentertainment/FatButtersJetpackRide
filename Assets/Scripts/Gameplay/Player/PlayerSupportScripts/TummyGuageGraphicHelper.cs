using UnityEngine;
using UnityEngine.UI;

public class TummyGuageGraphicHelper : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] Image image;
    [SerializeField] Image shineImage;
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

    void Start()
    {
        maxTummy = SaveManager.Instance.collectibleData.treatsUpgradeLevel;
        if(maxTummy == 0){maxTummy++;}
        currentTummy = maxTummy;
        SetMaxTummyGraphics();
    }

    void FixedUpdate()
    {
        currentTummy = (int)player.tummy;
        SetCurrentTummyGraphics();
    }

    void SetCurrentTummyGraphics()
    {
        if(currentTummy <= emptyAmount)
        {
            image.sprite = tum0;
        }
        if(currentTummy == quarterAmount)
        {
            image.sprite = tum25;
        }
        if(currentTummy == halfAmount)
        {
            image.sprite = tum50;
        }
        if(currentTummy == threeQuarterAmount)
        {
            image.sprite = tum75;
        }
        if(currentTummy >= fullAmount)
        {
            image.sprite = tum100;
        }
    }

    void SetMaxTummyGraphics()
    {
        if(maxTummy<=emptyAmount)
        {
            image.sprite = tumnull;
            image.enabled = false;
            shineImage.enabled = false;
        }
        if(maxTummy == quarterAmount)
        {
            image.sprite = tum25;
            image.enabled = true;
            shineImage.enabled = true;
        }
        if(maxTummy == halfAmount)
        {
            image.sprite = tum50;
            image.enabled = true;
            shineImage.enabled = true;
        }
        if(maxTummy == threeQuarterAmount)
        {
            image.sprite = tum75;
            image.enabled = true;
            shineImage.enabled = true;
        }
        if(maxTummy >= fullAmount)
        {
            image.sprite = tum100;
            image.enabled = true;
            shineImage.enabled = true;
        }
    }
}
