using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BotMoving : MovebleObject
{
    [Space]
    public float minDistance;
    public float maxDistance;
    ActiveEntity activeEntity;
    AIDestinationSetter target;
    AIPath aiPath;
    [ReadOnly] public Transform targetTree;

    public enum BotModes { Idle, BackToPlayer, Mining, MoveToTarget};
    public BotModes botModes;
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


        if (distToPlayer > 15)
        {
            botModes = BotModes.BackToPlayer;
        }
        else
        {
            if (botModes == BotModes.BackToPlayer)
            {
                if (distToPlayer < 5)
                {
                    botModes = BotModes.Idle;
                }
            }
        }


        switch (botModes)
        {
            case BotModes.BackToPlayer:
                target.target = GameManger.player.transform;
                break;
            case BotModes.Idle:
                if (activeEntity.GetCurrentEntity() == null)
                {
                    Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit);
                    var land = hit.transform.GetComponentInParent<Land>();
                    if (land == null)
                    {
                        land = LandsManager.instance.activeLands[0];
                    }
                    var trees = land.enteties.Cast<Transform>().OrderBy(x => Vector3.Distance(transform.position, x.position)).ToList();
                    if (trees.Count != 0)
                    {
                        for (int i = 0; i < trees.Count; i++)
                        {
                            var ent = trees[i].GetComponent<Entity>();
                            if (ent.miner == activeEntity || ent.miner == null)
                            {
                                targetTree = ent.transform;
                                botModes = BotModes.MoveToTarget;
                                break;
                            }
                        }
                    }
                }
                break;
            case BotModes.Mining:
                if (activeEntity.GetCurrentEntity() == null)
                {
                    botModes = BotModes.Idle;
                }
                else
                {
                    var miner = activeEntity.GetCurrentEntity().getSelf().miner;
                    if (miner != activeEntity && miner != null)
                    {
                        botModes = BotModes.Idle;
                    }
                }
                break;
            case BotModes.MoveToTarget:
                if (targetTree == null)
                {
                    botModes = BotModes.Idle;
                }
                else
                {
                    target.target = targetTree;
                    if (activeEntity.GetCurrentEntity() != null)
                    {
                        var self = activeEntity.GetCurrentEntity().getSelf().transform;
                        if (Vector3.Distance(self.position, transform.position) <= (1f * self.transform.localScale.x)/2f)
                        {
                            botModes = BotModes.Mining;
                        }
                        var miner = activeEntity.GetCurrentEntity().getSelf().miner;
                        if (miner != activeEntity && miner != null)
                        {
                            activeEntity.RemoveEntity(activeEntity.GetCurrentEntity());
                            botModes = BotModes.Idle;
                        }
                    }
                    
                }
                break;
            default:
                break;
        }

        run = aiPath.velocity.normalized.magnitude;

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
