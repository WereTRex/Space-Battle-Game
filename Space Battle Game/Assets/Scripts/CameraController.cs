using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("General Variables")]
    [SerializeField] Camera playerCam;

    [SerializeField] GameObject playerShip;
    [SerializeField] bool offsetShipRotation;
    [SerializeField] Quaternion originalRotation;
    [Space(20)]


    [Header("Moving Variables")]
    [SerializeField] Vector2 targetPos;
    [SerializeField] Vector2 originalPos;
    [SerializeField] bool moveToTarget;

    [Space(5)]

    [SerializeField] float moveSpeed;
    [SerializeField] [Range(0f, 1f)] float movementSmoothingValue;
    [Space(20)]


    [Header("Zoom Variables")]
    [SerializeField] float targetSize;
    [SerializeField] bool zoomToTarget;

    [Space(5)]

    [SerializeField] float zoomRate;
    [SerializeField] [Range(0f, 1f)] float zoomSmoothingValue;

    [Space(5)]

    [SerializeField] float originalSize;

    [Space(20)]

    [Header("Layer Masks")]
    LayerMask originalLayerMask;


    private void Start()
    {
        if (originalSize == 0) {
            originalSize = playerCam.orthographicSize;
        }
        if (originalRotation == new Quaternion(0, 0, 0, 0)) {
            originalRotation = playerCam.transform.rotation;
        }
        if (originalPos == new Vector2(0, 0)) {
            originalPos = playerCam.transform.position;
        }

        originalLayerMask = playerCam.cullingMask;

        if (playerShip == null) {
            playerShip = FindObjectOfType<PlayerShip>().gameObject;
        }
    }

    private void Update()
    {
        //Zooming
        if (zoomToTarget)
        {
            //Smoothing Calculations
            float frameSmoothing = 1f;
            if (zoomSmoothingValue != 0)
            {
                frameSmoothing = zoomSmoothingValue * (targetSize - playerCam.orthographicSize);
                if (frameSmoothing < 0) { frameSmoothing = -frameSmoothing; }
            }

            //Change the playerCam's orthographic size towards the target size (Unless it is within 0.1f of the target)
            if ((playerCam.orthographicSize <= targetSize + 0.5f) && (playerCam.orthographicSize >= targetSize - 0.5f))
            {
                playerCam.orthographicSize = targetSize;
                zoomToTarget = false;
            }
            else if (targetSize > playerCam.orthographicSize)
            {
                playerCam.orthographicSize += zoomRate * frameSmoothing * Time.deltaTime;
            }
            else if (targetSize < playerCam.orthographicSize)
            {
                playerCam.orthographicSize -= zoomRate * frameSmoothing * Time.deltaTime;
            }
        }

        //Moving
        if (moveToTarget)
        {
            //Smoothing Calculations
            float frameSmoothing = 1f;
            if (zoomSmoothingValue != 0)
            {
                frameSmoothing = movementSmoothingValue * (targetSize - playerCam.orthographicSize);
                if (frameSmoothing < 0) { frameSmoothing = -frameSmoothing; }
            }

            if ((playerCam.transform.localPosition.x <= targetPos.x + 0.1f && playerCam.transform.localPosition.x >= targetPos.x - 0.1f)
                && (playerCam.transform.localPosition.y <= targetPos.y + 0.1f && playerCam.transform.localPosition.y >= targetPos.y - 0.1f))
            {
                playerCam.transform.localPosition = new Vector3(targetPos.x, targetPos.y, playerCam.transform.position.z);
                moveToTarget = false;
            }

            float step = moveSpeed * Time.deltaTime;

            playerCam.transform.localPosition = Vector2.MoveTowards(playerCam.transform.localPosition, new Vector3 (targetPos.x, targetPos.y, playerCam.transform.position.z), step);
            
            playerCam.transform.position = new Vector3(playerCam.transform.position.x, playerCam.transform.position.y, -9);
        }


        //Offsetting Ship Rotation
        if (offsetShipRotation)
        {
            playerCam.transform.rotation = Quaternion.Euler(playerCam.transform.rotation.x - playerShip.transform.rotation.x, playerCam.transform.rotation.y - playerShip.transform.rotation.y, playerCam.transform.rotation.z - playerShip.transform.rotation.z);
        } else if (!offsetShipRotation && playerCam.transform.rotation != originalRotation) {
            playerCam.transform.rotation = originalRotation;
        }
    }

    public void ZoomCameraOut(float _targetSize)
    {
        targetSize = _targetSize;
        zoomToTarget = true;
    }
    public void RevertToOrigonalSize()
    {
        targetSize = originalSize;
        zoomToTarget = true;
    }

    public void MoveCameraPosition(Vector2 _targetPosition)
    {
        targetPos = _targetPosition;
        moveToTarget = true;
    }
    public void RevertToOrigonalPosition()
    {
        targetPos = originalPos;
        moveToTarget = true;
    }

    public void SetOffsetShipRotation(bool _offsetBool)
    {
        offsetShipRotation = _offsetBool;
    }


    public void SetCullingMask(LayerMask mask)
    {
        playerCam.cullingMask = mask;
    }
    public void RevertCullingMask()
    {
        playerCam.cullingMask = originalLayerMask;
    }


    public void SetOrthographicSize(float _sizeValue)
    {
        playerCam.orthographicSize = _sizeValue;
    }
}
