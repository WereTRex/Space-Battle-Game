using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    int currentPlayerCount;
    List<int> takenIDs = new List<int>();

    public void OnPlayerJoined(PlayerInput playerInput)
    {
        currentPlayerCount += 1;
        Debug.Log("Player " + currentPlayerCount + " joined");

        //Give the player an ID
        int generatedID = (Random.Range(0, 9999));
        while (takenIDs.Contains(generatedID))
        {
            Debug.Log("Had to change playerID");
            generatedID = (Random.Range(0, 9999));
        }

        playerInput.gameObject.GetComponent<PlayerInformationHolder>().SetupPlayer(generatedID);
        takenIDs.Add(generatedID);

        //Add a name to the player
    }

    public void OnPlayerLeft()
    {
        currentPlayerCount -= 1;
    }
}
