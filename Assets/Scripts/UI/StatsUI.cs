using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatsUI : MonoBehaviour
{
    [SerializeField] GameObject resourcePrefab;
    public static StatsUI instance;

    GameDataObject data;
    private void Awake()
    {
        instance = this;
        data = GameDataObject.GetData();
    }
    public void Redraw()
    {
        foreach (Transform item in transform)
        {
            Destroy(item.gameObject);
        }
        var sort = ResoucesManager.instance.valueItems.OrderBy(x => x.id).ToList();
        foreach (var it in sort)
        {
            SpawnItem(it);
        }

        var sortAbst = ResoucesManager.instance.itemsAbstract.OrderBy(x => x.value).ToList();
        foreach (var it in sortAbst)
        {
            SpawnItem(it);
        }
    }


    void SpawnItem(ItemCounter it)
    {
        var line = Instantiate(resourcePrefab, transform);
        line.name = $"Res [{it.id}]";
        line.GetComponentInChildren<RawImage>().texture = it.icon.texture;
        line.GetComponentInChildren<TMP_Text>().text = it.value.ToString();
    }
    void SpawnItem(AbstractItem it)
    {
        var line = Instantiate(resourcePrefab, transform);
        line.name = $"Res [{it.name}[abstract]]";
        line.GetComponentInChildren<RawImage>().texture = it.icon.texture;
        line.GetComponentInChildren<TMP_Text>().text = it.value.ToString();
    }
}
