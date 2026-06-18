using UnityEngine;
using UnityEngine.UI;

public class ViewScroller : MonoBehaviour
{
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private float padding = 20;

    private void Start()
    {
        if (scrollRect == null)
        {
            scrollRect = GetComponent<ScrollRect>();
        }

        if (scrollRect == null)
        {
            Debug.LogError("No referenced ScrollRect component on ViewScroller");
        }

        var selectableChildren = transform.GetComponentsInChildren<Selectable>();
        foreach(var child in selectableChildren)
        {
            var scrollHandler = child.GetComponent<ScrollToItem>();
            if (scrollHandler == null)
            {
                scrollHandler = child.gameObject.AddComponent<ScrollToItem>();
            }
            scrollHandler.scrollView = this;
        }
    }

    public void ScrollToElement(RectTransform transform)
    {
        Vector2 scrollPosition = scrollRect.content.position;
        
        var viewportCorners = new Vector3[4];
        scrollRect.viewport.GetWorldCorners(viewportCorners);
        var visibleContentTop = viewportCorners[1].y - padding; // top left corner
        var visibleContentBottom = viewportCorners[0].y + padding; // bottom left corner

        var elementCorners = new Vector3[4];
        transform.GetWorldCorners(elementCorners);
        var elementTop = elementCorners[1].y; // top left corner
        var elementBottom = elementCorners[0].y;  // bottom left corner

        if (elementTop > visibleContentTop)
        {
            // scroll up
            scrollPosition.y += visibleContentTop - elementTop;
        }

        else if (elementBottom < visibleContentBottom)
        {
            // scroll down
            scrollPosition.y -= elementBottom - visibleContentBottom;
        }

        scrollRect.content.position = scrollPosition;
    }
}