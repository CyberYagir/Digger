using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Vector3 offcet;
    public float speed = 10;
    Quaternion startRot;
    Vector3 minePos = new Vector3(0, -0.5f, -35f);

    void Start()
    {
        startRot = transform.rotation;
    }
    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, GameManger.player.transform.position + offcet, speed * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, startRot, 5 * Time.deltaTime);
    }
}
