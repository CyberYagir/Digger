using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotMoving : MovebleObject
{
    [Space]
    public float minDistance;
    public float maxDistance;
    ActiveEntity activeEntity;
    bool moveToPlayer;
     AIDestinationSetter target;
    AIPath aiPath;
    [ReadOnly] public Transform targetTree;
    bool findOtherTree;
    private void Start()
    {
        Init();
        target = GetComponent<AIDestinationSetter>();
        aiPath = GetComponent<AIPath>();
        activeEntity = GetComponent<ActiveEntity>();
        AstarPath.active.Scan();
    }
    private void Update()
    {
        var playerPos = GameManger.player.transform.position;
        var distToPlayer = Vector3.Distance(playerPos, transform.position);

        if ((activeEntity.GetCurrentEntity() == null && distToPlayer < maxDistance && !moveToPlayer) || findOtherTree)
        {
            if (targetTree == null)
            {
                findOtherTree = false;
                var dst = 99999f;
                int id = -1;
                var e = EntityManager.entityManager.entities;
                for (int i = 0; i < e.Count; i++)
                {
                    if (e[i] != null)
                    {
                        var n = Vector3.Distance(e[i].transform.position, transform.position);
                        if (n < dst && n < maxDistance / 1.2f && e[i].miner == null)
                        {
                            dst = n;
                            id = i;
                        }
                    }
                }
                if (id != -1)
                {
                    targetTree = e[id].transform;
                }
            }
            else
            {
                var ent = targetTree.GetComponent<Entity>();
                if (ent.miner == null || ent.miner == activeEntity)
                {
                    Move(targetTree.transform);
                }
                else
                {
                    targetTree = null;
                }
            }
        }
        else if (distToPlayer > maxDistance)
        {
            targetTree = null;
            moveToPlayer = true;
        }

        if (targetTree != null)
        {
            var ent = targetTree.GetComponent<Entity>();
            if (ent.miner != null && ent.miner != activeEntity)
            {
                targetTree = null;
                findOtherTree = true;
            }
        }
        if (new Vector3(aiPath.velocity.x, 0, aiPath.velocity.z).magnitude > 0.01f)
        {
            run += Time.deltaTime * 2;
        }

        if (moveToPlayer)
        {
            if (distToPlayer < minDistance)
            {
                moveToPlayer = false;
            }
        }
        if ((!isMineTarget || distToPlayer > maxDistance) && distToPlayer > minDistance + 3 && targetTree == null || moveToPlayer)
        {
            if (!isMineTarget || distToPlayer > maxDistance || distToPlayer > minDistance)
            {
                moveToPlayer = true;
            }
            Move(GameManger.player.transform);
        }
        else
        {
            if (new Vector3(aiPath.velocity.x, 0, aiPath.velocity.z).magnitude <= 0.01f)
            {
                run -= Time.deltaTime * 2;
            }
        }
        run = Mathf.Clamp01(run);
        animator.SetLayerWeight(1, run);
    }

    public void Move(Transform trg)
    {
        target.target = trg;
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        run += Time.deltaTime * 2;
    }
}
