using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomScale : MonoBehaviour
{
    public float min, max;

    private void Start()
    {
        if (transform.localScale == Vector3.one)
            transform.localScale *= Random.Range(min, max);
    }
}
