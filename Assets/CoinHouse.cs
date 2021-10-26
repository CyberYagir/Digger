using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinHouse : MonoBehaviour
{
    [SerializeField] float coinTime, coinProgress;
    Building building;
    [SerializeField] GameObject coin;
    [SerializeField] ParticleSystem particleSystem;
    private void Start()
    {
        building = GetComponent<Building>();
    }

    private void Update()
    {
        if (building.status == BuildingType.Finished)
        {
            coinProgress += Time.deltaTime;
            coin.SetActive((coinProgress >= coinTime));
        }
        
    }


    public void GetCoin()
    {
        if ((coinProgress >= coinTime))
        {
            ResourcesManager.instance.AddToAbstract("Coin", 1);
            coinProgress = -1;
            particleSystem.Play();
            StatsUI.instance.Redraw();
        }
    }
}
