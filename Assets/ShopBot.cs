using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopBot : MonoBehaviour
{
    [SerializeField] GameObject poof;
    [SerializeField] Transform spawnPoint;

    public void AddBot()
    {
        var prtc =  Instantiate(poof, spawnPoint.position, spawnPoint.rotation);
        Destroy(prtc, 1);

        var bt = Instantiate(PlayersManager.instance.botPrefab, spawnPoint.position, spawnPoint.rotation);
        PlayersManager.instance.AddPlayer(bt.GetComponent<MovebleObject>());
    }
}
