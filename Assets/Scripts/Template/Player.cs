using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Пример
/// </summary>

public abstract class MovebleObject : MonoBehaviour {
    protected float run;
    protected Animator animator;
    protected Rigidbody rb;
    public float speed;

    public bool isMineTarget;
    public void Init()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    public float GetLayer(int id)
    {
        return animator.GetLayerWeight(id);
    }
    public float SetLayer(int id, float val = 0)
    {
        animator.SetLayerWeight(id, Mathf.Clamp01(val));
        return val;
    }
    public void Play(string animName, int layer)
    {
        animator.Play(animName, layer);
    }
    public void LateUpdate()
    {
        if (transform.position.y < -5)
        {
            Physics.Raycast(Vector3.zero + new Vector3(0, 100, 0), Vector3.down, out RaycastHit hit);
            transform.position = hit.point;
        }
    }

    public virtual float GetRunSpeed()
    {
        return run;
    }
}

public class Player : MovebleObject
{
    public Joystick joy;

    private void Start()
    {
        Init();
        joy = FindObjectOfType<Joystick>();
    }

    
    void Update()
    {
        if (joy.Direction.magnitude > 0.1f)
        {
            float angle = Mathf.Atan2(joy.Horizontal, joy.Vertical) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, angle, 0));

            Physics.Raycast(transform.position + transform.forward/3f, Vector3.down, out RaycastHit hit);
            if (hit.collider != null)
            {
                transform.Translate(Vector3.forward * speed * Time.deltaTime * joy.Direction.magnitude);
                run += Time.deltaTime * 2;
            }
            else
            {
                run -= Time.deltaTime * 2;
            }
        }
        else
        {
            run -= Time.deltaTime * 2;
        }
        run = Mathf.Clamp01(run);
        animator.SetLayerWeight(1, run);
    }
}
