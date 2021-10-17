using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyMovement : MonoBehaviour
{
    //Movement
    [SerializeField] float acceleration;
    float currentSpeed;
    float step;
    [SerializeField] float maxSpeed;

    //Rotation
    float currentHeading;
    [SerializeField] float turnSpeed;
    [SerializeField] [Min(0)] float maxRotationSpeed;

    float gradient;
    float angleToPlayer;

    //Rigidbody Stuff
    [SerializeField] Rigidbody2D rb2D;
    
    //Other
    [SerializeField] Vector2 targetPos;
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
        //  Doesnt slow down, just stops instantly
        /*Vector3 prevPos = transform.position;

        currentSpeed += acceleration * Time.deltaTime; // instant speed
        step = currentSpeed * Time.deltaTime; // calculate distance to move
        transform.position = Vector3.MoveTowards(transform.position, targetPos, step);
    
        if (transform.position == prevPos)
        {
            currentSpeed = 0;
        }*/
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


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(targetPos, 5f);
    }
}
