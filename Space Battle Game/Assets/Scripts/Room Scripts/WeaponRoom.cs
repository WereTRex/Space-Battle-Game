using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class WeaponRoom : RoomScript
{
    [Header("Angles")]
    float currentAngle;
    float targetAngle;
    [SerializeField] float turnSpeed;
    [Space(20)]


    [Header("Firing Variables")]
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform shipCenter;

    [Space(5)]

    [SerializeField] float projectileSpeed;
    [SerializeField] float cooldownTime;
    float cooldownTimeRemaining;
    [Space(20)]


    [Header("UI Variables")]
    [SerializeField] TextMeshProUGUI currentAngleText;
    [SerializeField] TextMeshProUGUI targetAngleText;
    

    void Update()
    {
        CheckIfDisplayButtonPrompt();


        if (controllingPlayer == null || !modalWindow.activeInHierarchy) { return; }
        //Move the current turret angle towards the target angle
        MoveCurrentAngleToTargetAngle();

        //Allow the player to change the angle where the cannons try to aim
        ReadAngleInputAndUpdateTargetAngle();

        //Update UI
        UpdateUI();

        //Check if the fire button has been pressed & if so then fire a projectile out in the direction of currentAngle
        if (CheckFireInput() && cooldownTimeRemaining <= 0)
        {
            cooldownTimeRemaining = cooldownTime;
            FireWeapons();
        }
        if (cooldownTimeRemaining > 0)
        {
            cooldownTimeRemaining -= Time.deltaTime;
        }
    }


    void MoveCurrentAngleToTargetAngle()
    {
        if (currentAngle > targetAngle - 0.2f && currentAngle < targetAngle + 0.2f) { return; } //Stops the current angle from constantly updating when it can't exaclty reach the target angle
        
        if (currentAngle > targetAngle)
        {
            currentAngle -= turnSpeed * Time.deltaTime;
        } else if (currentAngle < targetAngle) {
            currentAngle += turnSpeed * Time.deltaTime;
        }
    }


    void ReadAngleInputAndUpdateTargetAngle()
    {
        if (playerInteractionHolder.GetAngleInput() > 0.1f)
        {
            targetAngle -= 60 * Time.deltaTime;
        }
        else if (playerInteractionHolder.GetAngleInput() < -0.1f)
        {
            targetAngle += 60 * Time.deltaTime;
        }


        if (targetAngle < 0)
        {
            targetAngle += 360;
            currentAngle += 360;
        }
        else if (targetAngle > 360)
        {
            targetAngle -= 360;
            currentAngle -= 360;
        }
    }

    
    bool CheckFireInput()
    {
        return playerInteractionHolder.GetFireInput();
    }

    void FireWeapons()
    {
        Debug.Log("The weapon has been fired!");

        //Spawn a prefab that is facing currentDirection
        GameObject pfBullet = Instantiate(projectilePrefab,
            new Vector3(shipCenter.position.x, shipCenter.position.y, 5),
            Quaternion.Euler(0, 0, currentAngle)); //Note: If you add 90 to the current angle it will make it so that 0° is straight up

        pfBullet.GetComponent<PlayerShipBullet>().SetupBullet(projectileSpeed);
    }


    void UpdateUI()
    {
        if (modalWindow.activeInHierarchy)
        {
            if (currentAngle < 0)
                currentAngleText.text = 0 + "°";
            else if (currentAngle > 360)
                currentAngleText.text = 360 + "°";
            else
                currentAngleText.text = Mathf.RoundToInt(currentAngle) + "°";


            targetAngleText.text = Mathf.RoundToInt(targetAngle) + "°";
        }
    }
}
