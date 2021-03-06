using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;

//Following Brackey's tutorial (Also has animations, so look there when implimenting that): https://www.youtube.com/watch?v=whzomFgjT50
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    Vector2 movementInput = Vector2.zero;

    public bool inMenu;

    [SerializeField] Transform GFX;
    [SerializeField] float rotationSpeed;
    
    [SerializeField] Transform playerShip;

    [Space(10)]
    [Header("Audio")]

    [SerializeField] AudioSource walkingSource;
    [SerializeField] float pitchRandomness = 0.1f;

    #region New Input System
    public void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }
    #endregion

    private void Awake()
    {
        playerShip = transform.parent.parent;
    }


    private void FixedUpdate()
    {
        if (inMenu) { return; }

        //Movement
        Vector2 movementDirection = new Vector2(movementInput.x, movementInput.y);

        transform.Translate((movementDirection * moveSpeed * Time.fixedDeltaTime), Space.Self);

        //Rotation
        if (movementDirection != Vector2.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(Vector3.forward, movementDirection);
            toRotation = Quaternion.Euler(toRotation.eulerAngles.x, toRotation.eulerAngles.y, toRotation.eulerAngles.z + playerShip.rotation.eulerAngles.z);

            GFX.rotation = Quaternion.RotateTowards(GFX.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }


        //Movement Sound
        if (movementDirection != Vector2.zero && !walkingSource.isPlaying)
        {
            walkingSource.pitch = Random.Range(1f - pitchRandomness, 1f + pitchRandomness);
            walkingSource.Play();
        }
    }
}
