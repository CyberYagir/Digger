using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinColor : MonoBehaviour
{
    public List<Color> skinColors;
    [SerializeField] Renderer r;

    private void Start()
    {
        r.material.color = skinColors[Random.Range(0, skinColors.Count)];
    }
}
