using UnityEngine;
using UnityEngine.UI;

public abstract class ImageViewModel<T> : ViewModel<T> where T : Model
{
    [SerializeField] protected Image Image;

    protected override void Awake()
    {
        base.Awake();
        if (Image == null)
        {
            Image = GetComponent<Image>();
            if (Image == null)
            {
                Debug.LogError($"{name} unable to find an Image on this game object");
            }
        }
    }

    protected override void OnModelChanged()
    {
        Image.sprite = GetSprite();
    }

    protected virtual Sprite GetSprite()
    {
        return Image.sprite;
    }
}
