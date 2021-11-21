using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInformationHolder : MonoBehaviour
{
    int playerID;
    [SerializeField] Camera playerCam;

    [Space(5)]

    [Header("UI")]
    [SerializeField] GameObject playerCanvas;

    [SerializeField] GameObject helmUI;
    [SerializeField] GameObject weaponUI;

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


    public GameObject GetPlayerCanvas()
    {
        return playerCanvas;
    }
    public GameObject GetWeaponUI()
    {
        return weaponUI;
    }
    public GameObject GetHelmUI()
    {
        return helmUI;
    }
}
