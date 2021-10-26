using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartsManager : MonoBehaviour
{
    public static PartsManager instance;
    public Rigidbody[] rigidbodies;

    private void Awake()
    {
        instance = this;
    }

    private void FixedUpdate()
    {
        rigidbodies = GetComponentsInChildren<Rigidbody>();
        foreach (var item in rigidbodies)
        {
            if (item.transform.position.y < -5)
            {
                Destroy(item.gameObject);
                break;
            }
            else
            {
                var it = item.GetComponent<RayfireDropPart>();
                if (it == null)
                {
                    var cl = item.GetComponent<Collider>();
                    cl.material.name = cl.material.name.Replace("(Instance)", "").Trim();
                    item.gameObject.AddComponent<RayfireDropPart>().cl = cl;
                }
            }
        }
        
    }

    private void LateUpdate()
    {
        if (rigidbodies.Length == 0)
        {
            foreach (var item in LandsManager.instance.activeLands)
            {
                LandRegenerator.instance.Regen(new Vector3Int(item.arrayPos.x, 0, item.arrayPos.y)*50);
            }
        }
    }
}
