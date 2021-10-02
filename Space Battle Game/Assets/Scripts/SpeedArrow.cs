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

    float gradient;
    float angle;

    [SerializeField] Slider slider;
    [SerializeField] LineRenderer line;

    
    private void Start()
    {
        if (!helm)
        {
            helm = FindObjectOfType<Helm>();
        }
        
        slider.maxValue = maxLength;
    }


    void Update()
    {
        currentVelocity = -helm.GetVelocity();


        //length = Speed / MaxSpeed * MaxLength
        currentLength = currentVelocity.magnitude / helm.GetMaxSpeed() * maxLength;
        slider.value = currentLength;


        //Gradient
        CalculateGradient();
        CalculateAngle();

        slider.transform.parent.transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void CalculateGradient()
    {
        if (currentVelocity.x != 0)
        {
            gradient = currentVelocity.y / currentVelocity.x;
        }
    }

    void CalculateAngle()
    {
        if (currentVelocity.x > 0)
        {
            angle = Mathf.Atan(gradient);
            angle = (angle * 180) / Mathf.PI;
            if (angle < 0)
                angle += 360;

        } else if (currentVelocity.x < 0) {
            angle = Mathf.Atan(gradient);
            angle = (angle * 180) / Mathf.PI;
            angle = 180 + angle;
        }
    }
}