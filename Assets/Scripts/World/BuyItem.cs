using System;
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


public class BuyItem : ActionCircle
{
    public List<ValueItem> items;
    public List<ByItemStack> stack;
    [ReadOnly] public int stackCount;
    [SerializeField] int maxLength;
    [SerializeField] float interval = 0.25f;
    [SerializeField] Transform stackTransform;
    [SerializeField] bool triggered, waitForExit;

    public event Action ItemsChangeEvent = delegate { };

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
        if (triggered)
        {
            if (time >= 0.15f && waitForExit == false && spawn == false)
            {
                if (items.FindAll(x => x.value <= 0).Count != items.Count)
                {
                    for (int i = 0; i < items.Count; i++)
                    {
                        if (IsValidDropPoint(items[i].itemID))
                        {
                            var removed = ResoucesManager.instance.RemoveItem(items[i].itemID, 1);
                            items[i].value -= removed.Count;
                            foreach (var item in removed)
                            {
                                item.transform.parent = stackTransform;
                                SetDropPos(item);
                            }
                            ItemsChangeEvent();
                            //StartCoroutine(moveDrop(removed));
                            time = 0;
                        }
                    }
                }
                else
                {
                    time = 0;
                    spawn = true;
                }
            }
        }
        if (time > 1 && spawn)
        {
            end.Invoke();
            SetEmpty();
            waitForExit = true;
            time = 0;
            spawn = false;
        }
    }
    public bool IsValidDropPoint(int resourceID)
    {
        return (stack.Find(x => x.drop == null && x.resID == resourceID) != null);
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

    private void OnDrawGizmos()
    {
        return;
        var stackCount = items.Sum(x => x.value);
        int y = 0;
        int xCount = 0;
        for (int i = 0; i < items.Count; i++)
        {
            for (int x = 0; x < items[i].value; x++)
            {
                Gizmos.DrawWireCube(stackTransform.TransformPoint(new Vector3(0, y * interval, xCount * interval)), new Vector3(1, 1, 2));
                xCount++;
                if (xCount > maxLength)
                {
                    xCount = 0;
                    y++;

                }
            }
        }
    }
}
