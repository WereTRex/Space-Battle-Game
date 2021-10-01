using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedArrow : MonoBehaviour
{
    [SerializeField] Helm helm;

    float currentLength;
    [SerializeField] float maxLength;

    Vector2 currentVelocity;
    Vector2 currentPosition;
    Vector2 previousPosition;

    [SerializeField] Slider slider;
    [SerializeField] LineRenderer line;

    
    private void Start()
    {
        if (!helm)
        {
            helm = FindObjectOfType<Helm>();
            previousPosition = helm.GetPosition();
        }
            
            

        slider.maxValue = maxLength;

    }

    void Update()
    {
        currentVelocity = helm.GetVelocity();
        Vector2 currentDirection = (currentPosition - previousPosition).normalized;
        Debug.Log("currentDirection - " + currentDirection);


        //length = Speed / MaxSpeed * MaxLength
        currentLength = currentVelocity.magnitude / helm.GetMaxSpeed() * maxLength;
        slider.value = currentLength;


        //Gradient

        /*slider.transform.parent.transform.rotation = Quaternion.Euler(0, 0, (slider.transform.rotation.x + Vector2.Dot(new Vector2(1, 1).normalized, currentVelocity.normalized)) * 360);
        
        Debug.Log(Vector2.Angle(Vector2.zero.normalized, currentVelocity.normalized));
        Debug.Log("Dot - " + Vector2.Dot(new Vector2 (1, 1), currentVelocity.normalized));
        Debug.Log("currentVelocity.normalized - " + currentVelocity.normalized);
        Debug.Log("currentVelocity - " + currentVelocity);*/

        previousPosition = helm.GetPosition();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector3(0, 0, -10), (currentVelocity.normalized * currentLength));
    }
}