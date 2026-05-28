using System.Collections.Generic;
using UnityEngine;

public class EnableItemsWhenLevelIsUnlocked : MonoBehaviour
{
    [SerializeField] private int levelId;
    [SerializeField] private List<GameObject> items;

    private CollectibleData collectibleData => SaveManager.Instance.collectibleData;

    private void Start()
    {
        var levelBeaten = collectibleData.IsLevelUnlocked(levelId);

        foreach (var item in items)
        {
            item.gameObject.SetActive(levelBeaten);
        }
    }
}