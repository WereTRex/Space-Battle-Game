using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyDetectionUI : MonoBehaviour
{
    [SerializeField] List<GameObject> enemyList;
    [SerializeField] List<RectTransform> UIEnemyMarkers;

    [SerializeField] GameObject EnemyMarkerPrefab;

    [SerializeField] float orbitRadius;


    [SerializeField] GameObject canvas;


    Vector2 enemyPosition;

    float calculatedGradient;
    float calculatedAngle;

    int counter;


    #region OnEnable/Disable
    void OnEnable()
    {
        EnemySpawner.OnEnemyNumberChanged += EnemyNumbersChanged;
    }
    void OnDisable()
    {
        EnemySpawner.OnEnemyNumberChanged -= EnemyNumbersChanged;
    }
    #endregion

    
    private void Update()
    {
        if (UIEnemyMarkers.Count != enemyList.Count)
        {
            ReInstantiateEnemyMarkers();
        }

        counter = 0;
        foreach (GameObject enemy in enemyList)
        {
            if (enemy == null) { enemyList.Remove(enemy); return; }
            
            //Get Enemy Positions
            enemyPosition = GetEnemyPosition(enemy);


            CalculateGradient(enemyPosition); //Calculate Gradient
            CalculateAngle(enemyPosition); //Calculate Angles


            //Display Angles by rotating UI elements around a circle
            RotateEnemyMarkers(calculatedAngle, counter);

            //Incriment the counter
            counter += 1;
        }
    }


    Vector2 GetEnemyPosition(GameObject enemy)
    {
        return enemy.transform.position;
    }

    void CalculateGradient(Vector2 _enemyPosition)
    {
        if (_enemyPosition.x != 0)
        {
            calculatedGradient = _enemyPosition.y / _enemyPosition.x;
        }
    }

    void CalculateAngle(Vector2 _enemyPosition)
    {
        if (_enemyPosition.x > 0)
        {
            calculatedAngle = Mathf.Atan(calculatedGradient);
            calculatedAngle = (calculatedAngle * 180) / Mathf.PI;
            if (calculatedAngle < 0)
                calculatedAngle += 360;

        }
        else if (_enemyPosition.x < 0)
        {
            calculatedAngle = Mathf.Atan(calculatedGradient);
            calculatedAngle = (calculatedAngle * 180) / Mathf.PI;
            calculatedAngle = 180 + calculatedAngle;
        }
    }

    void RotateEnemyMarkers(float angle, int markerNumber)
    {
        //Reset the position & rotation
        UIEnemyMarkers[markerNumber].anchoredPosition = new Vector2(orbitRadius, 0);
        UIEnemyMarkers[markerNumber].rotation = Quaternion.Euler(0, 0, 0);
        //Rotate to the desired angle
        UIEnemyMarkers[markerNumber].RotateAround(canvas.transform.position, Vector3.forward, angle);
    }


    void EnemyNumbersChanged()
    {
        Debug.Log("The number of enemies has changed");
        FindEnemies();
        ReInstantiateEnemyMarkers();
    }

    void ReInstantiateEnemyMarkers()
    {
        //Clear the current Markers
        foreach (Transform marker in UIEnemyMarkers)
        {
            Destroy(marker.gameObject);
        }
        UIEnemyMarkers.Clear();
        
        //Instantiates a new Enemy Marker Prefab & adds it to the UIEnemyMarkers list at the same time
        for (int i = 0; i < enemyList.Count; i++)
            UIEnemyMarkers.Add(Instantiate(EnemyMarkerPrefab, this.transform).GetComponent<RectTransform>());
    }

    void FindEnemies()
    {
        enemyList.Clear();
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            enemyList.Add(enemy);
        }
    }
}
