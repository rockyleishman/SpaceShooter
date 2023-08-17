using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Team
{
    Player,
    Enemy,
    Neutral,
    None
}

public abstract class BaseShip : PoolObject
{
    [Header("Team")]
    [SerializeField] public Team ShipTeam = Team.Enemy;

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
    [SerializeField] public float BaseShieldRegenDelay = 5.0f;
    protected float _finalShieldRegenDelay;
    protected float _shieldRegenTimer;

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
    protected const float k_turningSpeedDegreeModifier = 12566.371f;
    protected float _currentTurningVelocity; //used by Mathf.SmoothDampAngle
    [SerializeField] [Range(0.0f, 60.0f)] public float BaseManualBrakingPower = 1.0f;
    protected float _finalManualBrakingPower;
    [SerializeField] [Range(0.0f, 60.0f)] public float BaseInertiaBrakingPower = 0.2f;
    protected float _finalInertiaBrakingPower;

    [Header("Weapons")]
    [SerializeField] public Weapon[] PrimaryWeapons;
    protected int _activePrimaryWeapon;
    [SerializeField] [Range(0.0f, 360.0f)] public float BasePrimaryWeaponInaccuracy;
    protected float _finalPrimaryWeaponInaccuracy;
    [SerializeField] public Weapon[] SecondaryWeapons;
    protected int _activeSecondaryWeapon;
    [SerializeField] [Range(0.0f, 360.0f)] public float BaseSecondaryWeaponInaccuracy;
    protected float _finalSecondaryWeaponInaccuracy;
    [SerializeField] public Weapon[] TertiaryWeapons;
    protected int _activeTertiaryWeapon;
    [SerializeField] [Range(0.0f, 360.0f)] public float BaseTertiaryWeaponInaccuracy;
    protected float _finalTertiaryWeaponInaccuracy;
    [SerializeField] public Weapon[] QuaternaryWeapons;
    protected int _activeQuaternaryWeapon;
    [SerializeField] [Range(0.0f, 360.0f)] public float BaseQuaternaryWeaponInaccuracy;
    protected float _finalQuaternaryWeaponInaccuracy;

    internal float _aimAngle; //global angle //CHANGE TO PROTECTED

    [SerializeField] public Transform Reticle; //REMOVE TO PLAYER CONTROLLER

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
        _finalShieldRegenDelay = BaseShieldRegenDelay;
        _shieldRegenTimer = 0.0f;

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
        _finalManualBrakingPower = BaseManualBrakingPower;
        _finalInertiaBrakingPower = BaseInertiaBrakingPower;

