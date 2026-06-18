using UnityEngine;
using UnityEngine.EventSystems;

public class ScrollToItem : MonoBehaviour, ISelectHandler
{
    [SerializeField] private RectTransform element;
    [SerializeField] public ViewScroller scrollView;

    private void Start()
    {
        if (element == null)
        {
            element = GetComponent<RectTransform>();
        }

        if (scrollView == null)
        {
            scrollView = GetComponentInParent<ViewScroller>();
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (element == null)
        {
            Debug.LogError("Aborting attempt to select ScrollToItem with no RectTransform");
            return;
        }

        scrollView.ScrollToElement(element);
    }
}