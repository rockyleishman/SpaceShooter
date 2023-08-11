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
    Backward,
    Random,
    Aimed
}

public enum Team
{
    Player,
    Enemy,
    Neutral,
    None
}

public class Weapon : MonoBehaviour
{
    [Header("Barrel Tips")]
    [SerializeField] public Transform[] BarrelTips;

    [Header("Weapon Type")]
    [SerializeField] public WeaponType WeaponType = WeaponType.Projectile;

    [Header("Damage Settings")]
    [SerializeField] public float BaseAttackSpeed = 2.0f;
    private int _finalAttackSpeed;
    [SerializeField] public float BaseAttackDamage = 1.0f;
    private int _finalAttackDamage;
    [SerializeField] public float BaseIonDamage = 0.0f;
    private int _finalIonDamage;
    [SerializeField] public float BasePiercingDamage = 0.0f;
    private int _finalPiercingDamage;
    [SerializeField] public bool DoesBlastDamage = false;

    [Header("Projectile Settings")]
    [SerializeField] public Projectile ProjectilePrefab;
    [SerializeField] public float BaseProjectileSpeed = 20.0f;
    private float _finalProjectileSpeed;
    [SerializeField] public float BaseInaccuracy = 0.0f;
    private float _finalInaccuracy;


    [SerializeField] public bool IsHoming = false;
    
    [SerializeField] public float BaseBlastRadius = 1.0f;
    private int _finalBlastRadius;

    public void Trigger()
    {
        
    }

    private void Fire()
    {
        
    }
}
