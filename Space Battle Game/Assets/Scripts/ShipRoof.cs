using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipRoof : MonoBehaviour
{
    [SerializeField] bool fadeIn;
    [SerializeField] float fadeInDuration;
    int fadeInRequests = 0;

    [SerializeField] bool fadeOut;
    [SerializeField] float fadeOutDuration;

    [SerializeField] SpriteRenderer shipRoofRenderer;

    void Update()
    {
        // v=d/t
        float fadeInSpeed = 1 / fadeInDuration;
        float fadeOutSpeed = 1 / fadeOutDuration;

        if (fadeIn)
        {
            Color tempColor = shipRoofRenderer.color;
            tempColor.a += fadeInSpeed * Time.deltaTime;
            shipRoofRenderer.color = tempColor;
        }
        else if (fadeOut)
        {
            Color tempColor = shipRoofRenderer.color;
            if (shipRoofRenderer.color.a >= 0.99f)
            {
                tempColor.a = 0.99f;
                shipRoofRenderer.color = tempColor;
            }


            tempColor.a -= fadeOutSpeed * Time.deltaTime;
            shipRoofRenderer.color = tempColor;
        }


        if (shipRoofRenderer.color.a > 1)
        {
            Color tempColor = shipRoofRenderer.color;
            tempColor.a = 255;
            shipRoofRenderer.color = tempColor;
        } else if (shipRoofRenderer.color.a < 0) {
            Color tempColor = shipRoofRenderer.color;
            tempColor.a = 0;
            shipRoofRenderer.color = tempColor;

            fadeOut = false;
        }


        if (fadeInRequests > 0)
            fadeIn = true;
        else
            fadeIn = false;
    }


    public void StartFadeIn()
    {
        fadeInRequests += 1;
    }
    public void StartFadeOut()
    {
        fadeInRequests -= 1;
        fadeOut = true;
    }

    public void SetAlpha(float _alpha)
    {
        Color tempColor = shipRoofRenderer.color;
        tempColor.a = _alpha;
        shipRoofRenderer.color = tempColor;
    }
}
