using UnityEngine;
using UnityEngine.UI;

public class ViewScroller : MonoBehaviour
{
    [SerializeField] private ScrollRect scrollRect;

    public void ScrollToElement(RectTransform transform)
    {
        scrollRect.verticalNormalizedPosition = 5; //?
        // need to detect when something is outside the viewport and then make it barely inside the viewport
    }
}