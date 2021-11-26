using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestOfLevel : MonoBehaviour
{
    public delegate void RestOfLevelMoved(float deltaX, float deltaY);
    public static event RestOfLevelMoved OnRestOfLevelMoved;


    float xChange;
    float yChange;

    Vector2 previousCoords;

    private void Update()
    {
        if ((this.transform.position.x != previousCoords.x) || (this.transform.position.y != previousCoords.y))
        {
            xChange = transform.position.x - previousCoords.x;
            yChange = transform.position.y - previousCoords.y;

            Debug.Log("Rest Of Level has moved");
            OnRestOfLevelMoved?.Invoke(xChange, yChange);
        }

        previousCoords = transform.position;
    }
}
