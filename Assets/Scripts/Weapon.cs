using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    Projectile,
    Missile,
    Laser
}

public enum AimType
{
    Forward,
    LimitedFocused,
    LimitedDirectional,
    FullFocused,
    FullDirectional,
    Random
}

public class Weapon : MonoBehaviour
{
    [Header("Barrel Tips")]
    [SerializeField] public Transform[] BarrelTips;
    
    [Header("Weapon Type")]
    [SerializeField] public WeaponType WeaponType = WeaponType.Projectile;

    [Header("Targeting")]
    [SerializeField] public AimType AimType = AimType.FullFocused;
    [SerializeField] [Range(0.0f, 360.0f)] public float BaseLimitedRange = 90.0f;
    private float _finalLimitedRange;

    [Header("Damage Settings")]
    [SerializeField] [Range(0.0f, 60.0f)] public float BaseAttackSpeed = 2.0f;
    private float _finalAttackSpeed;
    private float _cooldown;
    [SerializeField] public float BaseAttackDamage = 1.0f;
    private float _finalAttackDamage;
    [SerializeField] public float BaseIonDamage = 0.0f;
    private float _finalIonDamage;
    [SerializeField] public float BasePiercingDamage = 0.0f;
    private float _finalPiercingDamage;
    [SerializeField] public bool IsExplosive = false;
    [SerializeField] public bool BlastDamagesAllTeams = false;
    [SerializeField] public float BaseBlastRadius = 1.0f;
    private float _finalBlastRadius;

    [Header("Projectile Settings")]
    [SerializeField] public Projectile ProjectilePrefab;
    [SerializeField] public Effect FireEffect;
    [SerializeField] public Effect DryFireEffect;
    [SerializeField] public int BaseProjectileCount = 1;
    private int _finalProjectileCount;
    [SerializeField] public float BaseProjectileSpeed = 20.0f;
    private float _finalProjectileSpeed;
    [SerializeField] public float BaseProjectilePersistence = 5.0f;
    private float _finalProjectilePersistence;
    [SerializeField] public float BaseInaccuracy = 0.0f;
    private float _finalInaccuracy;
    [SerializeField] public bool IsAutomatic = true;
    [SerializeField] public float BaseMaxAutoSpread = 0.0f;
    private float _finalMaxAutoSpread;
    private float _currentAutoSpread;
    [SerializeField] public float BaseAutoSpreadRate = 0.0f;
    private float _finalAutoSpreadRate;
    [SerializeField] [Range(0.0f, 60.0f)] public float BaseAutoSpreadRecovery = 0.0f;
    private float _finalAutoSpreadRecovery;

    [Header("Missile Settings")]
    [SerializeField] public bool IsHoming = false;
    [SerializeField] public float BaseBombInertiaBrakes = 0.0f;
    private float _finalBombInertiaBrakes;

    private void Start()
    {
        //init targeting stats
        _finalLimitedRange = BaseLimitedRange;

        //init damage stats
        _finalAttackSpeed = BaseAttackSpeed;
        _cooldown = 0.0f;
        _finalAttackDamage = BaseAttackDamage;
        _finalIonDamage = BaseIonDamage;
        _finalPiercingDamage = BasePiercingDamage;
        _finalBlastRadius = BaseBlastRadius;

        //init projectile stats
        _finalProjectileCount = BaseProjectileCount;
        _finalProjectileSpeed = BaseProjectileSpeed;
        _finalProjectilePersistence = BaseProjectilePersistence;
        _finalInaccuracy = BaseInaccuracy;
        _finalMaxAutoSpread = BaseMaxAutoSpread;
        _currentAutoSpread = 0.0f;
        _finalAutoSpreadRate = BaseAutoSpreadRate;
        _finalAutoSpreadRecovery = BaseAutoSpreadRecovery;

        //init missile stats
        _finalBombInertiaBrakes = BaseBombInertiaBrakes;
    }

    private void Update()
    {
        //cooldown
        _cooldown -= Time.deltaTime;

        //auto inacuracy recovery
        if (IsAutomatic)
        {
            _currentAutoSpread -= _finalAutoSpreadRecovery * Time.deltaTime;
            if (_currentAutoSpread < 0.0f)
            {
                _currentAutoSpread = 0.0f;
            }
        }
    }

    public bool AutoTrigger(Team firingTeam, float inaccuracy, Transform reticle, float aimAngle)
    {
        if (IsAutomatic)
        {
            if (_cooldown <= 0.0f)
            {
                //fire weapon
                Fire(firingTeam, inaccuracy + _finalInaccuracy + _currentAutoSpread, reticle, aimAngle);

                //become inaccurate
                _currentAutoSpread += _finalAutoSpreadRate / _finalAttackSpeed;
                if (_currentAutoSpread > _finalMaxAutoSpread)
                {
                    _currentAutoSpread = _finalMaxAutoSpread;
                }

                //initiate cooldown
                _cooldown = 1.0f / _finalAttackSpeed;

                //successful fire
                return true;
            }
            else
            {
                //is on cooldown
                return false;
            }
        }
        else
        {
            //no false fire for semi
            return true;
        }
    }

