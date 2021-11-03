using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Weapon
{
    public float damage;
    public float lifeTime;

    public float bulletSpeed;
    public GameObject bulletPrefab;

    public bool beam;

    [Space(5)]
    public float turnSpeed;
    [Tooltip("Don't touch this variable")] public float currentAngle;

    [Space(5)]
    [Header("Fire Rate")]
    public float cooldownTime;
    [Tooltip("Don't touch this variable")] public float cooldownTimeRemaining;
}

[System.Serializable]
public class EnemyWeapon
{
    public float damage;
    
    public float fireRange;
    [Tooltip("The angle of the firing cone for the weapon")] public float fireAngle;
    [Tooltip("Rotates the firing cone of the gun")] public float fireRotation;

    public float lifeTime;

    [Header("Firing Variables")]
    public Transform firePosition;
    public float bulletSpeed;
    public GameObject bulletPrefab;

    public bool beam;


    [Header("Fire Rate")]
    public float cooldownTime;
    [Tooltip("Don't touch this variable")] public float cooldownTimeRemaining;
}