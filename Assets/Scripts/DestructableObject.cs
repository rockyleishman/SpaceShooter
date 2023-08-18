using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableObject : PoolObject
{
    [Header("Team")]
    [SerializeField] public Team ShipTeam = Team.Enemy;

    [Header("Health")]
    [SerializeField] public Effect DeathEffect;
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

    private void Start()
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
    }

    #region Health

    internal void Damage(float attackDamage, float ionDamage, float piercingDamage)
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

    internal void Heal(float healthAmount, float shieldsAmount)
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

    protected virtual void Death()
    {
        //spawn death effect
        Effect deathEffect = (Effect)PoolManager.Instance.Spawn(DeathEffect.name, transform.position);
        deathEffect.Init();

        //despawn this
        OnDespawn();
    }

    #endregion
}
