using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class ItemCounter{
    public Sprite icon;
    public int id;
    public float value;
}


public class ResoucesManager : MonoBehaviour
{
    public List<ItemCounter> valueItems = new List<ItemCounter>();
    PlayersManager playersManager;

    public static ResoucesManager instance;


    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        playersManager = GetComponent<PlayersManager>();
    }
    public void UpdateResources()
    {
        valueItems = new List<ItemCounter>();
        foreach (var p in playersManager.players)
        {
            foreach (var item in p.stack.GetStack())
            {
                var findItem = valueItems.Find(x => x.id == item.resourceID);
                if (findItem == null)
                {
                    var it = ItemsManager.GetItem(item.resourceID);
                    valueItems.Add(new ItemCounter() { icon = it.icon, id = item.resourceID, value = 1});
                }
                else
                {
                    findItem.value++;
                }
            }
        }
    }

    public List<Drop> RemoveItem(int resID, int count)
    {
        List<Drop> removed = new List<Drop>();
        List<Drop> finded = new List<Drop>();
        foreach (var p in playersManager.players)
        {
            var stack = p.stack.GetStack();
            for (int i = 0; i < stack.Count; i++)
            {
                if (finded.Count < count)
                {
                    if (stack[i].resourceID == resID)
                    {
                        finded.Add(stack[i]);
                    }
                }
                else
                    break;
            }
        } 
        while (finded.Count != 0)
        {
            var rmv = finded[0].stackManager.RemoveFromStack(finded[0].stackManager.GetID(finded[0]));
            removed.Add(rmv);
            finded.RemoveAt(0);
        }
        foreach (var p in playersManager.players)
            p.stack.AssingNewPositions();

        if (removed.Count != 0)
        {
            ResoucesManager.instance.UpdateResources();
            StatsUI.instance.Redraw();
        }
        return removed;
    }
}
