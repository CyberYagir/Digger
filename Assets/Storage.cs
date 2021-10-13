using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storage : MonoBehaviour
{
    [SerializeField] StackManager stackManager;
    void Start()
    {
        stackManager = GetComponent<StackManager>();
        PlayersManager.instance.AddPlayer(stackManager);
    }
}
