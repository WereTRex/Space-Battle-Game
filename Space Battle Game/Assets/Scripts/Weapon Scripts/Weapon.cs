using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Weapon
{
    public float damage;
    public float fireRange;

    public float lifeTime;


    public float bulletSpeed;
    public GameObject bulletPrefab;

    public bool beam;


    [Header("Fire Rate")]
    public float cooldownTime;
    [Tooltip("Don't touch this variable")] public float cooldownTimeRemaining;
}