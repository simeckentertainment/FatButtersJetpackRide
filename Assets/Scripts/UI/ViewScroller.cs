using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ViewScroller : MonoBehaviour
{
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private float padding = 20;

    public void ScrollToElement(RectTransform transform)
    {
        //scrollRect.verticalNormalizedPosition = 0; //?

        // #1

        //Canvas.ForceUpdateCanvases();

        //scrollRect.content.anchoredPosition =
        //        (Vector2)scrollRect.transform.InverseTransformPoint(scrollRect.content.position)
        //        - (Vector2)scrollRect.transform.InverseTransformPoint(transform.position);
        // need to detect when something is outside the viewport and then make it barely inside the viewport

        // #2

        //Canvas.ForceUpdateCanvases();
        //Vector2 viewportLocalPosition = scrollRect.viewport.position;
        //Vector2 childLocalPosition = transform.position;
        //Vector2 result = new Vector2(
        //    0 - (viewportLocalPosition.x + childLocalPosition.x),
        //    0 - (viewportLocalPosition.y + childLocalPosition.y)
        //);

        //scrollRect.content.position = result;

        // #3

        //Canvas.ForceUpdateCanvases();

        //var contentPos = (Vector2)scrollRect.transform.InverseTransformPoint(scrollRect.content.position);
        //var childPos = (Vector2)scrollRect.transform.InverseTransformPoint(transform.position);
        //var endPos = contentPos - childPos;
        //// If no horizontal scroll, then don't change contentPos.x
        //if (!scrollRect.horizontal) endPos.x = contentPos.x;
        //// If no vertical scroll, then don't change contentPos.y
        //if (!scrollRect.vertical) endPos.y = contentPos.y;

        //scrollRect.content.anchoredPosition = endPos;

        // #4

        //Debug.Assert(transform.parent == scrollRect.content,
        //"EnsureVisibility assumes that 'child' is directly nested in the content of 'scrollRect'");

        //float viewportHeight = scrollRect.viewport.rect.height;
        //Debug.Log("viewportHeight = " + viewportHeight);
        //Vector2 scrollPosition = scrollRect.content.position;
        //Debug.Log("scrollPosition = " + scrollPosition);

        //float elementTop = transform.position.y;
        //float elementBottom = elementTop - transform.rect.height;
        //Debug.Log("elementTop/Bottom = " + elementTop + "/" + elementBottom);

        //float visibleContentTop = -scrollPosition.y/* - padding*/;
        //float visibleContentBottom = -scrollPosition.y - viewportHeight/* + padding*/;
        //Debug.Log("visibleContentTop/Bottom = " + visibleContentTop + "/" + visibleContentBottom);

        //float scrollDelta =
        //    elementTop > visibleContentTop ? visibleContentTop - elementTop :
        //    elementBottom < visibleContentBottom ? visibleContentBottom - elementBottom :
        //    0f;
        //Debug.Log("scrollDelta = " + scrollDelta);

        //Debug.Log("BEFORE : scrollRect.content.anchoredPosition = " + scrollRect.content.anchoredPosition);
        //scrollRect.content.anchoredPosition += new Vector2(0, scrollDelta);
        ////scrollRect.content.anchoredPosition = scrollPosition;
        //Debug.Log("AFTER : scrollRect.content.anchoredPosition = " + scrollRect.content.anchoredPosition);

        // NOTHING WORKS3

        // #5

        //var pos = 1 - ((scrollRect.content.rect.height / 2 - transform.anchoredPosition.y) / scrollRect.content.rect.height);
        //scrollRect.verticalNormalizedPosition = pos;

        // #6
        Vector2 scrollPosition = scrollRect.content.position;
        
        //float visibleContentTop = scrollRect.viewport.position.y - padding;
        //float visibleContentBottom = visibleContentTop - (scrollRect.viewport.rect.height * scrollRect.viewport.lossyScale.y) + padding;

        var viewportCorners = new Vector3[4];
        scrollRect.viewport.GetWorldCorners(viewportCorners);
        float visibleContentTop = viewportCorners[1].y - padding; // top left corner
        float visibleContentBottom = viewportCorners[0].y + padding; // bottom left corner


        //float visibleContentTop = scrollRect.viewport.rect.yMin; 
        //float visibleContentBottom = scrollRect.viewport.rect.yMax;

        //

        //var scaledHeight = (transform.rect.height * transform.lossyScale.y);

        //float elementTop = transform.position.y + (scaledHeight / 2);
        //float elementBottom = transform.position.y - (scaledHeight / 2);

        var elementCorners = new Vector3[4];
        transform.GetWorldCorners(elementCorners);
        float elementTop = elementCorners[1].y; // top left corner
        float elementBottom = elementCorners[0].y;  // bottom left corner

        //float elementTop = transform.rect.yMin;
        //float elementBottom = transform.rect.yMax;

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