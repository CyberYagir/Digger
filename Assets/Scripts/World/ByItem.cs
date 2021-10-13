using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class ValueItem {
    public int itemID;
    public int value;
    [HideInInspector]
    public int maxValue;
}
[System.Serializable]
public class ByItemStack {
    public int resID;
    public Vector3 pos;
    public Transform preview;
    public Transform drop;
}


public class ByItem : ActionCircle
{
    public List<ValueItem> items;
    public List<ByItemStack> stack;
    [ReadOnly] public int stackCount;
    [SerializeField] int maxLength;
    [SerializeField] float interval = 0.25f;
    [SerializeField] Transform stackTransform;
    [SerializeField] bool triggered, waitForExit;

    [SerializeField] UnityEvent end;

    float time;
    private void Start()
    {
        foreach (var item in items)
        {
            item.maxValue = item.value;
        }
        SetPreview();
    }


    public void SetPreview()
    {

        stackCount = items.Sum(x => x.value);
        int y = 0;
        int xCount = 0;
        for (int i = 0; i < items.Count; i++)
        {

            for (int x = 0; x < items[i].value; x++)
            {
                var n = Instantiate(ItemsManager.GetItem(items[i].itemID).prefab, Vector3.zero, Quaternion.Euler(Vector3.zero), stackTransform);
                Destroy(n.GetComponent<Rigidbody>());
                Destroy(n.GetComponent<Drop>());
                n.transform.parent = stackTransform;
                n.transform.localEulerAngles = Vector3.zero;
                var pos = new Vector3(0, y * interval, xCount * interval);
                n.transform.localPosition = pos;
                n.GetComponentInChildren<Renderer>().material.color *= new Color(1, 1, 1, 0.2f);

                stack.Add(new ByItemStack() { pos = pos, resID = items[i].itemID, preview = n.transform });

                xCount++;
                if (xCount > maxLength)
                {
                    xCount = 0;
                    y++;

                }
            }
        }
    }

    public void SetEmpty()
    {
        foreach (var item in items)
        {
            item.value = item.maxValue;
        }
        for (int i = 0; i < stack.Count; i++)
        {
            stack[i].preview.gameObject.SetActive(true);
            Destroy(stack[i].drop.gameObject);
        }
    }
    bool spawn;
    private void Update()
    {
        time += Time.deltaTime;
        print(spawn);
        if (spawn)
        {
            if (time > 2)
            {
                end.Invoke();
                SetEmpty();
                waitForExit = true;
                time = 0;
                spawn = false;
            }
        }
        if (triggered)
        {
            if (time >= 1 && waitForExit == false && spawn == false)
            {
                if (items.FindAll(x => x.value <= 0).Count != items.Count)
                {
                    for (int i = 0; i < items.Count; i++)
                    {
                        var removed = ResoucesManager.instance.RemoveItem(items[i].itemID, items[i].value);
                        items[i].value -= removed.Count;
                        StartCoroutine(moveDrop(removed));
                        time = 0;
                        spawn = true;
                    }
                }                
            }
        }
        else
        {
            time = 0;
        }
    }
    IEnumerator moveDrop(List<Drop> removed)
    {
        foreach (var item in removed)
        {
            yield return new WaitForSeconds(1f/(float)removed.Count);
            item.transform.parent = stackTransform;
            SetDropPos(item);
        }
    }

    public void SetDropPos(Drop drop)
    {
        var emptyPoint = stack.Find(x => x.drop == null && x.resID == drop.resourceID);
        drop.localPos = emptyPoint.pos;
        emptyPoint.drop = drop.transform;
        emptyPoint.preview.gameObject.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<Player>())
        {
            time = 0;
            triggered = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        waitForExit = false;
        triggered = false;
    }
}
