using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
[System.Serializable]
public class Vector2C
{
    public float x, y;
    public Vector2C()
    {

    }

    public Vector2C(float x, float y)
    {
        this.x = (float)System.Math.Round(x, 2);
        this.y = (float)System.Math.Round(y, 2);
    }
    public Vector2C(Vector2 vector3)
    {
        this.x = (float)System.Math.Round(vector3.x, 2);
        this.y = (float)System.Math.Round(vector3.y, 2);
    }

    public Vector2 back()
    {
        return new Vector2(x, y);
    }
}
[System.Serializable]
public class Vector3C:Vector2C
{
    public float z;
    public Vector3C(float x, float y, float z):base(x,y)
    {
        this.z = (float)System.Math.Round(z, 2);
    }
    public Vector3C(Vector3 vector3):base(vector3.x, vector3.y)
    {
        this.z = (float)System.Math.Round(vector3.z, 2);
    }
    public Vector3C()
    {

    }
    public new Vector3 back()
    {
        return new Vector3(x, y,z);
    }
}



[System.Serializable]
public class BuildingData
{
    public Vector3C pos, rot;
    public string name;
    [Space]
    public StackData stackData;
    [Space]
    public BuildingType buildingType;
    public List<ValueItem> items = new List<ValueItem>();
}

[System.Serializable]
public class EntityData
{
    public EntityType entityType;
    public Vector3C pos, rot, scale;
}
[System.Serializable]
public class LandData
{
    public Vector2C arrayPos;
    public List<EntityData> entityDatas = new List<EntityData>();
    public int nextIslandCost;
}

[System.Serializable]
public class AbstractItemData
{
    public string name;
    public int value;
}

[System.Serializable]
public class StackData
{
    /// <summary>
    /// item resourceID
    /// </summary>
    public List<int> stack = new List<int>();
}

[System.Serializable]
public class MovableData
{
    public Vector3C pos, rot;
    public StackData stackData;
}

[System.Serializable]
public class Data
{
    public MovableData playerData;
    public List<AbstractItemData> abstractItems = new List<AbstractItemData>();
    [Space]
    public List<MovableData> botsData = new List<MovableData>();
    [Space]
    public List<LandData> lands = new List<LandData>();
    [Space]
    public List<BuildingData> buildings = new List<BuildingData>();
}

public class SaveLoadManager : MonoBehaviour
{
    public Data data;

    public static string path;
    public static bool haveSave;
    private void Awake()
    {
        path = Application.persistentDataPath + "/save.json";
        haveSave = File.Exists(path);
       
    }

