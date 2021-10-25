using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfinderToPlayer : MonoBehaviour
{
    Vector3 point;
    private void Start()
    {
        SetPathFinder();
    }
    void Update()
    {
        if (Vector3.Distance(GameManger.player.transform.position, point) >= 20)
        {
            SetPathFinder();
        }
    }

    public void SetPathFinder()
    {
        (AstarPath.active.data.graphs[0] as GridGraph).center = new Vector3(GameManger.player.transform.position.x, (AstarPath.active.data.graphs[0] as GridGraph).center.y, GameManger.player.transform.position.z);
        point = (AstarPath.active.data.graphs[0] as GridGraph).center;
        AstarPath.active.Scan();
    }
}
