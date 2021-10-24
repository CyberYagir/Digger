using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EntityType { 
    Wood, Rock
}

public class EntityParent : MonoBehaviour, IEntity {

    [ReadOnly] public float uniqID;
    public EntityType entityType;
    public EntityParent getSelf() => this;


    protected List<ActiveEntity> activeEntities = new List<ActiveEntity>();

    public ActiveEntity miner;
    [SerializeField] public List<RayFire.RayfireRigid> parts;

    public void Init()
    {
        uniqID = Random.Range(-9999f, 9999f);
        var json = JsonUtility.ToJson(new NameData() { entityID = uniqID, matName = entityType.ToString() });
        for (int i = 0; i < parts.Count; i++)
        {
            parts[i].meshDemolition.seed = Random.Range(0, 50);
            parts[i].physics.material = new PhysicMaterial(json);
            parts[i].physics.material.frictionCombine = PhysicMaterialCombine.Maximum;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var act = other.GetComponent<ActiveEntity>();
        if (act != null)
        {
            act.AddEntity(this);
            activeEntities.Add(act);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var act = other.GetComponent<ActiveEntity>();
        if (act != null)
        {
            if (miner != null)
            {
                if (miner == act)
                {
                    miner = null;
                }
            }
            activeEntities.Remove(act);
            activeEntities.RemoveAll(x => x == null);
            act.RemoveEntity(this);
        }
    }

    public virtual void Mine(StackManager stackManager)
    {
    }
}

public class Entity : EntityParent
{

    private void Start()
    {
        Init();
        EntityManager.entityManager.entities.Add(this);
    }
    public override void Mine(StackManager stackManager)
    {
        base.Mine(stackManager);
        if (parts.Count > 0)
        {
            miner = stackManager.GetComponent<ActiveEntity>();
            parts[0].Demolish();
            parts.RemoveAt(0);
        }
        if (parts.Count == 0)
        {

            EntityManager.entityManager.SetMined(this, stackManager); //Выход их тупика
            for (int i = 0; i < activeEntities.Count; i++)
            {
                activeEntities[i].RemoveEntity(this);
            }
            Destroy(gameObject);
            EntityManager.entityManager.ScanAction();
        }
    }

   
}
