using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Helm : RoomScript
{
    [Header("Acceleration Variables")]
    float currentSpeed;
    [SerializeField] float acceleration;
    [SerializeField] float maxSpeed = 30f;


    [Space(20)]
    [Header("Turning Variables")]
    float currentHeading;
    [SerializeField] float turnSpeed;
    [SerializeField] [Min(0)] float maxRotationSpeed;

    [Space(5)]
    //Smooth rotation variables
    [SerializeField] float horizontalInputAcceleration;
    private float zRotationVelocity;
    [SerializeField] float rotationDrag = 1f;


    [Space(20)]
    [Header("Full Stop Variables")]
    [SerializeField] bool fullStop;
    [SerializeField] [Range(0f, 1f)] float fullStopSpeed = 0.1f;


    float angleInput;
    float accelerationInput;


    [Space(20)]
    [Header("Other Variables")]
    [SerializeField] Rigidbody2D restOfLevelRb2D;
    [SerializeField] Transform shipTransform;
    [SerializeField] Rigidbody2D rb2D;

    [Space(20)]
    [Header("UI Variables")]
    [SerializeField] TextMeshProUGUI currentHeadingText;
    [SerializeField] TextMeshProUGUI velocityXText, velocityYText, velocityNegXText, velocityNegYText;
    

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
            restOfLevelRb2D.velocity = restOfLevelRb2D.velocity * (fullStopSpeed * Time.deltaTime);
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
        //Run through the 4 velocity directions (x, y, -x, -y)      Note: This wont work :(
        velocityXText.text = Mathf.RoundToInt(restOfLevelRb2D.velocity.x).ToString();
        velocityYText.text = Mathf.RoundToInt(restOfLevelRb2D.velocity.y).ToString();
        velocityNegXText.text = Mathf.Clamp(-restOfLevelRb2D.velocity.x, 0, -restOfLevelRb2D.velocity.x).ToString();
        velocityNegYText.text = Mathf.Clamp(-restOfLevelRb2D.velocity.y, 0, -restOfLevelRb2D.velocity.y).ToString();

        //currentHeadingText.text = currentHeading.ToString() + "°";
    }

    public Vector2 GetVelocity()
    {
        return restOfLevelRb2D.velocity;
    }

    public Vector2 GetPosition()
    {
        return restOfLevelRb2D.position;
    }

    public float GetMaxSpeed()
    {
        return maxSpeed;
    }
}
