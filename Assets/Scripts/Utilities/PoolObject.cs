using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolObject : MonoBehaviour
{
    public void OnDespawn()
    {
        PoolManager.Instance.Despawn(this);
    }
}
