using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackManager : MonoBehaviour
{
    List<Drop> stack = new List<Drop>();
    [SerializeField] Transform stackBackPack;
    public float increment;

    [SerializeField] bool isTransparent = false;
    public static event System.Action AddToInventory = delegate { };
    private void Update()
    {
        if (Physics.Raycast(Camera.main.transform.position, transform.position - Camera.main.transform.position, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("Player")))
        {
            if (hit.collider != null)
            {
                bool transparent = false;
                if (hit.collider.GetComponentInParent<Drop>())
                {
                    transparent = true;
                }

                isTransparent = transparent;
            }
        }
        foreach (var item in stack)
        {
            var r = item.GetComponentInChildren<Renderer>();
            r.material.color = Color.Lerp(r.material.color, new Color(r.material.color.r, r.material.color.g, r.material.color.b, isTransparent ? 0.2f : 1), 2 * Time.deltaTime);
        }
    }
    public List<Drop> GetStack()
    {
        return stack;
    }
    public int GetID(Drop drop)
    {
        return stack.FindIndex(x => x == drop);
    }

    public void AssingNewPositions()
    {
        for (int i = 0; i < stack.Count; i++)
        {
            stack[i].localPos = GetPos(i);
        }
    }
    public Vector3 AddInStack(Transform obj, bool startEvent = true)
    {
        obj.transform.parent = stackBackPack;
        var drp = obj.GetComponent<Drop>();
        drp.stackManager = this;
        stack.Add(drp);
        ResoucesManager.instance.UpdateResources();
        StatsUI.instance.Redraw();
        if (startEvent)
            AddToInventory();
        return GetPos(stack.Count - 1);
    }
    public Drop RemoveFromStack(int id = 0, bool reAssign = false)
    {
        var drp = stack[id];
        stack.RemoveAt(id);
        if (reAssign)
        {
            AssingNewPositions();
        }
        AddToInventory();
        return drp;
    }
    public Vector3 GetPos(int id)
    {
        if (id % 2 == 0)
        {
            return new Vector3(0, 0, id * increment);
        }
        else
        {
            return new Vector3(0, -increment, ((id - 1) * increment) - increment / 2f);
        }
    }
}
