using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class StorageMoveItemUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public float time = 0.5f, wait = 1;
    public bool isDown;
    public int resourceID = -1;
    public TMP_Text valueText;
    private void Update()
    {
        if (isDown)
        {
            time += Time.deltaTime;
            if (time > wait)
            {
                MoveItem();
                time = 0;
            }
        }
    }

    public void MoveItem()
    {
        foreach (var pl in PlayersManager.instance.players)
        {
            if (pl.move != null)
            {
                var item = pl.stack.GetStack().FindIndex(x => x.resourceID == resourceID);
                if (item != -1)
                {
                    var removed = pl.stack.RemoveFromStack(item, true);
                    var stack = GetComponentInParent<StackManager>();
                    stack.AddInStack(removed.transform, false);
                    int size = 8;
                    removed.localPos = new Vector3(0, (stack.GetStack().Count / size) * stack.increment, (stack.GetStack().Count % size) * stack.increment);
                    GetComponentInParent<StorageBackpackUI>().UpdateUI(true);
                    wait -= Time.deltaTime * 2;
                }
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        MoveItem();
        isDown = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        time = 0.5f;
        wait = 1;
        GetComponentInParent<StorageBackpackUI>().UpdateUI();
        isDown = false;
    }
}
