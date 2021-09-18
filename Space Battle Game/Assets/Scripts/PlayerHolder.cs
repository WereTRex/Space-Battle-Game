using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHolder : MonoBehaviour
{
    public void OnPlayerJoined(PlayerInput playerInput)
    {
        Debug.Log("Player: " + playerInput.name + " has been moved");
        playerInput.transform.parent = this.transform;
    }
}
