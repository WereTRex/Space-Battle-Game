using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInformationHolder : MonoBehaviour
{
    int playerID;

    public void SetupPlayer(int newID)
    {
        playerID = newID;
    }


    public int GetPlayerID()
    {
        return playerID;
    }
}
