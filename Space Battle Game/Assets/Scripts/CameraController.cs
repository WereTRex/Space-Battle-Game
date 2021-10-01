using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("General Variables")]
    [SerializeField] Camera playerCam;
    [Space(20)]


    [Header("Moving Variables")]
    [SerializeField] Vector2 targetPos;
    bool moveToTarget;

    [Space(5)]

    [SerializeField] float moveSpeed;
    [SerializeField] [Range(0f, 1f)] float movementSmoothingValue;
    [Space(20)]


    [Header("Zoom Variables")]
    [SerializeField] float targetSize;
    bool zoomToTarget;

    [Space(5)]

    [SerializeField] float zoomRate;
    [SerializeField] [Range(0f, 1f)] float zoomSmoothingValue;

    [Space(5)]

    [SerializeField] float origonalSize;

    private void Start()
    {
        if (origonalSize == 0)
        {
            origonalSize = playerCam.orthographicSize;
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

            Debug.Log("Finished Smoothing Calcualtions");

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

            Debug.Log("Finished zooming for this frame");
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

            if ((playerCam.transform.position.x <= targetPos.x + 0.1f && playerCam.transform.position.x >= targetPos.x - 0.1f)
                && (playerCam.transform.position.y <= targetPos.y + 0.1f && playerCam.transform.position.y >= targetPos.y - 0.1f))
            {
                playerCam.transform.position = targetPos;
                moveToTarget = false;
            }

            float step = moveSpeed * Time.deltaTime;

            playerCam.transform.position = Vector2.MoveTowards(playerCam.transform.position, targetPos, step);
        }
    }

    public void ZoomCameraOut(float _targetSize)
    {
        targetSize = _targetSize;
        zoomToTarget = true;
    }

    public void RevertToOrigonalSize()
    {
        targetSize = origonalSize;
        zoomToTarget = true;
    }


    public void MoveCameraPosition(Vector2 _targetPosition)
    {
        targetPos = _targetPosition;
    }

    public void SetOrthographicSize(float _sizeValue)
    {
        playerCam.orthographicSize = _sizeValue;
    }
}
