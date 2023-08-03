using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseShip : BaseObject
{
    [Header("Shields")]
    [SerializeField] public float BaseMaxShieldPoints = 50.0f;
    protected float _finalMaxShieldPoints;
    protected float _currentShieldPoints;
    [SerializeField] public float BaseMinShieldPointsRegen = 1.0f;
    protected float _finalMinShieldPointsRegen;
    [SerializeField] [Range(0.0f, 6000.0f)] public float BaseMinShieldPercentRegen = 2.0f;
    protected float _finalMinShieldPercentRegen;

    [Header("Boost")]
    [SerializeField] public float BaseMaxBoostPoints = 1.0f;
    protected float _finalMaxBoostPoints;
    protected float _currentBoostPoints;
    [SerializeField] public float BaseMinBoostPointsRegen = 0.1f;
    protected float _finalMinBoostPointsRegen;
    [SerializeField] [Range(0.0f, 6000.0f)] public float BaseMinBoostPercentRegen = 10.0f;
    protected float _finalMinBoostPercentRegen;

    [Header("Engines")]
    [SerializeField] public Transform Helm; //used for a target rotation for turning
    [Space(5)]
    [SerializeField] public float BaseAcceleration = 10.0f;
    protected float _finalAcceleration;
    [SerializeField] public float BaseMaxSpeed = 10.0f;
    protected float _finalMaxSpeed;
    protected Vector3 _velocity;
    [SerializeField] public float BaseMaxTurningSpeed = 1440.0f;
    protected float _finalMaxTurningSpeed;
    protected const float k_turningSpeedDegreeModifier = 9.0f;
    protected float _currentTurningVelocity; //used by Mathf.SmoothDampAngle
    [SerializeField] public float BaseMinTurningTime = 0.0f;
    protected float _finalMinTurningTime;
    [SerializeField] [Range(0.0f, 60.0f)] public float BaseManualBrakingPower = 1.0f;
    protected float _finalManualBrakingPower;
    [SerializeField] [Range(0.0f, 60.0f)] public float BaseInertiaBrakingPower = 0.2f;
    protected float _finalInertiaBrakingPower;

    protected override void Start()
    {
        base.Start();

        //init shields
        _finalMaxShieldPoints = BaseMaxShieldPoints;
        _currentShieldPoints = _finalMaxShieldPoints;
        _finalMinShieldPointsRegen = BaseMinShieldPointsRegen;
        _finalMinShieldPercentRegen = BaseMinShieldPercentRegen;

        //init boost
        _finalMaxBoostPoints = BaseMaxBoostPoints;
        _currentBoostPoints = _finalMaxBoostPoints;
        _finalMinBoostPointsRegen = BaseMinBoostPointsRegen;
        _finalMinBoostPercentRegen = BaseMinBoostPercentRegen;

        //init engines
        _finalAcceleration = BaseAcceleration;
        _finalMaxSpeed = BaseMaxSpeed;
        _velocity = Vector2.zero;
        _finalMaxTurningSpeed = BaseMaxTurningSpeed;
        _finalMinTurningTime = BaseMinTurningTime;
        _finalManualBrakingPower = BaseManualBrakingPower;
        _finalInertiaBrakingPower = BaseInertiaBrakingPower;
    }
}
