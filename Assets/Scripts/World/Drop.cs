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
    private void Start()
    {
        var n = EntityManager.entityManager.mined.Find(x => x.entityID == entityID);
        if (n != null)
        {
            stackManager = n.stackManager;
        }
        else
        {
            Destroy(gameObject);
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
                GetComponentInChildren<Collider>().gameObject.layer = LayerMask.NameToLayer("Player");
            }
            else
            {
                transform.localPosition = Vector3.Slerp(transform.localPosition, localPos, 4 * Time.deltaTime);
                transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(0,0,0), 4 * Time.deltaTime);
            }
        }
    }
}
