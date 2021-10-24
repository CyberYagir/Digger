using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Item {
    public Sprite icon;
    public PhysicMaterial physicMaterial;
    public GameObject prefab;
}

public class ItemsManager : MonoBehaviour
{
    public static ItemsManager instance;
    public List<Item> items = new List<Item>();
    public GameObject particles;
    private void Awake()
    {
        instance = this;
    }

    public static Item GetItem(int id)
    {
        return instance.items[id];
    }
    public static int GetItemByMat(string name)
    {
        if (instance.items.Find(x => x.physicMaterial.name.Trim().ToLower() == name.Trim().ToLower()) != null)
        {
            return instance.items.FindIndex(x => x.physicMaterial.name.Trim().ToLower() == name.Trim().ToLower());
        }
        else
        {
            return 0;
        }
    }

}
