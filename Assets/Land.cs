using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Land : MonoBehaviour
{
    public Vector2Int arrayPos;
    public GameObject[] buyPoints;
    public Transform enteties;

    private void Start()
    {
        foreach (var item in buyPoints)
        {
            item.gameObject.SetActive(LandsManager.instance.CheckPos(arrayPos, item.GetComponentInChildren<BuyLand>().dir));
        }
    }
}
