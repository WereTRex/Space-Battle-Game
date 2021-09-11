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
    [SerializeField] float projectileSpeed;
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
        if (CheckFireInput())
            FireWeapons();
    }


    void MoveCurrentAngleToTargetAngle()
    {
        if (currentAngle != targetAngle)
        {
            if (currentAngle > targetAngle)
            {
                currentAngle -= turnSpeed * Time.deltaTime;
            }
            else
            {
                currentAngle += turnSpeed * Time.deltaTime;
            }
        }
    }


    void ReadAngleInputAndUpdateTargetAngle()
    {
        if (playerInteractionHolder.GetAngleInput() > 0.1f)
        {
            targetAngle += 60 * Time.deltaTime;
        }
        else if (playerInteractionHolder.GetAngleInput() < -0.1f)
        {
            targetAngle -= 60 * Time.deltaTime;
        }
    }

    
    bool CheckFireInput()
    {
        return playerInteractionHolder.GetFireInput();
    }

    void FireWeapons()
    {

    }


    void UpdateUI()
    {
        if (modalWindow.activeInHierarchy)
        {
            currentAngleText.text = Mathf.RoundToInt(currentAngle) + "°";
            targetAngleText.text = Mathf.RoundToInt(targetAngle) + "°";
        }
    }
}
