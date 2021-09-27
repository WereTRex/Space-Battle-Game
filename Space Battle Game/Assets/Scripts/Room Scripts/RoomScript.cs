using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RoomScript : MonoBehaviour
{
    
    public List<GameObject> playersInRoom;
    public GameObject buttonPromptMW;
    
    [Space(20)]
    [Header("Modal Window Varaibles")]
    public GameObject modalWindow;
    public bool windowOpen;

    [Space(20)]
    [Header("Player Based Varaibles")]
    public GameObject controllingPlayer;
    public PlayerInteractionHolder playerInteractionHolder;

    [Tooltip("Set to 0 for no change")] [Min(0)] public int cameraTargetSize;

    private void Awake()
    {
        PlayerInteractionHolder.OnPlayerInteracted += ShowWindow;
        ModalWindowPanel.OnCloseAction += HideWindow;
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
            
            if (!buttonPromptMW.GetComponent<ButtonPromptHider>().openRequests.Contains(this.gameObject))
                buttonPromptMW.GetComponent<ButtonPromptHider>().openRequests.Add(this.gameObject);
        }
        else
        {
            // Hide the button prompt
            if (buttonPromptMW.GetComponent<ButtonPromptHider>().openRequests.Contains(this.gameObject))
                buttonPromptMW.GetComponent<ButtonPromptHider>().openRequests.Remove(this.gameObject);
        }
    }

    void ShowWindow(GameObject player, int triggeringPlayerID)
    {
        if (!playersInRoom.Contains(player)) { return; } //Ensure that the player who is pressing the button is inside the room
        if (windowOpen) { HideWindow(modalWindow); return; } //When this script is triggered BUT the window is already open (e.g. If they press the button again), close the window instead


        Debug.Log("Triggered by player " + triggeringPlayerID);
        Debug.Log(modalWindow.name);

        //Show/Create the Modal Window
        windowOpen = true;
        modalWindow.SetActive(true);
        Debug.Log(modalWindow.activeInHierarchy);

        //Move the player's camera to the desired position
        if (cameraTargetSize != 0)
        {
            player.GetComponent<PlayerInformationHolder>().GetPlayerCameraGO().GetComponent<CameraController>().ZoomCameraOut(cameraTargetSize);
        }

        //Set the Modal Window's controlling player to the playerID
        Debug.Log("Reached the assigning of the controlling player");
        controllingPlayer = player;
        controllingPlayer.GetComponent<PlayerMovement>().inMenu = true;
        playerInteractionHolder = player.GetComponent<PlayerInteractionHolder>();
    }

    void HideWindow(GameObject _modalWindow)
    {
        if (_modalWindow != modalWindow) { return; }

        //Hide/Destroy the Modal Window
        windowOpen = false;
        modalWindow.SetActive(false);

        //Revert the player's camera to its normal size
        controllingPlayer.GetComponent<PlayerInformationHolder>().GetPlayerCameraGO().GetComponent<CameraController>().RevertToOrigonalSize();

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
                HideWindow(modalWindow);
            }
        }
    }
}
