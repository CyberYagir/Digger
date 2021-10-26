using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingStatsUI : MonoBehaviour
{
    [SerializeField] GameObject resourcePrefab;
    [SerializeField] BuyItem buyItem;

    private void Awake()
    {
        buyItem.ItemsChangeEvent += delegate { Redraw(); };
        Redraw();
    }
    public void Redraw()
    {
        foreach (Transform item in transform)
        {
            Destroy(item.gameObject);
        }
        var sort = buyItem.items.OrderBy(x => x.itemID).ToList();
        foreach (var it in sort)
        {
            var line = Instantiate(resourcePrefab, transform);
            line.name = $"Res [{it.itemID}]";
            line.GetComponentInChildren<RawImage>().texture = ItemsManager.GetItem(it.itemID).icon.texture;
            line.GetComponentInChildren<TMP_Text>().text = it.value.ToString();
        }
    }
}
