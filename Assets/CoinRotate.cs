using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinRotate : MonoBehaviour
{
    [SerializeField] float speed; 
    void Update()
    {
        transform.eulerAngles += Vector3.up * speed * Time.deltaTime;
    }
}
