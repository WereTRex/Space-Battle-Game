using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShipBullet : MonoBehaviour
{
    float moveSpeed;
    float damage;


    GameObject firingObject;

    [SerializeField] LayerMask mask;


    private void Update()
    {
        transform.position += transform.right * moveSpeed * Time.deltaTime;
    }


    public void SetupBullet(float _damage, float _moveSpeed, GameObject _firingObject)
    {
        damage = _damage;
        moveSpeed = _moveSpeed;
        firingObject = _firingObject;

        Debug.Log("I exist!");
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (mask == (mask | (1 << collision.gameObject.layer))) { return; } //Stops the shooter from hitting specified layers

        //Check if the collision is a target collider

        // Deal damage
        if (collision.gameObject.TryGetComponent(out EnemyHealth enemyHealth))
        {
            enemyHealth.DealDamage(damage);
        }

        //Destroy this bullet
        Debug.Log("Player's hit: " + collision.gameObject.name);
        Destroy(this.gameObject);
    }
}