    public void Start()
    {
        if (haveSave)
        {
            Load();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F12))
        {
            Save();
        }
        if (Input.GetKeyDown(KeyCode.F11))
        {
            Load();
        }
    }

    public void Save()
    {
        data = new Data();

        #region Player
        var pl = GameManger.player.transform;
        data.playerData = new MovableData() { pos = new Vector3C(pl.position), rot = new Vector3C(pl.localEulerAngles), stackData = StackManagerToData(pl.GetComponent<StackManager>()) };
        #endregion

        #region AbstractItems

        foreach (var item in ResourcesManager.instance.itemsAbstract)
        {
            data.abstractItems.Add(new AbstractItemData() { name = item.name, value = item.value });
        }

        #endregion

        #region Bots
        var bots = PlayersManager.instance.players.FindAll(x => x.playerDataType == PlayerDataType.Bot);
        foreach (var item in bots)
        {
            data.botsData.Add(new MovableData() { pos = new Vector3C(item.move.transform.position), rot = new Vector3C(item.move.transform.localEulerAngles), stackData = StackManagerToData(item.stack) });
        }
        #endregion

        #region Lands

        foreach (var item in LandsManager.instance.activeLands)
        {
            var land = new LandData() { arrayPos = new Vector2C(item.arrayPos) };
            land.nextIslandCost = item.buyPoints[0].GetComponent<BuyLand>().money;
            foreach (Transform entity in item.enteties)
            {
                var ent = entity.GetComponent<Entity>();
                land.entityDatas.Add(new EntityData() { entityType = ent.entityType, pos = new Vector3C(entity.position), rot = new Vector3C(entity.localEulerAngles), scale = new Vector3C(entity.localScale) });
            }
            data.lands.Add(land);
        }

        #endregion

        #region SaveBuilds

        var buildings = FindObjectsOfType<Building>().ToList();
        buildings.RemoveAll(x => x.status == BuildingType.Preview);
        foreach (var item in buildings)
        {
            data.buildings.Add(new BuildingData() { name = item.houseName, pos = new Vector3C(item.transform.position), rot = new Vector3C(item.transform.localEulerAngles), stackData = StackManagerToData(item.GetComponent<StackManager>()), buildingType = item.status, items = item.status == BuildingType.InConstruction ? item.GetComponentInChildren<BuyItem>().items : null});
        }

        #endregion

        File.WriteAllText(path, JsonConvert.SerializeObject(data, Formatting.None, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }));

    }

    public void Load()
    {
        data = JsonConvert.DeserializeObject<Data>(File.ReadAllText(path));

        #region LandCreation
        for (int i = 0; i < data.lands.Count; i++)
        {
            var land = LandsManager.instance.AddLand(Vector2Int.RoundToInt(data.lands[i].arrayPos.back()), false);
            foreach (var ent in data.lands[i].entityDatas)
            {
                var obj = Instantiate(LandRegenerator.instance.resourcesList[(int)ent.entityType], ent.pos.back(), Quaternion.Euler(ent.rot.back()));
                obj.transform.parent = land.enteties;
                obj.transform.localScale = ent.scale.back();
            }
            foreach (var item in land.buyPoints)
            {
                item.GetComponent<BuyLand>().money = data.lands[i].nextIslandCost;
                item.GetComponent<BuyLand>().initMoney = false;
            }
            
        }
        #endregion

        #region AbstractItems
        foreach (var abst in data.abstractItems)
        {
            ResourcesManager.instance.SetToAbstract(abst.name, abst.value);
        }
        #endregion

        #region Player

        GameManger.player.transform.position = data.playerData.pos.back();
        GameManger.player.transform.localEulerAngles = data.playerData.rot.back();
        RestoreStackManager(data.playerData.stackData, GameManger.player.GetComponent<StackManager>());

        #endregion

        #region Bots
        if (data.botsData.Count != 0)
        {
            AstarPath.active.Scan();

            foreach (var botD in data.botsData)
            {
                var bt = Instantiate(PlayersManager.instance.botPrefab, botD.pos.back(), Quaternion.Euler(botD.rot.back()));
                PlayersManager.instance.AddPlayer(bt.GetComponent<MovebleObject>());
                RestoreStackManager(botD.stackData, bt.GetComponent<StackManager>());
            }
        }
        #endregion

        #region Buildings
        var builder = FindObjectOfType<Builder>();
        foreach (var builD in data.buildings)
        {
            var build = Instantiate(builder.builds.Find(x => x.name.Trim().ToLower() == builD.name.Trim().ToLower())).GetComponent<Building>();
            build.transform.position = builD.pos.back();
            build.transform.localEulerAngles = builD.rot.back();
            build.status = builD.buildingType;
            build.houseName = builD.name;
            if (builD.buildingType == BuildingType.InConstruction)
            {
                var bItem = build.GetComponentInChildren<BuyItem>(true);
                if (bItem.stack.Count == 0)
                {
                    bItem.SetPreview();
                }
                for (int a = 0; a < builD.items.Count; a++)
                {
                    for (int res = 0; res < builD.items[a].maxValue - builD.items[a].value; res++)
                    {
                        var drp = Instantiate(ItemsManager.GetItem(builD.items[a].itemID).prefab).GetComponent<Drop>();
                        drp.resourceID = builD.items[a].itemID;
                        drp.dontDestroy = true;

                        drp.SetLayer(bItem.stackTransform.gameObject.layer);
                        bItem.AddInList(drp, true);
                        bItem.items[a].value -= 1;
                    }
                }
                bItem.UpdateUI();
            }
            else if (builD.buildingType == BuildingType.Finished)
            {
                if (build.GetComponent<Storage>())
                {
                    RestoreStackManager(builD.stackData, build.GetComponent<Storage>().stackManager);
                }
            }
        }

        #endregion
    }



    public void RestoreStackManager(StackData stackData, StackManager stackManager)
    {
        foreach (var rID in stackData.stack)
        {

            var drop = Instantiate(ItemsManager.GetItem(rID).prefab).GetComponent<Drop>();
            drop.SetLayer(stackManager.gameObject.layer);
            drop.resourceID = rID;
            drop.dontDestroy = true;
            drop.localPos = stackManager.AddInStack(drop.transform, false, true);
        }
        StatsUI.instance.Redraw();
    }

    public StackData StackManagerToData(StackManager stackm)
    {
        if (stackm == null)
        {
            return null;
        }
        var stackd = new StackData();
        foreach (var item in stackm.GetStack())
        {
            stackd.stack.Add(item.resourceID);
        }
        return stackd;
    }


    private void OnApplicationPause(bool pause)
    {
        Save();
    }
}
