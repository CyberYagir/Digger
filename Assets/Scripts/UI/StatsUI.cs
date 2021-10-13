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
    private void Awake()
    {
        instance = this;
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
            var line = Instantiate(resourcePrefab, transform);
            line.name = $"Res [{it.id}]";
            line.GetComponentInChildren<RawImage>().texture = it.icon.texture;
            line.GetComponentInChildren<TMP_Text>().text = it.value.ToString();
        }
    }
}
