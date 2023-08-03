using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : PoolObject
{
    [SerializeField] public float ProjectileSpeed;
    [SerializeField] public float AttackDamage;
    [SerializeField] public float IonDamage;
    [SerializeField] public float PiercingDamage;
    [SerializeField] public bool IsHoming;
    [SerializeField] public bool DoesBlastDamage;
    [SerializeField] public float BlastRadius;
}
