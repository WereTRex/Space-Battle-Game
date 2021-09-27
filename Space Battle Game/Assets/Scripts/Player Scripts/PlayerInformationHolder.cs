using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInformationHolder : MonoBehaviour
{
    int playerID;
    [SerializeField] Camera playerCam;

    public void SetupPlayer(int newID)
    {
        playerID = newID;
    }


    public int GetPlayerID()
    {
        return playerID;
    }

    public Camera GetPlayerCamera()
    {
        return playerCam;
    }
    public GameObject GetPlayerCameraGO()
    {
        return playerCam.gameObject;
    }
}
