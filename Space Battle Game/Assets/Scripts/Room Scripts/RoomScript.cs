using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RoomScript : MonoBehaviour
{
    
    public List<GameObject> playersInRoom;
    public GameObject buttonPromptMW;

    public GameObject playerShip;
    
    [Space(20)]
    [Header("Modal Window Varaibles")]
    public GameObject UIWindow;
    public bool windowOpen;

    [Space(20)]
    [Header("Player Based Varaibles")]
    public GameObject controllingPlayer;
    public PlayerInteractionHolder playerInteractionHolder;

    [Space(20)]
    [Header("Player Camera Variables")]
    [Tooltip("Set to 0 for no change")] [Min(0)] public int cameraTargetSize;
    [Tooltip("This is the local position that the camera will attempt to reach")] public Vector2 cameraTargetPosition;
    public LayerMask mask;
    public bool cameraOffsetShipRotation;

    [Space(5)]

    [Tooltip("Leave unset to keep the roof invisible when using this room")] public ShipRoof shipRoof;
    
    

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
        if (windowOpen) { HideWindow(); return; } //When this script is triggered BUT the window is already open (e.g. If they press the button again), close the window instead


        Debug.Log("Triggered by player " + triggeringPlayerID);

        //Show/Create the Modal Window
        windowOpen = true;
        if (UIWindow != null) { UIWindow.SetActive(true); }

        


        CameraController playerCamController = player.GetComponent<PlayerInformationHolder>().GetPlayerCameraGO().GetComponent<CameraController>();

        //Move & Zoom the player's camera to the desired position
        if (cameraTargetSize != 0)
        {
            playerCamController.ZoomCameraOut(cameraTargetSize);
        }
        playerCamController.MoveCameraPosition(cameraTargetPosition);
        playerCamController.SetOffsetShipRotation(cameraOffsetShipRotation);

        //Fade in the ship roof & allow the playerCam to see it
        if (shipRoof != null) { shipRoof.StartFadeIn(); }
        playerCamController.SetCullingMask(mask);


        //Set the Modal Window's controlling player to the playerID
        Debug.Log("Reached the assigning of the controlling player");
        controllingPlayer = player;
        controllingPlayer.GetComponent<PlayerMovement>().inMenu = true;
        playerInteractionHolder = player.GetComponent<PlayerInteractionHolder>();
    }

    void HideWindow()
    {
        //Hide/Destroy the Modal Window
        windowOpen = false;

        if (UIWindow != null) { UIWindow.SetActive(false); }

        //Revert the player's camera to its normal size
        CameraController playerCamController = controllingPlayer.GetComponent<PlayerInformationHolder>().GetPlayerCameraGO().GetComponent<CameraController>();

        playerCamController.RevertToOrigonalSize();
        playerCamController.RevertToOrigonalPosition();
        playerCamController.SetOffsetShipRotation(false);

        //Fade out the ship roof
        if (shipRoof != null) { shipRoof.StartFadeOut(); }
        playerCamController.RevertCullingMask();


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
