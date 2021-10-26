using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum PlayerDataType {Player, Bot, Building};

[System.Serializable]
public class PlayerData {
    public MovebleObject move;
    public ActiveEntity targets;
    public StackManager stack;
    public PlayerDataType playerDataType;
}

public class PlayersManager : MonoBehaviour
{
    public static PlayersManager instance;
    public List<PlayerData> players;
    public GameObject botPrefab;
    private void Awake()
    {
        instance = this;
        players = new List<PlayerData>();
    }
    private void Start()
    {
        var pl = FindObjectsOfType<MovebleObject>().ToList();
        foreach (var item in pl)
        {
            AddPlayer(item);
        }
    }


    public PlayerData AddPlayer(MovebleObject obj)
    {
        var type = PlayerDataType.Bot;
        if (obj is Player)
        {
            type = PlayerDataType.Player;
        }

        var pd = new PlayerData() { move = obj, stack = obj.GetComponent<StackManager>(), targets = obj.GetComponent<ActiveEntity>(), playerDataType = type};
        players.Add(pd);
        return pd;
    }

    public PlayerData AddPlayer(StackManager obj)
    {
        var pd = new PlayerData() { stack = obj.GetComponent<StackManager>(), playerDataType = PlayerDataType.Building};
        players.Add(pd);
        return pd;
    }
}
