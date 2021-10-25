using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingParametrs : MonoBehaviour
{
    public Vector3 sizes;
    public Vector3 localCenterPosition;



    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.TransformPoint(localCenterPosition), sizes);
    }
}
