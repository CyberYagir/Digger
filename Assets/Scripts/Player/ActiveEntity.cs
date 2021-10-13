using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEntity {
    public void Mine(StackManager stackManager);
    public EntityParent getSelf();
}


public class ActiveEntity : MonoBehaviour
{
    List<IEntity> entities = new List<IEntity>();

    public void AddEntity(IEntity entity)
    {
        entities.Add(entity);
    }
    public IEntity GetCurrentEntity(bool findEmpty = false)
    {
        if (entities.Count != 0)
        {
            if (findEmpty == false)
            {
                return entities[0];
            }
            else
            {
                return entities.Find(x => x.getSelf().miner == null || x.getSelf().miner == this);
            }
        }

        return null;
    }
    public void RemoveEntity(IEntity entity)
    {
        entities.RemoveAll(x => x == entity || x == null);
    }


    private void OnDrawGizmos()
    {
        foreach (var item in entities)
        {
            Gizmos.DrawLine(item.getSelf().transform.position, transform.position);
        }
    }
}