    public bool SemiTrigger(Team firingTeam, float inaccuracy, Transform reticle, float aimAngle)
    {
        if (!IsAutomatic)
        {
            if (_cooldown <= 0.0f)
            {
                //fire weapon
                Fire(firingTeam, inaccuracy + _finalInaccuracy + _currentAutoSpread, reticle, aimAngle);

                //initiate cooldown
                _cooldown = 1.0f / _finalAttackSpeed;

                //successful fire
                return true;
            }
            else
            {
                //is on cooldown - dry fire
                DryFire();
                return false;
            }
        }
        else
        {
            //no false fire for auto
            return true;
        }
    }

    private void DryFire()
    {
        //spawn dry fire effect
        Effect dryFireEffect = (Effect)PoolManager.Instance.Spawn(DryFireEffect.name, transform.position);
        dryFireEffect.Init();
    }

    private void Fire(Team firingTeam, float netInaccuracy, Transform reticle, float aimAngle)
    {
        //spawn fire effect
        Effect fireEffect = (Effect)PoolManager.Instance.Spawn(FireEffect.name, transform.position);
        fireEffect.Init();

        //spawn projectiles
        foreach (Transform barrelTip in BarrelTips)
        {
            for (int i = 0; i < _finalProjectileCount; i++)
            {
                Projectile projectile = (Projectile)PoolManager.Instance.Spawn(ProjectilePrefab.name, barrelTip.transform.position);
                projectile.Init(_finalProjectileSpeed, _finalProjectilePersistence, firingTeam, _finalAttackDamage, _finalIonDamage, _finalPiercingDamage, IsExplosive, BlastDamagesAllTeams, _finalBlastRadius);

                //orient projectile
                switch (AimType)
                {
                    case AimType.LimitedFocused:

                        projectile.transform.up = reticle.position - transform.position;
                        projectile.transform.rotation = Quaternion.Euler(0.0f, 0.0f, projectile.transform.eulerAngles.z + Random.Range(-netInaccuracy / 2, netInaccuracy / 2));

                        //clamp
                        float localFocusedAngle = Vector2.SignedAngle(new Vector2(transform.up.x, transform.up.y), new Vector2(projectile.transform.up.x, projectile.transform.up.y));
                        if (localFocusedAngle > _finalLimitedRange / 2.0f)
                        {
                            projectile.transform.rotation = Quaternion.Euler(0.0f, 0.0f, transform.eulerAngles.z + _finalLimitedRange / 2.0f);
                        }
                        else if (localFocusedAngle < -_finalLimitedRange / 2.0f)
                        {
                            projectile.transform.rotation = Quaternion.Euler(0.0f, 0.0f, transform.eulerAngles.z - _finalLimitedRange / 2.0f);
                        }

                        break;

                    case AimType.LimitedDirectional:

                        projectile.transform.rotation = Quaternion.Euler(0.0f, 0.0f, transform.eulerAngles.z + aimAngle + Random.Range(-netInaccuracy / 2, netInaccuracy / 2));

                        //clamp
                        float localDirectionalAngle = Vector2.SignedAngle(new Vector2(transform.up.x, transform.up.y), new Vector2(projectile.transform.up.x, projectile.transform.up.y));
                        if (localDirectionalAngle > _finalLimitedRange / 2.0f)
                        {
                            projectile.transform.rotation = Quaternion.Euler(0.0f, 0.0f, transform.eulerAngles.z + _finalLimitedRange / 2.0f);
                        }
                        else if (localDirectionalAngle < -_finalLimitedRange / 2.0f)
                        {
                            projectile.transform.rotation = Quaternion.Euler(0.0f, 0.0f, transform.eulerAngles.z - _finalLimitedRange / 2.0f);
                        }

                        break;

                    case AimType.FullFocused:

                        projectile.transform.up = reticle.position - transform.position;
                        projectile.transform.rotation = Quaternion.Euler(0.0f, 0.0f, projectile.transform.eulerAngles.z + Random.Range(-netInaccuracy / 2, netInaccuracy / 2));

                        break;

                    case AimType.FullDirectional:

                        projectile.transform.rotation = Quaternion.Euler(0.0f, 0.0f, transform.eulerAngles.z + aimAngle + Random.Range(-netInaccuracy / 2, netInaccuracy / 2));

                        break;

                    case AimType.Random:

                        projectile.transform.rotation = Quaternion.Euler(0.0f, 0.0f, Random.Range(-180.0f, 180.0f));

                        break;

                    case AimType.Forward:
                    default:

                        projectile.transform.rotation = Quaternion.Euler(0.0f, 0.0f, transform.eulerAngles.z + Random.Range(-netInaccuracy / 2, netInaccuracy / 2));

                        break;
                }
            }
        }
    }
}
