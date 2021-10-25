using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BuyLand : MonoBehaviour
{
    public int money;
    public Vector2Int dir;
    public TMP_Text text;
    Vector2Int parentPos;
    private void Start()
    {
        parentPos = GetComponentInParent<Land>().arrayPos;
        money *= LandsManager.instance.activeLands.Count;
        UpdateText();

    }

    private void Update()
    {
        if (!LandsManager.instance.CheckPos(parentPos, dir))
        {
            gameObject.SetActive(false);
        }
    }
    public void Buy()
    {
        UpdateText();
        if (money - ResoucesManager.GetItemAbstract("Coin").value <= 0)
        {
            ResoucesManager.GetItemAbstract("Coin").value -= money;
            money = 0;
        }

        if (money == 0)
        {
            LandsManager.instance.ActiveLandWithLoading(parentPos + dir);
            gameObject.SetActive(false);
            StatsUI.instance.Redraw();
        }
    }

    public void UpdateText()
    {
        text.text = money.ToString();
    }
}
