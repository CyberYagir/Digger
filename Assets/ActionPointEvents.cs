using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class ActionCircle:MonoBehaviour { }

public class ActionPointEvents : ActionCircle
{
    [SerializeField] bool triggered, waitForExit;

    [SerializeField] UnityEvent action, exit;

    float time;
    private void Start()
    {
    }

    private void Update()
    {
        if (triggered)
        {
            time += Time.deltaTime;
            if (time >= 1 && waitForExit == false)
            {
                action.Invoke();
                waitForExit = true;
            }
        }
        else
        {
            time = 0;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<Player>())
        {
            triggered = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponentInParent<Player>())
        {
            exit.Invoke();
            waitForExit = false;
            triggered = false;
        }
    }

}
