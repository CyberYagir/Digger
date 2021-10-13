using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionPoint : MonoBehaviour
{
    [SerializeField] Color color;

    private void Start()
    {
        GetComponent<Renderer>().material.color = color;
    }
}
