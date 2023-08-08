using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseShip : BaseObject
{
    [Header("Health")]
    [SerializeField] public float BaseMaxHitPoints = 100.0f;
    protected float _finalMaxHitPoints;
    protected float _currentHitPoints;

    [Header("Shields")]
    [SerializeField] public float BaseMaxShieldPoints = 50.0f;
    protected float _finalMaxShieldPoints;
    protected float _currentShieldPoints;
    [SerializeField] public float BaseMinShieldPointsRegen = 1.0f;
    protected float _finalMinShieldPointsRegen;
    [SerializeField] [Range(0.0f, 6000.0f)] public float BaseMinShieldPercentRegen = 2.0f;
    protected float _finalMinShieldPercentRegen;

    [Header("Boost")]
    [SerializeField] [Range(1.0f, 5.0f)] public float BaseBoostMultiplier = 1.5f;
    protected float _finalBoostMultiplier;
    [SerializeField] [Range(0.0f, 60.0f)] public float BaseBoostDecayTime = 1.0f;
    protected float _finalBoostDecayTime;
    protected float _currentBoostDecayVelocity; //used by Mathf.SmoothDampAngle
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
    protected float _currentAcceleration;
    [SerializeField] public float BaseMaxSpeed = 10.0f;
    protected float _finalMaxSpeed;
    protected float _currentMaxSpeed;
    protected Vector3 _velocity;
    [SerializeField] public float BaseMaxTurningSpeed = 90.0f;
    protected float _finalMaxTurningSpeed;
    protected const float k_turningSpeedDegreeModifier = 15.0f;
    protected float _currentTurningVelocity; //used by Mathf.SmoothDampAngle
    internal float BaseMinTurningTime = 0.0f; //could be public, but not needed
    protected float _finalMinTurningTime;
    [SerializeField] [Range(0.0f, 60.0f)] public float BaseManualBrakingPower = 1.0f;
    protected float _finalManualBrakingPower;
    [SerializeField] [Range(0.0f, 60.0f)] public float BaseInertiaBrakingPower = 0.2f;
    protected float _finalInertiaBrakingPower;

    [Header("Weapons")]
    [SerializeField] public Weapon[] PrimaryWeapons;
    protected int _activePrimaryWeapon;
    [SerializeField] public Weapon[] SecondaryWeapons;
    protected int _activeSecondaryWeapon;
    [SerializeField] public Weapon[] TertiaryWeapons;
    protected int _activeTertiaryWeapon;
    [SerializeField] public Weapon[] QuaternaryWeapons;
    protected int _activeQuaternaryWeapon;

    protected virtual void Start()
    {
        //init health
        _finalMaxHitPoints = BaseMaxHitPoints;
        _currentHitPoints = _finalMaxHitPoints;

        //init shields
        _finalMaxShieldPoints = BaseMaxShieldPoints;
        _currentShieldPoints = _finalMaxShieldPoints;
        _finalMinShieldPointsRegen = BaseMinShieldPointsRegen;
        _finalMinShieldPercentRegen = BaseMinShieldPercentRegen;

        //init boost
        _finalBoostMultiplier = BaseBoostMultiplier;
        _finalBoostDecayTime = BaseBoostDecayTime;
        _finalMaxBoostPoints = BaseMaxBoostPoints;
        _currentBoostPoints = _finalMaxBoostPoints;
        _finalMinBoostPointsRegen = BaseMinBoostPointsRegen;
        _finalMinBoostPercentRegen = BaseMinBoostPercentRegen;

        //init engines
        _finalAcceleration = BaseAcceleration;
        _currentAcceleration = _finalAcceleration;
        _finalMaxSpeed = BaseMaxSpeed;
        _velocity = Vector2.zero;
        _finalMaxTurningSpeed = BaseMaxTurningSpeed;
        _finalMinTurningTime = BaseMinTurningTime;
        _finalManualBrakingPower = BaseManualBrakingPower;
        _finalInertiaBrakingPower = BaseInertiaBrakingPower;

        //init weapons
        _activePrimaryWeapon = 0;
        _activeSecondaryWeapon = 0;
        _activeTertiaryWeapon = 0;
        _activeQuaternaryWeapon = 0;
    }

    protected void Fire1()
    {
        if (PrimaryWeapons.Length > 0)
        {
            
        }
    }

    protected void Fire2()
    {
        if (SecondaryWeapons.Length > 0)
        {

        }
    }

    protected void Fire3()
    {
        if (TertiaryWeapons.Length > 0)
        {

        }
    }

    protected void Fire4()
    {
        if (QuaternaryWeapons.Length > 0)
        {

        }
    }
}
