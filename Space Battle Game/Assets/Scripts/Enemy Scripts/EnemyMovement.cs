using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyMovement : MonoBehaviour
{
    //Movement
    [SerializeField] float acceleration;
    [SerializeField] float maxSpeed;

    float previousVelocity;
    float stopDistance;
    bool decelerate;

    //Rotation
    [SerializeField] float turnSpeed;
    [SerializeField] [Min(0)] float maxRotationSpeed;

    float gradient;
    float angleToPlayer;

    //Rigidbody Stuff
    [SerializeField] Rigidbody2D rb2D;
    
    //Other
    [SerializeField] Vector2 targetPos;
    Vector2 previousTargetPos;

    Vector2 localTargetPos;


    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        //Rotation
        ConvertTargetPosToLocal();
        CalculateGradient();
        CalculateAngle();


        if (!(transform.eulerAngles.z >= angleToPlayer - 5 && transform.eulerAngles.z <= angleToPlayer + 5))
        {
            if (Mathf.DeltaAngle(angleToPlayer, transform.eulerAngles.z) < 0)
                transform.Rotate(new Vector3(0, 0, 1), turnSpeed * Time.deltaTime);
            else
                transform.Rotate(new Vector3(0, 0, 1), -turnSpeed * Time.deltaTime);
        }


        //Movement

        if (rb2D.velocity.magnitude != previousVelocity || targetPos != previousTargetPos)
        {
            stopDistance = 0;
            int steps = Mathf.RoundToInt(rb2D.velocity.magnitude / (acceleration / (rb2D.mass / 10)));

            for (int i = 0; i < steps; i++)
            {
                stopDistance += (i + 1) * rb2D.velocity.magnitude;
            }

            decelerate = (Vector2.Distance(transform.position, targetPos) <= stopDistance);
        }

        if (decelerate)
        {
            rb2D.velocity *= 1 - (0.95f * Time.deltaTime);
        } else {
            rb2D.AddForce(transform.right * acceleration);
        }

        rb2D.velocity = new Vector2(Mathf.Clamp(rb2D.velocity.x, -maxSpeed, maxSpeed), Mathf.Clamp(rb2D.velocity.y, -maxSpeed, maxSpeed));


        previousVelocity = rb2D.velocity.magnitude;
        previousTargetPos = targetPos;
    }


    void ConvertTargetPosToLocal()
    {
        localTargetPos = new Vector2 (targetPos.x - transform.position.x, targetPos.y - transform.position.y);
    }

    void CalculateGradient()
    {
        if (localTargetPos.x != 0)
        {
            gradient = localTargetPos.y / localTargetPos.x;
        }
    }

    void CalculateAngle()
    {
        if (localTargetPos.x > 0)
        {
            angleToPlayer = Mathf.Atan(gradient);
            angleToPlayer = (angleToPlayer * 180) / Mathf.PI;
            if (angleToPlayer < 0)
                angleToPlayer += 360;

        }
        else if (localTargetPos.x < 0)
        {
            angleToPlayer = Mathf.Atan(gradient);
            angleToPlayer = (angleToPlayer * 180) / Mathf.PI;
            angleToPlayer = 180 + angleToPlayer;
        }
    }


    public void SetTarget(Vector2 _targetPos)
    {
        targetPos = _targetPos;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(targetPos, 5f);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(targetPos, stopDistance);
    }
}
