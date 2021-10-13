using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildPoint : MonoBehaviour
{
    public static BuildPoint instance;
    private void Start()
    {
        instance = this;
    }
}
