using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StorageBackpackUI : MonoBehaviour
{
    [SerializeField] Transform holderBackpack, holderInStorage;
    [SerializeField] Transform item;

    private void Start()
    {
        StackManager.AddToInventory += delegate { UpdateUI(); };
    }

    public void UpdateUI(bool dontUpdateButtons = false)
    {
        List<ItemCounter> valueItems = new List<ItemCounter>();
        var players = PlayersManager.instance.players;

        foreach (var pl in players)
        {
            if (pl.move != null)
            {
                foreach (var it in pl.stack.GetStack())
                {
                    var fItem = valueItems.Find(x => x.id == it.resourceID);
                    if (fItem == null)
                    {
                        valueItems.Add(new ItemCounter() { value = 1, id = it.resourceID, icon = ItemsManager.instance.items[it.resourceID].icon });
                    }
                    else
                    {
                        fItem.value++;
                    }
                }
            }
        }
        if (GetComponentsInChildren<StorageMoveItemUI>().ToList().FindAll(x => x.isDown).Count == 0)
        {

            foreach (Transform item in holderBackpack)
            {
                Destroy(item.gameObject);
            }
            for (int i = 0; i < valueItems.Count; i++)
            {
                var it = Instantiate(item.gameObject, holderBackpack);
                it.GetComponentInChildren<Image>().sprite = valueItems[i].icon;
                it.GetComponentInChildren<TMP_Text>().text = valueItems[i].value.ToString();
                it.GetComponentInChildren<StorageMoveItemUI>().resourceID = valueItems[i].id;
            }
        }

        if (dontUpdateButtons)
        {
            foreach (var item in GetComponentsInChildren<StorageMoveItemUI>())
            {
                var i = valueItems.Find(x => x.id == item.resourceID);
                if (i != null)
                {
                    item.valueText.text = i.value.ToString();
                    if (i.value == 0)
                    {
                        UpdateUI();
                        item.transform.parent.gameObject.SetActive(false);
                        continue;
                    }
                }
                else
                {
                    UpdateUI();
                    item.transform.parent.gameObject.SetActive(false);
                    continue;
                }
            }
        }

        foreach (Transform b in holderInStorage)
        {
            Destroy(b.gameObject);
        }

        List<ItemCounter> stackItems = new List<ItemCounter>();

        foreach (var it in GetComponentInParent<StackManager>().GetStack())
        {
            var fItem = stackItems.Find(x => x.id == it.resourceID);
            if (fItem == null)
            {
                stackItems.Add(new ItemCounter() { value = 1, id = it.resourceID, icon = ItemsManager.instance.items[it.resourceID].icon });
            }
            else
            {
                fItem.value++;
            }
        }

        for (int i = 0; i < stackItems.Count; i++)
        {
            var it = Instantiate(item.gameObject, holderInStorage);
            it.GetComponentInChildren<Image>().sprite = stackItems[i].icon;
            it.GetComponentInChildren<TMP_Text>().text = stackItems[i].value.ToString();
            it.GetComponentInChildren<StorageMoveItemUI>().gameObject.SetActive(false);
        }
    }
}
