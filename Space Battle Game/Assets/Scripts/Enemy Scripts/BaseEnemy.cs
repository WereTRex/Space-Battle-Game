using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Note: Nav mesh agents don't work in 2D, crap
//Note 2: Look at this: https://www.youtube.com/watch?v=SDfEytEjb5o. I would the now but its nearing 00:00
[RequireComponent(typeof(EnemyHealth))]
public class BaseEnemy : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] [Min(0)] float moveToTargetRandomness;
    [SerializeField] [Min(0.1f)] float moveToTargetChangeTime;
    float moveToTargetChangeTimeRemaining;

    [Space(20)]

    [Header("Hull")]
    float currentHullHealth;
    float maxHullHealth;
    float hullHealthPercentage;

    [Space(20)]

    [Header("Shields")]
    float currentSheildHealth;
    float maxShieldHealth;
    float shieldPercentage;

    [Space(20)]

    [Header("Retreating")]
    [SerializeField] float fleeRange;
    [SerializeField] float detectionRange;

    [Min(0)] float maxRandomTargetValue;

    [Space(20)]

    [Header("Reparing")]
    [SerializeField] [Tooltip ("The time taken to repair 'repairAmount' amount of health")] float repairTime;
    float repairTimeRemaining;
    [SerializeField] float repairAmount;

    [Space(20)]

    [Header("Weapons")]
    [SerializeField] Weapon[] weapons;
    [SerializeField] float furthestWeaponRange;
    int previousWeaponCount;

    [Space(20)]


    [Header("Other")]
    [SerializeField] EnemyHealth healthScript;
    [SerializeField] EnemyMovement movementScript;

    [Space(5)]

    [SerializeField] Transform playerShip;
    [SerializeField] LayerMask linecastMask;

    [Space(5)]

    [SerializeField] Vector2 mapBounds;

    [Space(5)]

    [SerializeField] Vector2 targetPos;
    Vector2 tempTargetPos;

    void Start()
    {
        if (!healthScript) { healthScript = GetComponent<EnemyHealth>(); }
        if (!movementScript) { movementScript = GetComponent<EnemyMovement>(); }
        
        currentHullHealth = maxHullHealth;
        currentSheildHealth = maxShieldHealth;
    }

    void Awake()
    {
        GetFurthestWeaponRange();
        CheckMapBounds();
    }

    void Update()
    {
        CheckHullHealth();
        CheckShieldHealth();

        if (weapons.Length != previousWeaponCount)
        {
            GetFurthestWeaponRange();
        }

        healthScript.StartStopRepairingHull(false);
        Transitions();

        previousWeaponCount = weapons.Length;
    }

    void Transitions()
    {
        if (hullHealthPercentage <= 15 && shieldPercentage <= 25 && Vector2.Distance(transform.position, playerShip.position) < fleeRange)
        {
            Debug.Log("Full Retreat");
            FullRetreat();
        }
        else if (hullHealthPercentage < 35 && shieldPercentage <= 0 && Vector2.Distance(transform.position, playerShip.position) < fleeRange)
        {
            Debug.Log("Retreat");
            Retreat();
        }
        else if (Vector2.Distance(transform.position, playerShip.position) < furthestWeaponRange)
        {
            //Debug.Log("Attack Target");

            MoveToTarget();
            AttackTarget();
        }
        else if (hullHealthPercentage < 60)
        {
            Debug.Log("Repair");
            Repair();
        }
        else if (Vector2.Distance(transform.position, playerShip.position) < detectionRange)
        {
            Debug.Log("Checking LOS");
            if (CheckLOSToTarget())
            {
                Debug.Log("Move To Target");
                MoveToTarget();
            }
        }
        else
        {
            Debug.Log("Idle");
            Idle();
        }


        //Movement
        movementScript.SetTarget(targetPos);


        //Weapon Cooldown
        foreach (Weapon weapon in weapons)
        {
            weapon.cooldownTimeRemaining -= 1 * Time.deltaTime;
        }
    }


    //Retreating
    void FullRetreat()
    {
        //Find gradient from player pos to current pos
        float m = (transform.position.y - playerShip.position.y) / (transform.position.x - playerShip.position.x);

        //Find angle to player
        float angle = 0;

        if ((transform.position.x - playerShip.position.x) > 0)
        {
            angle = Mathf.Atan(m);
            angle = (angle * 180) / Mathf.PI;
            if (angle < 0)
                angle += 360;
        }
        else if ((transform.position.x - playerShip.position.x) < 0)
        {
            angle = Mathf.Atan(m);
            angle = (angle * 180) / Mathf.PI;
            angle = 180 + angle;
        }

        //Set get pos
        var radians = angle * Mathf.Deg2Rad;
        var x = Mathf.Cos(radians);
        var y = Mathf.Sin(radians);
        var pos = new Vector2(x, y);
        pos = pos * (fleeRange * 1.1f);

        targetPos = pos;
    }

    void Retreat()
    {
        //Attack the target while fleeing
        AttackTarget();


        //Retreat
        //  Find gradient from player pos to current pos
        float m = (transform.position.y - playerShip.position.y) / (transform.position.x - playerShip.position.x);

        //  Find angle to player
        float angle = 0;

        if ((transform.position.x - playerShip.position.x) > 0)
        {
            angle = Mathf.Atan(m);
            angle = (angle * 180) / Mathf.PI;
            if (angle < 0)
                angle += 360;
        }
        else if ((transform.position.x - playerShip.position.x) < 0)
        {
            angle = Mathf.Atan(m);
            angle = (angle * 180) / Mathf.PI;
            angle = 180 + angle;
        }

        //  Get pos
        var radians = angle * Mathf.Deg2Rad;
        var x = Mathf.Cos(radians);
        var y = Mathf.Sin(radians);
        var pos = new Vector2(x, y);
        pos = pos * (fleeRange * 1.1f);

        //  Set Target Pos
        targetPos = pos;
    }

    //Attack Target
    void AttackTarget()
    {
        GameObject pfBullet;
        
        //Run through each weapon
        foreach(Weapon weapon in weapons)
        {
            if (weapon.cooldownTimeRemaining > 0) { return; }
            

            //  Calculate, using the weapon's speed, where you'd need to aim for to hit the player based on their current speed
            float timeToReachPlayer = Vector2.Distance(transform.position, playerShip.position) * weapon.bulletSpeed;
            Vector2 bulletTarget = new Vector2(playerShip.position.x + (playerShip.gameObject.GetComponent<Rigidbody2D>().velocity.x * timeToReachPlayer), playerShip.position.y + (playerShip.gameObject.GetComponent<Rigidbody2D>().velocity.y * timeToReachPlayer));

            Debug.Log("Bullet Target: " + bulletTarget);
            Debug.Log("Velocity: " + playerShip.gameObject.GetComponent<Rigidbody2D>().velocity.x);

            //  Instantiate the bullet
            pfBullet = Instantiate(weapon.bulletPrefab, transform.position, transform.rotation);
            pfBullet.GetComponent<PlayerShipBullet>().SetupBullet(weapon.bulletSpeed, bulletTarget, this.gameObject);

            weapon.cooldownTimeRemaining = weapon.cooldownTime;

            Destroy(pfBullet, weapon.lifeTime);
        }
    }


    //Repair
    void Repair()
    {
        //Repair the hull
        healthScript.StartStopRepairingHull(true);

        targetPos = transform.position;
    }


    //Move To Target
    void MoveToTarget()
    {
        if (moveToTargetChangeTimeRemaining <= 0)
        {
            targetPos = new Vector2(playerShip.position.x + Random.Range(-moveToTargetRandomness, moveToTargetRandomness), playerShip.position.y + Random.Range(-moveToTargetRandomness, moveToTargetRandomness));
            moveToTargetChangeTimeRemaining = moveToTargetChangeTime;
        } else {
            moveToTargetChangeTimeRemaining -= 1 * Time.deltaTime;
        }
    }


    //Idle
    void Idle()
    {
        //Only choose new points if you are close to the current point
        if (Vector2.Distance(transform.position, targetPos) < 10)
            targetPos = new Vector2(Random.Range(-mapBounds.x, mapBounds.x), Random.Range(-mapBounds.y, mapBounds.y)); //Choose point within map bounds
    }



    void CheckMapBounds()
    {

    }

    void CheckHullHealth()
    {
        //Get hull health
        healthScript.GetHull(out currentHullHealth, out maxHullHealth);

        //Calculate hull percentage
        hullHealthPercentage = (currentHullHealth / maxHullHealth) * 100f;
    }
    void CheckShieldHealth()
    {
        //Get current shields
        healthScript.GetShields(out currentSheildHealth, out maxShieldHealth);

        //Calculate shield percentage
        shieldPercentage = (currentSheildHealth / maxShieldHealth) * 100f;
    }

    void GetFurthestWeaponRange()
    {
        foreach(Weapon weapon in weapons)
        {
            if (weapon.fireRange > furthestWeaponRange)
                furthestWeaponRange = weapon.fireRange;
        }
    }

    bool CheckLOSToTarget()
    {
        bool foundLOS = false;

        RaycastHit rayHit;
        if (!Physics.Linecast(transform.position, playerShip.position, out rayHit, linecastMask, QueryTriggerInteraction.Ignore))
        {
            foundLOS = true;
        }

        Debug.Log(foundLOS);

        return foundLOS;
    }



    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, fleeRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, furthestWeaponRange);

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(targetPos, 10f);
    }
}
