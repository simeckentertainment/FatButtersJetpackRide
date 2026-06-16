using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
//using UnityEngine.UIElements;

//public class ScrollToItem : Selectable
//{
//    [SerializeField] private Selectable element;
//    [SerializeField] private ViewScroller scrollView;

//    protected override void Start()
//    {
//        base.Start();

//        if (scrollView == null)
//        {
//            scrollView = GetComponentInParent<ViewScroller>();
//        }
//    }

//    public override void OnSelect(BaseEventData eventData)
//    {
//        base.OnSelect(eventData);

//        scrollView.ScrollToElement(transform);

//        element.OnSelect.AddListener(OnSelect);
//    }
//}

public class ScrollToItem : MonoBehaviour, ISelectHandler
{
    [SerializeField] private RectTransform element;
    [SerializeField] private ViewScroller scrollView;

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
        scrollView.ScrollToElement(element);
    }
}