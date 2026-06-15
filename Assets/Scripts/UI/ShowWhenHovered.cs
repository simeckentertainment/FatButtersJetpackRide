using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShowWhenHovered : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private List<GameObject> show;
    [SerializeField] private List<GameObject> hide;

    private void Start()
    {
        SetObjectsEnabled(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SetObjectsEnabled(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SetObjectsEnabled(false);
    }

    private void SetObjectsEnabled(bool elementHovered)
    {
        foreach (var obj in show)
        {
            obj.SetActive(elementHovered);
        }

        foreach (var obj in hide)
        {
            obj.SetActive(!elementHovered);
        }
    }
}
