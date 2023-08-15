using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : PoolObject
{
    [SerializeField] float Persistence = 2.0f;
    private float _lifeTimeRemaining;

    public void Init()
    {
        _lifeTimeRemaining = Persistence;
    }

    private void Update()
    {
        //update life time remaining
        _lifeTimeRemaining -= Time.deltaTime;

        //check existence
        if (_lifeTimeRemaining <= 0.0f)
        {
            //despawn
            OnDespawn();
        }
    }
}
