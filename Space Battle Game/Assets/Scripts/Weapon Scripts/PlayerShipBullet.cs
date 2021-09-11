using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShipBullet : MonoBehaviour
{
    float moveSpeed;

    private void Update()
    {
        transform.position += transform.right * moveSpeed * Time.deltaTime;
    }

    public void SetupBullet(float _moveSpeed)
    {
        moveSpeed = _moveSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player") || collision.transform.CompareTag("Player Ship")) { return; } //Stops the player's ship bullet from colliding with the ship it is fired from or from hitting the players

        //Check if the collision is a target collider
        // Deal damage

        //Destroy this bullet
        Debug.Log("We hit: " + collision.gameObject.name);
        Destroy(this.gameObject);
    }
}
