using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseObject : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] public float BaseMaxHitPoints = 100.0f;
    protected float _finalMaxHitPoints;
    protected float _currentHitPoints;

    

    protected virtual void Start()
    {
        //init health
        _finalMaxHitPoints = BaseMaxHitPoints;
        _currentHitPoints = _finalMaxHitPoints;
    }
}
