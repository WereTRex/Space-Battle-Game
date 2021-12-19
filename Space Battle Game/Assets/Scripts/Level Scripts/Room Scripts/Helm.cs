using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Helm : RoomScript
{
    [Space(5)]
    [Header("Acceleration Variables")]
    [SerializeField] float acceleration;
    float currentSpeed;
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
    [SerializeField] [Range(0f, 10f)] float fullStopSpeed = 4f;


    float angleInput;
    float accelerationInput;


    [Space(20)]
    [Header("Other Variables")]
    [SerializeField] Rigidbody2D restOfLevelRb2D;
    [SerializeField] Transform shipTransform;
    [SerializeField] Rigidbody2D rb2D;

    [Space(20)]
    [Header("UI Variables")]
    [SerializeField] TextMeshProUGUI velocityXText, velocityYText, velocityNegXText, velocityNegYText;
    

    void Update()
    {
        CheckIfDisplayButtonPrompt();

        // apply turn input
        float zTurnAcceleration = -1 * angleInput * horizontalInputAcceleration;
        zRotationVelocity += zTurnAcceleration * Time.deltaTime;
    }

    void FixedUpdate()
    {
        if (controllingPlayer == null) { return; }

        //Get player input (Used for speed & angle)
        GetPlayerInput();
        Debug.Log(accelerationInput);

        //'Move' the ship by applying a force to the RigidBody2D of the rest of the level
        restOfLevelRb2D.AddForce(-transform.right * accelerationInput * acceleration);
        
        ConstrainVelocity();

        //Update the UI
        UpdateUI();


        //Rotate the ship
        zRotationVelocity = zRotationVelocity * (1 - Time.deltaTime * rotationDrag);
        zRotationVelocity = Mathf.Clamp(zRotationVelocity, -maxRotationSpeed, maxRotationSpeed);
        
        shipTransform.Rotate(0, 0, zRotationVelocity * Time.deltaTime);


        //Optional: If the player presses space then decelerate/accelerate them towards 0 and when it gets within 1f make it 0 (Autopilot brake)
        if (fullStop)
        {
            restOfLevelRb2D.velocity *= 1 - (fullStopSpeed * Time.deltaTime);
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
        velocityXText.text = Mathf.RoundToInt(Mathf.Clamp(-restOfLevelRb2D.velocity.x, 0, -restOfLevelRb2D.velocity.x)).ToString();
        velocityYText.text = Mathf.RoundToInt(Mathf.Clamp(-restOfLevelRb2D.velocity.y, 0, -restOfLevelRb2D.velocity.y)).ToString();
        velocityNegXText.text = Mathf.RoundToInt(Mathf.Clamp(restOfLevelRb2D.velocity.x, 0, restOfLevelRb2D.velocity.x)).ToString();
        velocityNegYText.text = Mathf.RoundToInt(Mathf.Clamp(restOfLevelRb2D.velocity.y, 0, restOfLevelRb2D.velocity.y)).ToString();

        //currentHeadingText.text = currentHeading.ToString() + "°";
    }

    void ConstrainVelocity()
    {
        if (-restOfLevelRb2D.velocity.x > maxSpeed)
        {
            restOfLevelRb2D.velocity = new Vector2(-maxSpeed, restOfLevelRb2D.velocity.y);
        } else if (-restOfLevelRb2D.velocity.x < -maxSpeed) {
            restOfLevelRb2D.velocity = new Vector2(maxSpeed, restOfLevelRb2D.velocity.y);
        }

        if (-restOfLevelRb2D.velocity.y > maxSpeed)
        {
            restOfLevelRb2D.velocity = new Vector2(restOfLevelRb2D.velocity.x, -maxSpeed);
        } else if (-restOfLevelRb2D.velocity.y < -maxSpeed) {
            restOfLevelRb2D.velocity = new Vector2(restOfLevelRb2D.velocity.x, maxSpeed);
        }
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


    public override void GetPlayerUI()
    {
        UIWindow = controllingPlayer.GetComponent<PlayerInformationHolder>().GetHelmUI();
    }
}
