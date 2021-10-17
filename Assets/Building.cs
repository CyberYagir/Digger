using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public List<GameObject> parts;
    [SerializeField] GameObject building, main;

    public void BuildingMode()
    {
        building.SetActive(true);
        main.SetActive(false);
    }
    public void NormalMode()
    {
        building.SetActive(true);
        main.SetActive(true);
    }
}
