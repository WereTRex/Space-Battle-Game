using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShipBullet : MonoBehaviour
{
    float moveSpeed;
    float damage;


    GameObject firingObject;

    private void Update()
    {
        transform.position += transform.right * moveSpeed * Time.deltaTime;
    }


    public void SetupBullet(float _moveSpeed, GameObject _firingObject)
    {
        moveSpeed = _moveSpeed;
        firingObject = _firingObject;
    }
    public void SetupBullet(float _moveSpeed, Vector2 _targetPos, GameObject _firingObject)
    {
        moveSpeed = _moveSpeed;
        firingObject = _firingObject;

        transform.LookAt(_targetPos);
        transform.rotation = new Quaternion(0, 0, -transform.rotation.x, transform.rotation.w);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == firingObject || (firingObject.GetComponent<PlayerShip>() && collision.CompareTag("Player"))) { return; } //Stops the shooter from hitting themself or the player ship hitting the players

        //Check if the collision is a target collider
        // Deal damage

        //Destroy this bullet
        Debug.Log("We hit: " + collision.gameObject.name);
        Destroy(this.gameObject);
    }
}
