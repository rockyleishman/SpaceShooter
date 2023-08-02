using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    Projectile,
    Missile,
    Laser
}

public class Weapon : MonoBehaviour
{
    [SerializeField] public WeaponType WeaponType = WeaponType.Projectile;
    [SerializeField][Range(1, 20)] public int WeaponCountBase = 1;
    private int _weaponCountFinal;
    [SerializeField] [Range(1, 1000)] public int MagazineSizeBase = 10;

}
