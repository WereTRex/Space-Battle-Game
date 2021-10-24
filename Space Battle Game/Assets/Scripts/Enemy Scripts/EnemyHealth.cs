using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Hull")]
    [SerializeField] float currentHull;
    [SerializeField] float maxHull;
    
    [Space(5)]
    [SerializeField] bool repairHull;
    [Space(5)]

    [SerializeField] float hullRepairDelay;
    float hullRepairDelayRemaining;
    float previousHull;

    [Space(5)]

    [SerializeField] float hullRepairAmount;

    [Space(20)]


    [Header("Shields")]
    [SerializeField] float currentShields;
    [SerializeField] float maxShields;

    [Space(5)]
    [SerializeField] bool regenerateShields;
    [Space(5)]

    [SerializeField] float shieldRegenerationDelay;
    float shieldRegenerationDelayRemaining;
    float previousShields;

    [Space(5)]

    [SerializeField] float shieldRegenerationAmount;


    void Start()
    {
        currentHull = maxHull;
        currentShields = maxShields;

        shieldRegenerationDelayRemaining = shieldRegenerationDelay;
    }

    void Update()
    {
        //Hull
        if (previousHull == currentHull)
        {
            if (repairHull == true)
            {
                hullRepairDelayRemaining -= 1 * Time.deltaTime;
            }
        } else {
            hullRepairDelayRemaining = hullRepairDelay;
        }
        

        //Shields
        if (previousShields == currentShields)
        {
            shieldRegenerationDelayRemaining -= 1 * Time.deltaTime;
        } else {
            shieldRegenerationDelayRemaining = shieldRegenerationDelay;
        }


        if (hullRepairDelayRemaining <= 0)
        {
            RepairHull();
        }
        if (shieldRegenerationDelayRemaining <= 0)
        {
            RegenerateShields();
        }

        previousHull = currentHull;
        previousShields = currentShields;
    }


    void RepairHull()
    {
        if (currentHull < maxHull)
        {
            currentHull += hullRepairAmount * Time.deltaTime;
        } else {
            currentHull = maxHull;
        }
    }

    public void HealHull(float healAmount)
    {
        currentHull += healAmount;
    }
    public void DamageHull(float damage)
    {
        currentHull -= damage;
    }
    public void StartStopRepairingHull(bool startOrStop)
    {
        repairHull = startOrStop;
    }


    void RegenerateShields()
    {
        if (currentShields < maxShields)
        {
            currentShields += shieldRegenerationAmount * Time.deltaTime;
        } else {
            currentShields = maxShields;
        }
    }
    public void HealShields(float shieldRegenerationAmount)
    {
        currentShields += shieldRegenerationAmount;
    }
    public void DamageShields(float damage)
    {
        currentShields -= damage;
    }


    public void GetHull(out float _currentHealth, out float _maxHealth)
    {
        _currentHealth = currentHull;
        _maxHealth = maxHull;
    }
    public void GetShields(out float _currentShields, out float _maxShields)
    {
        _currentShields = currentShields;
        _maxShields = maxShields;
    }


    public void DealDamage(float damage)
    {
        if (currentShields > 0)
        {
            currentShields -= damage;
            if (currentShields < 0) { damage = -currentShields; } else { damage = 0; }
        }

        currentHull -= damage;
    }
}
