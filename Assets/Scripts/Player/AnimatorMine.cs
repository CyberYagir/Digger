using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorMine : MonoBehaviour
{
    ActiveEntity activeEntity;
    private void Start()
    {
        activeEntity = GetComponentInParent<ActiveEntity>();
    }
    public void Mine()
    {
        if (activeEntity.GetCurrentEntity() != null)
        {
            CameraShake.Shake();
            activeEntity.GetCurrentEntity().Mine(GetComponentInParent<StackManager>());
        }
    }
}
