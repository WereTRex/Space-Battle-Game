using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShipBullet : MonoBehaviour
{
    float moveSpeed;
    float damage;


    GameObject firingObject;

    [SerializeField] LayerMask mask;


    private void Update()
    {
        transform.position += transform.right * moveSpeed * Time.deltaTime;
    }
    public void SetupBullet(float _damage, float _moveSpeed, Vector2 _targetPos, GameObject _firingObject)
    {
        moveSpeed = _moveSpeed;
        firingObject = _firingObject;
        damage = _damage;

        //Rotation (Thanks Kastenessen on Unity Answers)
        float angle = Mathf.Atan2(_targetPos.y - transform.position.y, _targetPos.x - transform.position.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Answer bellow 'Best Answer' in this: https://answers.unity.com/questions/50279/check-if-layer-is-in-layermask.html explains what the if statment bellow does
        if (mask == (mask | (1 << collision.gameObject.layer))) { return; } //Stops the shooter from hitting themself or hitting specified layers

        //Check if the collision is a target collider

        Debug.Log("Hit Player");

        // Deal damage
        if (collision.gameObject.TryGetComponent(out PlayerShip playerShip))
        {
            Debug.Log("Damaging");
            playerShip.DealDamage(damage);
        } else if (collision.gameObject.transform.parent.TryGetComponent(out playerShip)) {
            Debug.Log("Damaging");
            playerShip.DealDamage(damage);
        } else if (collision.gameObject.transform.parent.parent.TryGetComponent(out playerShip)) {
            Debug.Log("Damaging");
            playerShip.DealDamage(damage);
        }

        //Destroy this bullet
        Debug.Log("Enemy hit: " + collision.gameObject.name);
        Destroy(this.gameObject);
    }
}
