using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drop : MonoBehaviour
{
    float time;
    public Vector3 localPos;
    public StackManager stackManager;
    public float entityID;
    public int resourceID = -1;
    [HideInInspector]
    public bool dontDestroy;
    private void Start()
    {
        var n = EntityManager.entityManager.mined.Find(x => x.entityID == entityID);
        if (n != null)
        {
            stackManager = n.stackManager;
        }
        else
        {
            if (!dontDestroy)
            {
                Destroy(gameObject);
            }
        }
    }
    public void SetLayer(int layer)
    {
        foreach (var item in GetComponentsInChildren<Renderer>())
        {
            item.gameObject.layer = layer;
        }
        foreach (var item in GetComponentsInChildren<Collider>())
        {
            item.isTrigger = true;
        }
    }


    private void Update()
    {
        time += Time.deltaTime;
        if (time > 1)
        {
            if (transform.parent == null)
            {
                localPos = stackManager.AddInStack(transform);
                Destroy(GetComponent<Rigidbody>());
                GetComponentInChildren<Collider>().isTrigger = true;
                GetComponentInChildren<Collider>().gameObject.layer = stackManager.gameObject.layer;
            }
            else
            {
                transform.localPosition = Vector3.Slerp(transform.localPosition, localPos, 4 * Time.deltaTime);
                transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(0,0,0), 4 * Time.deltaTime);
            }
        }
    }
}
