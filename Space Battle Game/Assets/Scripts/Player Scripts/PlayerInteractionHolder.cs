using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractionHolder : MonoBehaviour
{
    public delegate void PlayerInteracted(GameObject triggeringPlayer, int triggeringPlayerID);
    public static event PlayerInteracted OnPlayerInteracted;


    float inputAngle;
    bool fireInputPressed;

    #region New Input System
    //Note: This is being used as an event while the others aren't as it needs to pass out multiple variables rather than just a single one
    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed && this.gameObject.scene.IsValid())
        {
            int playerID = this.gameObject.GetComponent<PlayerInformationHolder>().GetPlayerID();
            OnPlayerInteracted(this.gameObject, playerID);
        }
    }


    public void OnAngleInputPressed(InputAction.CallbackContext context)
    {
        inputAngle = context.ReadValue<float>();
    }
    
    public void OnFireInputPressed(InputAction.CallbackContext context)
    {
        fireInputPressed = context.performed;
    }
    #endregion


    public float GetAngleInput()
    {
        return inputAngle;
    }

    public bool GetFireInput()
    {
        return fireInputPressed;
    }
}
