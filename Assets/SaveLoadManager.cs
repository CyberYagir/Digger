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

        foreach (var item in ResoucesManager.instance.itemsAbstract)
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
}
