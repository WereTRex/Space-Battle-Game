using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class ShieldRoom : RoomScript
{
    [Space(20)]

    [Header("Shields")]

    [SerializeField] float maxShieldHealth;
    [SerializeField] float currentShieldHealth;

    [Space(5)]

    [SerializeField] float shieldRegenerationDelay;
    float shieldRegenerationDelayRemaining;
    [SerializeField] float shieldRegenerationAmount;

    float previousShields;

    [Space(5)]

    [SerializeField] Transform shieldObject;
    [SerializeField] SpriteShapeRenderer shieldObjectSprite;
    [SerializeField] float rotateRate;

    float inputValue;

    private void Start()
    {
        currentShieldHealth = maxShieldHealth;

        if (shieldObjectSprite == null)
            shieldObjectSprite = shieldObject.gameObject.GetComponentInChildren<SpriteShapeRenderer>();
    }

    void Update()
    {
        CheckIfDisplayButtonPrompt();


        //Shields
        if (previousShields == currentShieldHealth)
        {
            shieldRegenerationDelayRemaining -= 1 * Time.deltaTime;
        }
        else
        {
            shieldRegenerationDelayRemaining = shieldRegenerationDelay;
        }

        if (shieldRegenerationDelayRemaining <= 0)
        {
            RegenerateShields();
        }


        if (currentShieldHealth < 0)
        {
            shieldObject.gameObject.SetActive(false);
        }
        else
        {
            shieldObjectSprite.color = new Color(shieldObjectSprite.color.r, shieldObjectSprite.color.g, shieldObjectSprite.color.b, 255 * (currentShieldHealth / maxShieldHealth));
            shieldObject.gameObject.SetActive(true);
        }


        if (controllingPlayer != null)
        {

        //Get Inputs
        inputValue = -playerInteractionHolder.GetAngleInput();

        //Rotate Shield
        RotateShield();
        }

        previousShields = currentShieldHealth;
    }


    void RotateShield()
    {
        shieldObject.RotateAround(Vector2.zero, Vector3.forward, inputValue * rotateRate * Time.deltaTime);
    }

    void RegenerateShields()
    {
        if (currentShieldHealth < maxShieldHealth)
        {
            currentShieldHealth += shieldRegenerationAmount * Time.deltaTime;
        }
        else
        {
            currentShieldHealth = maxShieldHealth;
        }
    }


    public void DamageShield(float damage)
    {
        currentShieldHealth -= damage;
    }
}
