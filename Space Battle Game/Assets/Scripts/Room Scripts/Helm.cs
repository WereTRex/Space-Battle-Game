using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Helm : RoomScript
{
    float currentSpeed;
    [SerializeField] float acceleration;


    float currentHeading;
    [SerializeField] float turnSpeed;
    [SerializeField] float maxRotationSpeed;

    [SerializeField] float horizontalInputAcceleration;
    private float zRotationVelocity;
    [SerializeField] float rotationDrag = 1f;


    [SerializeField] bool fullStop;
    [SerializeField] float fullStopSpeed;


    float angleInput;
    float accelerationInput;

    [SerializeField] Rigidbody2D restOfLevelRb2D;
    [SerializeField] Transform shipTransform;
    [SerializeField] Rigidbody2D rb2D;

    [Space(20)]
    [Header("UI Variables")]
    [SerializeField] TextMeshProUGUI currentSpeedText;
    [SerializeField] TextMeshProUGUI currentHeadingText;

    void Update()
    {
        // apply turn input
        float zTurnAcceleration = -1 * angleInput * horizontalInputAcceleration;
        zRotationVelocity += zTurnAcceleration * Time.deltaTime;
    }

    void FixedUpdate()
    {
        CheckIfDisplayButtonPrompt();

        if (controllingPlayer == null || !modalWindow.activeInHierarchy) { return; }

        //Get player input (Used for speed & angle)
        GetPlayerInput();

        //Update the UI
        UpdateUI();

        //'Move' the ship by applying a force to the RigidBody2D of the rest of the level
        restOfLevelRb2D.AddForce(-transform.right * accelerationInput * acceleration);

        //Check how far you moved and if it is a small amount (Or check if you are colliding with something), then reduce speed

        //Rotate the ship
        zRotationVelocity = zRotationVelocity * (1 - Time.deltaTime * rotationDrag);
        zRotationVelocity = Mathf.Clamp(zRotationVelocity, -maxRotationSpeed, maxRotationSpeed);
        
        shipTransform.Rotate(0, 0, zRotationVelocity * Time.deltaTime);


        //Optional: If the player presses space then decelerate/accelerate them towards 0 and when it gets within 1f make it 0 (Autopilot brake)
        if (fullStop)
        {
            Debug.Log(0.9f * Time.deltaTime);
            restOfLevelRb2D.velocity = restOfLevelRb2D.velocity * (1-(0.9f * Time.deltaTime));
        }
    }


    void GetPlayerInput()
    {
        // Acceleration Input (W&S / R2 & L2)
        accelerationInput = playerInteractionHolder.GetAccelerateInput();

        // Angle Input (A&D)
        angleInput = playerInteractionHolder.GetAngleInput();

        // Full Stop (Space)
        fullStop = playerInteractionHolder.GetSpacePressed();
    }

    void UpdateUI()
    {
        currentSpeed = Mathf.RoundToInt(restOfLevelRb2D.velocity.magnitude);

        currentSpeedText.text = currentSpeed.ToString() + " u/s";
        currentHeadingText.text = currentHeading.ToString() + "°";
    }
}
