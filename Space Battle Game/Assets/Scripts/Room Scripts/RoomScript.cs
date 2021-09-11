using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RoomScript : MonoBehaviour
{
    public List<GameObject> playersInRoom;
    public GameObject buttonPromptMW;
    public GameObject modalWindow;

    public GameObject controllingPlayer;
    public PlayerInteractionHolder playerInteractionHolder;

    private void Awake()
    {
        PlayerInteractionHolder.OnPlayerInteracted += ShowWindow;
    }

    void Update()
    {
        CheckIfDisplayButtonPrompt();
    }

    public void CheckIfDisplayButtonPrompt()
    {
        //If there is a player in the room:
        if (playersInRoom.Count > 0)
        {
            // Display the button prompt
            buttonPromptMW.SetActive(true);
        }
        else
        {
            // Hide the button prompt
            buttonPromptMW.SetActive(false);
        }
    }

    void ShowWindow(GameObject player, int triggeringPlayerID)
    {
        if (!playersInRoom.Contains(player)) { return; } //Ensure that the player who is pressing the button is inside the room
        
        Debug.Log("Triggered by player " + triggeringPlayerID);

        //Show/Create the Modal Window

        //Set the Modal Window's controlling player to the playerID
        controllingPlayer = player;
        controllingPlayer.GetComponent<PlayerMovement>().inMenu = true;
        playerInteractionHolder = player.GetComponent<PlayerInteractionHolder>();
    }

    void HideWindow()
    {
        //Hide/Destroy the Modal Window

        //Set the controlling player to null
        controllingPlayer.GetComponent<PlayerMovement>().inMenu = false;
        controllingPlayer = null;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        //Check if the collider is a player's (And that they aren't already in the room)
        if (other.CompareTag("Player") && !(playersInRoom.Contains(other.gameObject)))
        {
            // Mark that player as being in the room
            playersInRoom.Add(other.gameObject);
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        //Check of the collider is a player's who was in the room
        if (other.CompareTag("Player"))
        {
            // Remove that player from the room
            playersInRoom.Remove(other.gameObject);
            if (other.gameObject == controllingPlayer)
            {
                HideWindow();
            }
        }
    }
}
