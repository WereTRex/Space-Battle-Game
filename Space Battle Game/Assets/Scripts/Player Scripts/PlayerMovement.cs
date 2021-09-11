using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//Following Brackey's tutorial (Also has animations, so look there when implimenting that): https://www.youtube.com/watch?v=whzomFgjT50
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D rb;

    Vector2 movementInput = Vector2.zero;

    public bool inMenu;

    #region New Input System
    public void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }
    #endregion

    private void FixedUpdate()
    {
        if (inMenu) { return; }

        //Movement
        rb.MovePosition(rb.position + movementInput * moveSpeed * Time.fixedDeltaTime);
    }
}
