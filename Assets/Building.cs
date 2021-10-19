using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuildingType {Preview, InConstruction, Finished}
public class Building : MonoBehaviour
{
    public BuildingType status;
    public List<GameObject> parts;
    [SerializeField] GameObject building, main, canvas, buyItem;

 

    private void Update()
    {
        switch (status)
        {
            case BuildingType.Preview:
                building.SetActive(false);
                main.SetActive(true);
                canvas.SetActive(false);
                buyItem.SetActive(false);
                break;
            case BuildingType.InConstruction:
                building.SetActive(true);
                canvas.SetActive(true);
                main.SetActive(false);
                buyItem.SetActive(true);
                break;
            case BuildingType.Finished:
                building.SetActive(false);
                main.SetActive(true);
                canvas.SetActive(false);
                buyItem.SetActive(false);
                break;
            default:
                break;
        }
    }

}
