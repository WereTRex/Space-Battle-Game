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
        healthScript = GetComponent<EnemyHealth>();
        
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
            Debug.Log("Attack Target"); 
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

    }


    //Repair
    void Repair()
    {
        //Repair the hull
        healthScript.StartStopRepairingHull(true);
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
        //Choose point within map bounds
        if (Vector2.Distance(transform.position, targetPos) > 10)
            targetPos = new Vector2(Random.Range(-mapBounds.x, mapBounds.x), Random.Range(-mapBounds.y, mapBounds.y));
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
            if (weapon.range > furthestWeaponRange)
                furthestWeaponRange = weapon.range;
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
