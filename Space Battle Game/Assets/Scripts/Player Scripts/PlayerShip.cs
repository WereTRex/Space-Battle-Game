using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShip : MonoBehaviour
{
    [Header("Hull")]
    [SerializeField] float currentHull;
    float currentHullPercentage;
    [SerializeField] float maxHull;
    float previousHull;

    [Space(5)]

    [SerializeField] float hullRegenerationRate;

    [SerializeField] float hullRegenerationDelay;
    float hullRegenerationDelayRemaining;

    [Space(5)]
    
    [SerializeField] int numberOfBars;
    float hullBarPercentages;

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

    [Space(20)]


    [Header("UI")]
    [SerializeField] HealthBar healthBar;
    [SerializeField] GameObject gameOverUI;



    void Start()
    {
        currentHull = maxHull;
        currentShields = maxShields;

        shieldRegenerationDelayRemaining = shieldRegenerationDelay;
        hullRegenerationDelayRemaining = hullRegenerationDelay;

        healthBar.Setup(maxHull, maxShields);
    }

    void Update()
    {
        //Death
        if (currentHull <= 0) { Die(); }
        
        
        //Hull
        currentHullPercentage = (100 / maxHull) * currentHull;
        hullBarPercentages = 100 / numberOfBars;

        
        if (previousHull == currentHull)
        {
            hullRegenerationDelayRemaining -= 1 * Time.deltaTime;
        } else {
            hullRegenerationDelayRemaining = hullRegenerationDelay;
        }
        

        //Shields
        if (previousShields == currentShields)
        {
            shieldRegenerationDelayRemaining -= 1 * Time.deltaTime;
        } else {
            shieldRegenerationDelayRemaining = shieldRegenerationDelay;
        }


        if (shieldRegenerationDelayRemaining <= 0)
        {
            RegenerateShields();
        }
        if (hullRegenerationDelayRemaining <= 0)
        {
            if (CheckRepair())
            {
                Repair();
            }
        }


        UpdateUI();
        previousShields = currentShields;
        previousHull = currentHull;
    }

    public void HealHull(float healAmount)
    {
        currentHull += healAmount;
    }
    public void DamageHull(float damage)
    {
        currentHull -= damage;
    }


    void RegenerateShields()
    {
        if (currentShields < maxShields)
        {
            currentShields += shieldRegenerationAmount * Time.deltaTime;
        }
        else
        {
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


    bool CheckRepair()
    {
        if (!(currentHullPercentage >= 100 || currentHullPercentage <= 0))
        {
            if (Mathf.Ceil(currentHullPercentage) % Mathf.Ceil(hullBarPercentages) == 0)
            {
                //Is divisible clearly
                return false;
            } else {
                //Isn't divisible clearly
                return true;
            }
        }
        return false;
    }

    void Repair()
    {
        currentHull += hullRegenerationRate * Time.deltaTime;
    }


    void UpdateUI()
    {
        healthBar.SetHealth(currentHull);
        healthBar.SetShields(currentShields);
    }

    void Die()
    {
        //Game Over
        gameOverUI.SetActive(true);

        //Change Action Maps
        
        //Restart When The Key is Pressed
    }
}
