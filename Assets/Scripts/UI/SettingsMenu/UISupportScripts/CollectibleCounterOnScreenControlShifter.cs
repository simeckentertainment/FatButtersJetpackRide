using UnityEngine;

public class CollectibleCounterOnScreenControlShifter : MonoBehaviour
{
    [SerializeField] RectTransform rt;
    Vector3 referencePos;
    public CollectibleData cd;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        referencePos = rt.localPosition;
    }
    // Update is called once per frame
    void Update()
    {
        rt.localPosition = cd.OnScreenControlsEnabled ? new Vector3(referencePos.x+150f,referencePos.y,referencePos.z) : referencePos;
    }
}
