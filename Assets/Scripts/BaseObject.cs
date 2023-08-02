using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseObject : MonoBehaviour
{
    [SerializeField] public Transform RotationTarget;
    
    
    protected float _currentHitPoints;
    [SerializeField] public float BaseMaxHitPoints = 100.0f;
    protected float _finalMaxHitPoints;

    protected float _currentShieldPoints;
    [SerializeField] public float BaseMaxShieldPoints = 50.0f;
    protected float _finalMaxShieldPoints;


    protected Vector3 _velocity;
    [SerializeField] public float BaseAcceleration = 10.0f;
    protected float _finalAcceleration;
    [SerializeField] public float BaseMaxSpeed = 10.0f;
    protected float _finalMaxSpeed;
    [SerializeField] [Range(0.0f, 60.0f)] public float BaseInertiaBrakingPower = 0.2f;
    protected float _finalInertiaBrakingPower;
    [SerializeField] [Range(0.0f, 60.0f)] public float BaseManualBrakingPower = 1.0f;
    protected float _finalManualBrakingPower;

    protected float _currentRotationalVelocity; //used by Mathf.SmoothDampAngle
    [SerializeField] public float BaseRotationTime = 0.0f;
    protected float _finalRotationTime;
    [SerializeField] public float BaseRotationSpeed = 1440.0f;
    protected float _finalRotationSpeed;

    protected virtual void Start()
    {
        //init properties
        _finalMaxHitPoints = BaseMaxHitPoints;
        _currentHitPoints = _finalMaxHitPoints;

        _finalMaxShieldPoints = BaseMaxShieldPoints;
        _currentShieldPoints = _finalMaxShieldPoints;

        _velocity = Vector2.zero;
        _finalAcceleration = BaseAcceleration;
        _finalMaxSpeed = BaseMaxSpeed;
        _finalInertiaBrakingPower = BaseInertiaBrakingPower;
        _finalManualBrakingPower = BaseManualBrakingPower;

        _finalRotationTime = BaseRotationTime;
        _finalRotationSpeed = BaseRotationSpeed;



    }
}
