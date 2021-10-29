using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PortCapitanUI : MonoBehaviour
{
    PortCapitan portCapitan;

    public GameObject item;
    public Transform holder;

    public TMP_Text buttonText;

    public void Init()
    {
        portCapitan = GetComponentInParent<PortCapitan>();
    }

    public void Redraw()
    {
        foreach (Transform item in holder.transform)
        {
            Destroy(item.gameObject);
        }
        int id = 0;
        foreach (var it in portCapitan.needs)
        {
            var n = Instantiate(item, holder);
            n.GetComponentInChildren<RawImage>().texture = ResourcesManager.instance.itemsAbstract[it.abstractID].icon.texture;
            n.GetComponentInChildren<TMP_Text>().text = $"{ResourcesManager.instance.itemsAbstract[it.abstractID].name}\n{it.value}/{it.needvalue}";
            var kn = id;
            n.GetComponent<Button>().onClick.AddListener(new UnityEngine.Events.UnityAction(delegate { portCapitan.AddItem(kn); }));

            id++;
        }
        buttonText.text = "Sell for " + portCapitan.reward;
    }
}
