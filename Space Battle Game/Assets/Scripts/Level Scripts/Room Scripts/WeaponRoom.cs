using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class WeaponRoom : RoomScript
{
    [Space(5)]
    [Header("Angles")]
    [SerializeField] float turnSpeed;
    float targetAngle;

    float inputValue;
    [Space(20)]


    [Header("Firing Variables")]
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform shipCenter;

    [Space(5)]

    [SerializeField] Weapon[] weapons;
    bool isZero;

    [SerializeField] float cooldownTime;
    float cooldownTimeRemaining;
    [Space(20)]


    [Header("UI Variables")]
    [SerializeField] WeaponUI UIScript;
    float previousWeaponCount;
    float previousTargetAngle;


    void Update()
    {   
        CheckIfDisplayButtonPrompt();

        if (controllingPlayer == null)
        {
            if (!isZero)
            {
                foreach (Weapon weapon in weapons)
                {
                    weapon.currentAngle = 0;
                }

                isZero = true;
                targetAngle = 0;
            } 
            return;
        }
        isZero = false;

        //Move the current turret angle towards the target angle
        MoveCurrentAngleToTargetAngle();

        //Allow the player to change the angle where the cannons try to aim
        ReadAngleInputAndUpdateTargetAngle();

        //Update UI
        UpdateUI();
        if (previousWeaponCount != weapons.Length ||  UIScript.GetNumberOfCrosshairs() <= 0)
        {
            StartCoroutine(UIScript.CreateAndRemoveCrosshairs(weapons.Length));
            Debug.Log("Added/Removed Crosshairs");
        }

        //Check if the fire button has been pressed & if so then fire a projectile out in the direction of currentAngle
        if (CheckFireInput())
        {
            FireWeapons();
        }


        foreach (Weapon weapon in weapons)
        {
            weapon.cooldownTimeRemaining -= 1 * Time.deltaTime;
        }


        if (targetAngle != previousTargetAngle)
        {
            //Allows the main crosshair to move
            UIScript.TargetAngleChanged(targetAngle);
        }

        previousWeaponCount = weapons.Length;
        previousTargetAngle = targetAngle;
    }


    void MoveCurrentAngleToTargetAngle()
    {
        foreach (Weapon weapon in weapons)
        {
            if (weapon.currentAngle > targetAngle - 0.2f && weapon.currentAngle < targetAngle + 0.2f)
            {
                //Stop the current angle from constantly updating when it can't exaclty reach the target angle
                weapon.currentAngle = targetAngle;
            }
            else if (weapon.currentAngle > targetAngle)
            {
                weapon.currentAngle -= weapon.turnSpeed * Time.deltaTime;
            }
            else if (weapon.currentAngle < targetAngle)
            {
                weapon.currentAngle += weapon.turnSpeed * Time.deltaTime;
            }
        }
    }


    void ReadAngleInputAndUpdateTargetAngle()
    {
        inputValue = playerInteractionHolder.GetAngleInput();
        inputValue = Mathf.Clamp(inputValue, -1, 1);

        if (inputValue > 0.1f)
        {
            targetAngle -= turnSpeed * inputValue * Time.deltaTime;
        }
        else if (inputValue < -0.1f)
        {
            targetAngle += turnSpeed * -inputValue * Time.deltaTime;
        }


        if (targetAngle < 0)
        {
            targetAngle += 360;
            foreach (Weapon weapon in weapons)
                weapon.currentAngle += 360;
        }
        else if (targetAngle > 360)
        {
            targetAngle -= 360;
            foreach (Weapon weapon in weapons)
                weapon.currentAngle -= 360;
        }
    }

    
    bool CheckFireInput()
    {
        return playerInteractionHolder.GetFireInput();
    }

    void FireWeapons()
    {
        GameObject pfBullet;

        foreach (Weapon weapon in weapons)
        {
            if (weapon.cooldownTimeRemaining <= 0)
            {
                //Spawn a prefab that is facing currentDirection
                pfBullet = Instantiate(projectilePrefab,
                    new Vector3(transform.position.x, transform.position.y, 7),
                    Quaternion.Euler(0, 0, weapon.currentAngle + playerShip.transform.eulerAngles.z)); //Note: If you add 90 to the current angle it will make it so that 0° is straight up. Note 2: I am subtracting the 4 to get it to line up with the UI


                pfBullet.GetComponent<PlayerShipBullet>().SetupBullet(weapon.damage, weapon.bulletSpeed, playerShip);

                weapon.cooldownTimeRemaining = weapon.cooldownTime;

                Destroy(pfBullet, weapon.lifeTime);
            }
        }
    }


    void UpdateUI()
    {
        if (UIWindow == null) { return; }
        
        if (UIWindow.activeInHierarchy)
        {
            List<float> temp = new List<float>();
            foreach (Weapon weapon in weapons)
            {
                temp.Add(weapon.currentAngle);
            }

            UIScript.RecieveValues(inputValue, turnSpeed, temp);
        }
    }

    public override void GetPlayerUI()
    {
        UIWindow = controllingPlayer.GetComponent<PlayerInformationHolder>().GetWeaponUI();
        UIScript = UIWindow.GetComponent<WeaponUI>();
    }
}
