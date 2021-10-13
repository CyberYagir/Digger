using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[System.Serializable]
public class NameData {
    public string matName;
    public float entityID;
}
[System.Serializable]
public class MineEntity {
    public float entityID;
    public StackManager stackManager;
}

public class EntityManager : MonoBehaviour
{
    public static IEnumerator ScanAsync;
    public static EntityManager entityManager;
    public List<Entity> entities;
    public List<MineEntity> mined;
    private void Awake()
    {
        entityManager = this;
    }
    public void SetMined(Entity entity, StackManager miner)
    {
        mined.RemoveAll(x => x.entityID == entity.uniqID);
        mined.Add(new MineEntity() { entityID = entity.uniqID, stackManager = miner });
    }

    public void ResetData()
    {

        mined = new List<MineEntity>();
        entities.RemoveAll(x => x == null);
    }
    public void ScanAction()
    {
        if (ScanAsync == null)
        {
            ScanAsync = Scan();
            StartCoroutine(ScanAsync);
        }
    }
    public IEnumerator Scan()
    {
        foreach (Progress progress in AstarPath.active.ScanAsync())
        {
            yield return null;
        }
        ScanAsync = null;
    }

}
