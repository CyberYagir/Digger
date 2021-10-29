using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class MillRotator : MonoBehaviour
{
    public Vector3 rotate;
    void Update()
    {
        transform.Rotate(rotate * Time.deltaTime);
    }
}
