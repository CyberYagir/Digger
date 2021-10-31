using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortCapitan : MonoBehaviour
{
    [System.Serializable]
    public class Need
    {
        public int abstractID;
        public int needvalue, value;
    }

    public List<Need> needs = new List<Need>();
    public int reward = 2;
    public int questCount = 0;


    public event System.Action ChangeNeed = delegate { };


    private void Start()
    {
        questCount = 0;
        ChangeNeed = delegate { };
        GetComponentInChildren<PortCapitanUI>(true).Init();
        ChangeNeed += delegate { GetComponentInChildren<PortCapitanUI>(true).Redraw(); };
        ChangeNeed += delegate { StatsUI.instance.Redraw(); };

        if (needs.Count == 0)
            CreateQuest();
    }
    public void AddItem(int needID)
    {
        var item = ResourcesManager.instance.itemsAbstract[needs[needID].abstractID];
        var val = item.value;
        for (int i = 0; i < val; i++)
        {
            if (item.value > 0 && needs[needID].value < needs[needID].needvalue)
            {
                item.value--;
                needs[needID].value++;
            }
            else
            {
                break;
            }
        }
        ChangeNeed();
    }

    public void CreateQuest()
    {
        questCount++;
        needs = new List<Need>();
        var items = ResourcesManager.instance.itemsAbstract;
        for (int i = 1; i < items.Count; i++)
        {
            if (needs.Count == 0 || Random.Range(1, 3) == 2)
            {
                needs.Add(new Need() { abstractID = i, needvalue = Random.Range(2, 5) * questCount });
            }
        }
        reward = (Random.Range(2, 5) * questCount) + reward;
        ChangeNeed();
    }

    public void FinalQuest()
    {
        foreach (var item in needs)
        {
            if (item.value < item.needvalue)
            {
                return;
            }
        }
        ResourcesManager.instance.AddToAbstract("Coin", reward);
        CreateQuest();
    }


}