        //init weapons
        _activePrimaryWeapon = 0;
        _finalPrimaryWeaponInaccuracy = BasePrimaryWeaponInaccuracy;
        _activeSecondaryWeapon = 0;
        _finalSecondaryWeaponInaccuracy = BaseSecondaryWeaponInaccuracy;
        _activeTertiaryWeapon = 0;
        _finalTertiaryWeaponInaccuracy = BaseTertiaryWeaponInaccuracy;
        _activeQuaternaryWeapon = 0;
        _finalQuaternaryWeaponInaccuracy = BaseQuaternaryWeaponInaccuracy;
    }

    protected virtual void Update()
    {
        RegenShields();
    }

    #region Health

    public void Damage(float attackDamage, float ionDamage, float piercingDamage)
    {
        //apply attack damage
        _currentShieldPoints -= attackDamage;
        if (_currentShieldPoints < 0.0f)
        {
            //apply extra damage to hull
            _currentHitPoints += _currentShieldPoints;
        }

        //apply ion damage
        _currentShieldPoints -= ionDamage;
        if (_currentShieldPoints < 0.0f)
        {
            //set shield to zero
            _currentShieldPoints = 0.0f;
        }

        //apply piercing damage
        _currentHitPoints -= piercingDamage;
        if (_currentHitPoints <= 0.0f)
        {
            //destroy ship
            _currentHitPoints = 0.0f;
            Death();
        }

        //reset shield regen timer
        _shieldRegenTimer = _finalShieldRegenDelay;
    }

    public void Heal(float healthAmount, float shieldsAmount)
    {
        //apply health heal
        _currentHitPoints += healthAmount;
        if (_currentHitPoints > _finalMaxHitPoints)
        {
            _currentHitPoints = _finalMaxHitPoints;
        }

        //apply shields heal
        _currentShieldPoints += shieldsAmount;
        if (_currentShieldPoints > _finalMaxShieldPoints)
        {
            _currentShieldPoints = _finalMaxShieldPoints;
        }
    }

    protected void RegenShields()
    {
        if (_shieldRegenTimer <= 0.0f && _currentShieldPoints < _finalMaxShieldPoints)
        {
            float shieldPercentUp = _finalMaxShieldPoints * _finalMinShieldPercentRegen / 100.0f * Time.deltaTime;
            float shieldPointsUp = _finalMinShieldPointsRegen * Time.deltaTime;
            if (shieldPercentUp > shieldPointsUp)
            {
                _currentShieldPoints += shieldPercentUp;
            }
            else
            {
                _currentShieldPoints += shieldPointsUp;
            }
            //clamp at max
            if (_currentShieldPoints > _finalMaxShieldPoints)
            {
                _currentShieldPoints = _finalMaxShieldPoints;
            }
        }

        //decrease shield regen timer
        _shieldRegenTimer -= Time.deltaTime;
    }
    protected void RegenBoost()
    {
        if (_currentBoostPoints < _finalMaxBoostPoints)
        {
            float boostPercentUp = _finalMaxBoostPoints * _finalMinBoostPercentRegen / 100.0f * Time.deltaTime;
            float boostPointsUp = _finalMinBoostPointsRegen * Time.deltaTime;
            if (boostPercentUp > boostPointsUp)
            {
                _currentBoostPoints += boostPercentUp;
            }
            else
            {
                _currentBoostPoints += boostPointsUp;
            }
            //clamp at max
            if (_currentBoostPoints > _finalMaxBoostPoints)
            {
                _currentBoostPoints = _finalMaxBoostPoints;
            }
        }
    }

    protected abstract void Death();

    #endregion

    #region Weapons

    protected void Fire1Auto()
    {
        if (PrimaryWeapons.Length > 0)
        {
            PrimaryWeapons[_activePrimaryWeapon].AutoTrigger(ShipTeam, _finalPrimaryWeaponInaccuracy, Reticle, _aimAngle);
        }
    }

    protected void Fire1Semi()
    {
        if (PrimaryWeapons.Length > 0)
        {
            PrimaryWeapons[_activePrimaryWeapon].SemiTrigger(ShipTeam, _finalPrimaryWeaponInaccuracy, Reticle, _aimAngle);
        }
    }

    protected void Fire2Auto()
    {
        if (SecondaryWeapons.Length > 0)
        {
            SecondaryWeapons[_activeSecondaryWeapon].AutoTrigger(ShipTeam, _finalPrimaryWeaponInaccuracy, Reticle, _aimAngle);
        }
    }

    protected void Fire2Semi()
    {
        if (SecondaryWeapons.Length > 0)
        {
            SecondaryWeapons[_activeSecondaryWeapon].SemiTrigger(ShipTeam, _finalPrimaryWeaponInaccuracy, Reticle, _aimAngle);
        }
    }

    protected void Fire3Auto()
    {
        if (TertiaryWeapons.Length > 0)
        {
            TertiaryWeapons[_activeTertiaryWeapon].AutoTrigger(ShipTeam, _finalPrimaryWeaponInaccuracy, Reticle, _aimAngle);
        }
    }

    protected void Fire3Semi()
    {
        if (TertiaryWeapons.Length > 0)
        {
            TertiaryWeapons[_activeTertiaryWeapon].SemiTrigger(ShipTeam, _finalPrimaryWeaponInaccuracy, Reticle, _aimAngle);
        }
    }

    protected void Fire4Auto()
    {
        if (QuaternaryWeapons.Length > 0)
        {
            QuaternaryWeapons[_activeQuaternaryWeapon].AutoTrigger(ShipTeam, _finalPrimaryWeaponInaccuracy, Reticle, _aimAngle);
        }
    }

    protected void Fire4Semi()
    {
        if (QuaternaryWeapons.Length > 0)
        {
            QuaternaryWeapons[_activeQuaternaryWeapon].SemiTrigger(ShipTeam, _finalPrimaryWeaponInaccuracy, Reticle, _aimAngle);
        }
    }

    #endregion
}
