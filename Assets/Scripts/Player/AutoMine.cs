using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class MineItem {
    public EntityType entityType;
    public GameObject tool;
    public int animationLayer;
}


public class AutoMine : MonoBehaviour
{
    [SerializeField] List<MineItem> mineItems = new List<MineItem>();
    MovebleObject player;
    ActiveEntity activeEntity;
    [SerializeField] bool isEmptyMiner;
    private void Start()
    {
        player = GetComponent<MovebleObject>();
        activeEntity = GetComponent<ActiveEntity>();
    }

    private void Update()
    {
        if (player.GetRunSpeed() == 0)
        {
            if (activeEntity.GetCurrentEntity(isEmptyMiner) != null)
            {
                player.isMineTarget = true;
                var pos = activeEntity.GetCurrentEntity().getSelf().transform.position - transform.position;
                pos = new Vector3(pos.x, transform.position.y, pos.z);
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(pos), 5 * Time.deltaTime);
                transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
                var weapon = mineItems.Find(x => x.entityType == activeEntity.GetCurrentEntity().getSelf().entityType);
                weapon.tool.gameObject.SetActive(true);
                player.Play("Mine", weapon.animationLayer);
                player.SetLayer(weapon.animationLayer, player.GetLayer(weapon.animationLayer) + Time.deltaTime * 2);
                return;
            }
        }

        player.isMineTarget = false;
        foreach (var item in mineItems)
        {
            player.SetLayer(item.animationLayer, player.GetLayer(item.animationLayer) - Time.deltaTime * 4);
            if (item.tool != null)
                item.tool.gameObject.SetActive(false);
        }
    }
}
