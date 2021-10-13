using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBuild : MonoBehaviour
{
    public Vector3 offcet;
    public Vector3 moveOffcet;
    public float speed = 10;
    public Vector3 pos;
    void Start()
    {
        moveOffcet = Vector3.zero;
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, GameManger.player.transform.position + moveOffcet + offcet, speed * Time.deltaTime);

        if (Input.GetKey(KeyCode.Mouse1) && !Input.GetKeyDown(KeyCode.Mouse1))
        {
            var oldMoveOffcet = moveOffcet;
            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit))
            {
                if (hit.collider == null)
                {
                    moveOffcet = oldMoveOffcet;
                    transform.position = GameManger.player.transform.position + moveOffcet + offcet;
                }
                else
                {
                    var newOffcet = moveOffcet - new Vector3(Input.GetAxis("Mouse X"), 0, Input.GetAxis("Mouse Y")) * speed * Time.deltaTime;
                    if (Physics.Raycast(GameManger.player.transform.position + newOffcet + offcet, transform.forward, out RaycastHit hit2))
                    {
                        if (hit2.collider != null)
                        {
                            moveOffcet -= new Vector3(Input.GetAxis("Mouse X"), 0, Input.GetAxis("Mouse Y")) * speed * Time.deltaTime;
                        }
                    }
                    Debug.DrawLine(transform.position, hit.point);
                }
            }
        }
    }
}
