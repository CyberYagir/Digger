using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayfireDropPart : MonoBehaviour
{
    float time;
    public Collider cl;
    [SerializeField] NameData data;
    private void Start()
    {
        data = JsonUtility.FromJson<NameData>(cl.material.name);
    }
    private void FixedUpdate()
    {
        if (EntityManager.entityManager.entities.Find(x=>x.uniqID == data.entityID) == null)
        {
            time += Time.deltaTime;
            if (time > 1)
            {
                var findItem = ItemsManager.instance.items.Find(x => x.physicMaterial.name == data.matName);
                if (findItem != null)
                {
                    if (Random.Range(0, 3) == 1)
                    {
                        var drop = Instantiate(findItem.prefab, transform.position, transform.rotation);
                        drop.GetComponent<Drop>().entityID = data.entityID;
                        drop.GetComponent<Drop>().resourceID = ItemsManager.GetItemByMat(data.matName);
                        drop.GetComponent<Rigidbody>().isKinematic = false;
                    }
                    ParticlesPool.instance.GetFromPool(transform.position);
                    Destroy(gameObject);
                }
            }
        }
    }
}
