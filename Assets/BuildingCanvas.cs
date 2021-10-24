using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingCanvas : MonoBehaviour
{
    Quaternion globalRot;

    private void Update()
    {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, 0, 0);
    }
}
